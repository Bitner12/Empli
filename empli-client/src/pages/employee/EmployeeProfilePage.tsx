import { useEffect, useState } from 'react'
import * as api from '../../api/empliApi'
import type { EmployeeProfileResponse } from '../../api/types'

export function EmployeeProfilePage() {
  const [emp, setEmp] = useState<EmployeeProfileResponse | null>(null)
  const [loading, setLoading] = useState(true)
  const [err, setErr] = useState<string | null>(null)
  const [fn, setFn] = useState('')
  const [ln, setLn] = useState('')
  const [rate, setRate] = useState(0)

  useEffect(() => {
    void (async () => {
      try {
        const data = await api.getEmployee()
        setEmp(data)
        setFn(data.firstName ?? '')
        setLn(data.lastName ?? '')
        setRate(data.costPerHour ?? 0)
      } catch {
        setErr('Не удалось загрузить профиль')
      } finally {
        setLoading(false)
      }
    })()
  }, [])

  async function saveProfile(e: React.FormEvent) {
    e.preventDefault()
    if (!emp || emp.isLinkedToCompany) return
    setErr(null)
    try {
      const updated = await api.updateEmployee({
        firstName: fn.trim(),
        lastName: ln.trim(),
        pesel: emp.pesel ?? undefined,
        costPerHour: rate,
      })
      setEmp(updated)
    } catch {
      setErr('Сохранение не удалось')
    }
  }

  if (loading) return <p className="muted">Загрузка…</p>
  if (err && !emp) return <p className="error">{err}</p>
  if (!emp) return null

  return (
    <>
      <h1>Мой профиль</h1>

      {emp.isLinkedToCompany ? (
        <section className="card">
          <dl className="profile-dl">
            <dt>Имя</dt>
            <dd>{emp.firstName}</dd>
            <dt>Фамилия</dt>
            <dd>{emp.lastName}</dd>
            <dt>PESEL</dt>
            <dd>{emp.pesel}</dd>
            <dt>Ставка / час</dt>
            <dd>{emp.costPerHour != null ? `${emp.costPerHour}` : '—'}</dd>
          </dl>
        </section>
      ) : (
        <section className="card narrow-card">
          <form onSubmit={saveProfile} className="form-stack">
            {err && <p className="error">{err}</p>}
            <label>
              Имя
              <input value={fn} onChange={(e) => setFn(e.target.value)} required />
            </label>
            <label>
              Фамилия
              <input value={ln} onChange={(e) => setLn(e.target.value)} required />
            </label>
            <label>
              Ставка / час
              <input
                type="number"
                step="0.01"
                min={0}
                value={rate}
                onChange={(e) => setRate(Number(e.target.value))}
              />
            </label>
            <button type="submit" className="btn">
              Сохранить
            </button>
            <p className="muted small">PESEL: {emp.pesel}</p>
          </form>
        </section>
      )}
    </>
  )
}
