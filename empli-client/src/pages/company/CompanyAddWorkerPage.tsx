import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import * as api from '../../api/empliApi'

export function CompanyAddWorkerPage() {
  const nav = useNavigate()
  const [wFn, setWFn] = useState('')
  const [wLn, setWLn] = useState('')
  const [wPesel, setWPesel] = useState('')
  const [wRate, setWRate] = useState(50)
  const [err, setErr] = useState<string | null>(null)

  async function addWorker(e: React.FormEvent) {
    e.preventDefault()
    if (!/^\d{11}$/.test(wPesel)) {
      setErr('PESEL: 11 цифр')
      return
    }
    setErr(null)
    try {
      await api.addCompanyWorker({
        pesel: wPesel,
        firstName: wFn.trim(),
        lastName: wLn.trim(),
        costPerHour: wRate,
      })
      nav('/company', { replace: true })
    } catch {
      setErr('Не удалось добавить работника')
    }
  }

  return (
    <>
      <h1>Новый работник</h1>
      <section className="card narrow-card">
        <form onSubmit={addWorker} className="form-stack">
          {err && <p className="error">{err}</p>}
          <label>
            Имя
            <input value={wFn} onChange={(e) => setWFn(e.target.value)} required />
          </label>
          <label>
            Фамилия
            <input value={wLn} onChange={(e) => setWLn(e.target.value)} required />
          </label>
          <label>
            PESEL
            <input
              value={wPesel}
              onChange={(e) => setWPesel(e.target.value.replace(/\D/g, '').slice(0, 11))}
              required
            />
          </label>
          <label>
            Ставка / час
            <input
              type="number"
              step="0.01"
              min={0}
              value={wRate}
              onChange={(e) => setWRate(Number(e.target.value))}
              required
            />
          </label>
          <div className="row gap wrap">
            <button type="submit" className="btn">
              Добавить
            </button>
            <button type="button" className="btn ghost" onClick={() => nav(-1)}>
              Отмена
            </button>
          </div>
        </form>
      </section>
    </>
  )
}
