import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import * as api from '../../api/empliApi'
import type { Worker } from '../../api/types'
import { localDateInputString } from '../../utils/dateLocal'

function toDateInput(iso: string) {
  return iso.slice(0, 10)
}

function filterHours(worker: Worker, start: string, end: string): Worker {
  const hs = worker.hours ?? []
  const filtered = hs.filter((h) => {
    const d = toDateInput(h.date)
    return d >= start && d <= end
  })
  return { ...worker, hours: filtered }
}

export function CompanyWorkersPage() {
  const [workers, setWorkers] = useState<Worker[]>([])
  const [loading, setLoading] = useState(true)
  const [err, setErr] = useState<string | null>(null)

  const [fStart, setFStart] = useState(() => {
    const d = new Date()
    d.setDate(1)
    return localDateInputString(d)
  })
  const [fEnd, setFEnd] = useState(() => localDateInputString())

  const load = useCallback(async () => {
    setErr(null)
    try {
      const w = await api.getCompanyWorkers()
      setWorkers(w ?? [])
    } catch {
      setErr('Не удалось загрузить список работников')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void load()
  }, [load])

  const filteredWorkers = useMemo(
    () => workers.map((w) => filterHours(w, fStart, fEnd)),
    [workers, fStart, fEnd],
  )

  if (loading) return <p className="muted">Загрузка…</p>

  return (
    <>
      <h1>Сотрудники</h1>
      {err && <p className="error">{err}</p>}

      <section className="card">
        <h2 className="card-heading">Сводка часов за период</h2>
        <div className="row gap wrap filter-bar">
          <label>
            С даты
            <input type="date" value={fStart} onChange={(e) => setFStart(e.target.value)} />
          </label>
          <label>
            По дату
            <input type="date" value={fEnd} onChange={(e) => setFEnd(e.target.value)} />
          </label>
        </div>

        <div className="table-wrap">
          <table className="data-table">
            <thead>
              <tr>
                <th>Имя</th>
                <th>Часы (период)</th>
              </tr>
            </thead>
            <tbody>
              {filteredWorkers.length === 0 ? (
                <tr>
                  <td colSpan={2} className="muted">
                    Нет сотрудников
                  </td>
                </tr>
              ) : (
                filteredWorkers.map((w) => {
                  const sum = (w.hours ?? []).reduce((s, h) => s + h.hours, 0)
                  return (
                    <tr key={w.id}>
                      <td>
                        <Link to={`/company/workers/${w.id}`} className="name-link">
                          {w.firstName} {w.lastName}
                        </Link>
                      </td>
                      <td>{sum.toFixed(2)}</td>
                    </tr>
                  )
                })
              )}
            </tbody>
          </table>
        </div>
      </section>
    </>
  )
}
