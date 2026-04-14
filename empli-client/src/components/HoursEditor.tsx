import axios from 'axios'
import { useCallback, useEffect, useMemo, useState } from 'react'
import * as api from '../api/empliApi'
import type { Hour } from '../api/types'
import { isDateOnOrBeforeToday, localDateInputString } from '../utils/dateLocal'

function toDateInput(iso: string) {
  return iso.slice(0, 10)
}

function dayStartIso(dateInput: string) {
  return `${dateInput}T00:00:00`
}

function errorMessage(err: unknown, fallback: string): string {
  if (axios.isAxiosError(err)) {
    const d = err.response?.data
    if (typeof d === 'string') return d
    if (d && typeof d === 'object' && 'detail' in d) return String((d as { detail: string }).detail)
    if (d && typeof d === 'object' && 'title' in d) return String((d as { title: string }).title)
  }
  return fallback
}

function isConflictStatus(err: unknown): boolean {
  return axios.isAxiosError(err) && err.response?.status === 409
}

type Props = {
  workerId: string
  title?: string
  allowDelete?: boolean
  /** Для расчёта суммы за период (PLN). Если не передано — показываются только часы. */
  hourlyRate?: number | null
}

export function HoursEditor({
  workerId,
  title = 'Рабочие часы',
  allowDelete = true,
  hourlyRate,
}: Props) {
  const today = localDateInputString()

  const [start, setStart] = useState(() => {
    const d = new Date()
    d.setDate(1)
    return localDateInputString(d)
  })
  const [end, setEnd] = useState(() => localDateInputString())
  const [hours, setHours] = useState<Hour[]>([])
  const [loading, setLoading] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [newDate, setNewDate] = useState(() => localDateInputString())
  const [newHours, setNewHours] = useState(8)

  const load = useCallback(async () => {
    if (!workerId) return
    setLoading(true)
    setErr(null)
    try {
      const ds = dayStartIso(start)
      const de = dayStartIso(end)
      const data = await api.getHoursPeriod(workerId, ds, de)
      setHours(
        [...data].sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()),
      )
    } catch (e) {
      setErr(errorMessage(e, 'Не удалось загрузить часы'))
      setHours([])
    } finally {
      setLoading(false)
    }
  }, [workerId, start, end])

  const totalHours = useMemo(() => hours.reduce((s, h) => s + h.hours, 0), [hours])
  const totalEarned =
    hourlyRate != null && Number.isFinite(hourlyRate) ? totalHours * hourlyRate : null

  useEffect(() => {
    void load()
  }, [load])

  async function addRow(e: React.FormEvent) {
    e.preventDefault()
    setErr(null)
    if (!isDateOnOrBeforeToday(newDate)) {
      setErr('Можно добавлять записи только за сегодня и прошедшие дни.')
      return
    }

    const isoForApi = dayStartIso(newDate)
    const existingInList = hours.find((h) => toDateInput(h.date) === newDate)
    if (existingInList) {
      if (
        !window.confirm(
          `На дату ${newDate} уже есть запись (${existingInList.hours} ч.). Заменить значение на ${newHours} ч.?`,
        )
      ) {
        return
      }
      try {
        await api.updateHour(workerId, existingInList.date, newHours)
        await load()
      } catch (err) {
        setErr(errorMessage(err, 'Не удалось обновить запись'))
      }
      return
    }

    try {
      await api.createHour(workerId, isoForApi, newHours)
      await load()
    } catch (e) {
      if (isConflictStatus(e)) {
        const serverText = errorMessage(
          e,
          'На эту дату уже есть запись учёта часов.',
        )
        if (
          !window.confirm(
            `${serverText}\n\nЗаменить существующую запись на ${newHours} ч.?`,
          )
        ) {
          return
        }
        try {
          const rows = await api.getHoursPeriod(workerId, isoForApi, isoForApi)
          const row = rows[0]
          if (!row) {
            setErr('Не удалось найти существующую запись для обновления.')
            return
          }
          await api.updateHour(workerId, row.date, newHours)
          await load()
        } catch (err) {
          setErr(errorMessage(err, 'Не удалось обновить запись'))
        }
        return
      }
      setErr(errorMessage(e, 'Не удалось добавить запись'))
    }
  }

  async function patchRow(h: Hour, value: number) {
    setErr(null)
    if (!isDateOnOrBeforeToday(toDateInput(h.date))) {
      setErr('Нельзя менять записи с датой позже сегодняшнего дня.')
      return
    }
    try {
      await api.updateHour(workerId, h.date, value)
      await load()
    } catch (e) {
      setErr(errorMessage(e, 'Не удалось обновить'))
    }
  }

  async function removeRow(h: Hour) {
    if (!allowDelete) return
    if (!window.confirm('Удалить запись часов?')) return
    setErr(null)
    try {
      await api.deleteHour(workerId, h.date)
      await load()
    } catch (e) {
      setErr(errorMessage(e, 'Не удалось удалить'))
    }
  }

  return (
    <section className="card hours-editor-card">
      <h2 className="card-heading">{title}</h2>
      <div className="row gap wrap filter-bar">
        <label>
          С&nbsp;даты
          <input type="date" value={start} onChange={(e) => setStart(e.target.value)} />
        </label>
        <label>
          По&nbsp;дату
          <input type="date" value={end} onChange={(e) => setEnd(e.target.value)} />
        </label>
      </div>
      <div className="period-totals" aria-live="polite">
        <span>
          Итого за период:{' '}
          <strong>{totalHours.toFixed(2)}</strong> ч.
        </span>
        {totalEarned != null && (
          <span>
            Заработано: <strong>{totalEarned.toFixed(2)}</strong> PLN
          </span>
        )}
      </div>
      {loading && <p className="muted small">Обновление…</p>}
      {err && <p className="error">{err}</p>}

      <div className="hours-add-block">
        <h3 className="subheading">Новая запись</h3>
        <form className="row gap wrap" onSubmit={addRow}>
          <label>
            Дата
            <input
              type="date"
              value={newDate}
              max={today}
              onChange={(e) => setNewDate(e.target.value)}
              required
            />
          </label>
          <label>
            Часы
            <input
              type="number"
              step="0.25"
              min={0}
              max={24}
              value={newHours}
              onChange={(e) => setNewHours(Number(e.target.value))}
              required
            />
          </label>
          <button type="submit" className="btn">
            Добавить
          </button>
        </form>
      </div>

      <div className="table-wrap">
        <table className="data-table">
          <thead>
            <tr>
              <th>Дата</th>
              <th>Часы</th>
              {allowDelete && <th />}
            </tr>
          </thead>
          <tbody>
            {hours.length === 0 ? (
              <tr>
                <td colSpan={allowDelete ? 3 : 2} className="muted">
                  Нет записей за период
                </td>
              </tr>
            ) : (
              hours.map((h) => {
                const rowDate = toDateInput(h.date)
                const canEdit = isDateOnOrBeforeToday(rowDate)
                return (
                  <tr key={`${h.date}-${h.id}`}>
                    <td>{rowDate}</td>
                    <td>
                      <input
                        type="number"
                        step="0.25"
                        min={0}
                        max={24}
                        defaultValue={h.hours}
                        disabled={!canEdit}
                        title={!canEdit ? 'Запись в будущем — недоступна для правок' : undefined}
                        onBlur={(e) => {
                          const v = Number(e.target.value)
                          if (v !== h.hours) void patchRow(h, v)
                        }}
                      />
                    </td>
                    {allowDelete && (
                      <td>
                        <button
                          type="button"
                          className="btn ghost danger"
                          disabled={!canEdit}
                          onClick={() => void removeRow(h)}
                        >
                          Удалить
                        </button>
                      </td>
                    )}
                  </tr>
                )
              })
            )}
          </tbody>
        </table>
      </div>
    </section>
  )
}
