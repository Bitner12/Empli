import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import * as api from '../../api/empliApi'
import type { Contractor, Hour } from '../../api/types'
import { localDateInputString } from '../../utils/dateLocal'

function toDateInput(iso: string) {
  return iso.slice(0, 10)
}

export function CompanyContractorDetailPage() {
  const { contractorId } = useParams<{ contractorId: string }>()

  const [contractor, setContractor] = useState<Contractor | null>(null)
  const [hours, setHours] = useState<Hour[]>([])
  const [initLoading, setInitLoading] = useState(true)
  const [hoursLoading, setHoursLoading] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  const [editName, setEditName] = useState('')
  const [editRate, setEditRate] = useState('')
  const [editOk, setEditOk] = useState<string | null>(null)
  const [showDetails, setShowDetails] = useState(false)

  const [start, setStart] = useState(() => {
    const d = new Date()
    d.setDate(1)
    return localDateInputString(d)
  })
  const [end, setEnd] = useState(() => localDateInputString())

  // keep latest start/end for the hours loader without recreating the callback
  const startRef = useRef(start)
  const endRef = useRef(end)
  startRef.current = start
  endRef.current = end

  const loadContractor = useCallback(async () => {
    if (!contractorId) return
    try {
      const list = await api.getContractorsByCompany()
      const found = list.find((c) => c.id === contractorId) ?? null
      setContractor(found)
      if (found) {
        setEditName(found.name)
        setEditRate(String(found.ratePerHour))
      }
    } catch {
      setErr('Не удалось загрузить контрагента')
    }
  }, [contractorId])

  const loadHours = useCallback(async () => {
    if (!contractorId) return
    setHoursLoading(true)
    try {
      const data = await api.getHoursByContractor(
        contractorId,
        `${startRef.current}T00:00:00`,
        `${endRef.current}T00:00:00`,
      )
      setHours(data)
    } catch {
      setErr('Не удалось загрузить записи')
    } finally {
      setHoursLoading(false)
    }
  }, [contractorId])

  // initial load only
  useEffect(() => {
    void (async () => {
      setInitLoading(true)
      await loadContractor()
      await loadHours()
      setInitLoading(false)
    })()
  }, [loadContractor, loadHours])

  // reload hours when dates change — but NOT the whole page
  const isFirstMount = useRef(true)
  useEffect(() => {
    if (isFirstMount.current) {
      isFirstMount.current = false
      return
    }
    void loadHours()
  }, [start, end, loadHours])

  const totalHours = useMemo(() => hours.reduce((s, h) => s + h.hours, 0), [hours])

  // group records by date so entries of different workers on the same day
  // are shown as separate fields under one date
  const groupedByDate = useMemo(() => {
    const map = new Map<string, Hour[]>()
    for (const h of hours) {
      const d = toDateInput(h.date)
      const arr = map.get(d)
      if (arr) arr.push(h)
      else map.set(d, [h])
    }
    return Array.from(map.entries()).sort((a, b) => (a[0] < b[0] ? 1 : -1))
  }, [hours])

  async function saveContractor(e: React.FormEvent) {
    e.preventDefault()
    if (!contractorId) return
    setEditOk(null)
    setErr(null)
    try {
      await api.updateContractor(contractorId, editName.trim(), Number(editRate) || 0)
      setEditOk('Сохранено')
      await loadContractor()
    } catch {
      setErr('Не удалось сохранить изменения')
    }
  }

  if (!contractorId) return <p className="error">Некорректная ссылка</p>
  if (initLoading) return <p className="muted">Загрузка…</p>
  if (!contractor) return <p className="error">Контрагент не найден</p>

  return (
    <>
      <p className="cabinet-back">
        <Link to="/company/contractors">← К списку контрагентов</Link>
      </p>

      <div className="row gap wrap" style={{ justifyContent: 'space-between', alignItems: 'center' }}>
        <h1>{contractor.name}</h1>
        <button
          type="button"
          className="btn ghost"
          onClick={() => setShowDetails((v) => !v)}
        >
          {showDetails ? 'Скрыть данные контрагента' : 'Данные контрагента'}
        </button>
      </div>
      {err && <p className="error">{err}</p>}

      {showDetails && (
        <section className="card">
          <h2 className="card-heading">Данные контрагента</h2>
          {editOk && <p className="muted small">{editOk}</p>}
          <form className="row gap wrap" onSubmit={saveContractor}>
            <label style={{ flex: '2 1 180px' }}>
              Название
              <input
                value={editName}
                onChange={(e) => setEditName(e.target.value)}
                required
              />
            </label>
            <label style={{ flex: '1 1 120px' }}>
              Ставка (PLN/ч)
              <input
                type="number"
                step="0.01"
                min={0}
                value={editRate}
                onChange={(e) => setEditRate(e.target.value)}
                placeholder="0"
                required
              />
            </label>
            <button type="submit" className="btn" style={{ alignSelf: 'flex-end' }}>
              Сохранить
            </button>
          </form>
        </section>
      )}

      <section className="card hours-editor-card">
        <h2 className="card-heading">Записи часов</h2>

        <div className="row gap wrap filter-bar">
          <label>
            С&nbsp;даты
            <input
              type="date"
              value={start}
              onChange={(e) => setStart(e.target.value)}
            />
          </label>
          <label>
            По&nbsp;дату
            <input
              type="date"
              value={end}
              onChange={(e) => setEnd(e.target.value)}
            />
          </label>
        </div>

        <div className="period-totals" aria-live="polite">
          <span>
            Итого часов: <strong>{totalHours.toFixed(2)}</strong> ч.
          </span>
        </div>

        {hoursLoading && <p className="muted small">Обновление…</p>}

        {groupedByDate.length === 0 ? (
          <p className="muted">Нет записей за период</p>
        ) : (
          <div className="date-groups">
            {groupedByDate.map(([date, entries]) => (
              <div key={date} className="date-group">
                <div className="date-group-header">{date}</div>
                <div className="date-group-entries">
                  {entries.map((h) => (
                    <div key={h.id} className="worker-entry">
                      <span className="worker-entry-name">
                        {h.worker ? `${h.worker.firstName} ${h.worker.lastName}` : '—'}
                      </span>
                      <span className="worker-entry-hours">{h.hours.toFixed(2)} ч.</span>
                      {h.comment && <span className="worker-entry-comment">{h.comment}</span>}
                    </div>
                  ))}
                </div>
              </div>
            ))}
          </div>
        )}
      </section>
    </>
  )
}
