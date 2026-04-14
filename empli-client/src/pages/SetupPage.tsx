import { useState } from 'react'
import { Navigate, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import * as api from '../api/empliApi'
import { UserType } from '../api/types'

export function SetupPage() {
  const { profile, refreshProfile } = useAuth()
  const nav = useNavigate()
  const location = useLocation()
  const intent = (location.state as { intent?: 'company' | 'employee' } | null)?.intent

  const [companyName, setCompanyName] = useState('')
  const [nip, setNip] = useState('')
  const [fn, setFn] = useState('')
  const [ln, setLn] = useState('')
  const [pesel, setPesel] = useState('')
  const [rate, setRate] = useState<number>(50)
  const [err, setErr] = useState<string | null>(null)

  if (!profile) return <Navigate to="/login" replace />
  if (profile.userType === UserType.Company) return <Navigate to="/company" replace />
  if (profile.userType === UserType.Employee) return <Navigate to="/employee" replace />

  const showCompany = intent === 'company' || intent == null
  const showEmployee = intent === 'employee' || intent == null

  async function submitCompany(e: React.FormEvent) {
    e.preventDefault()
    setErr(null)
    try {
      await api.createCompany(companyName.trim(), nip.trim())
      await refreshProfile()
      nav('/company', { replace: true })
    } catch {
      setErr('Не удалось создать компанию (возможно, профиль уже создан).')
    }
  }

  async function submitEmployee(e: React.FormEvent) {
    e.preventDefault()
    setErr(null)
    if (!/^\d{11}$/.test(pesel)) {
      setErr('PESEL должен содержать 11 цифр')
      return
    }
    try {
      await api.createEmployee({
        firstName: fn.trim(),
        lastName: ln.trim(),
        pesel,
        costPerHour: rate,
      })
      await refreshProfile()
      nav('/employee', { replace: true })
    } catch {
      setErr(
        'Не удалось создать профиль сотрудника. Если работник с таким PESEL уже есть у компании, вы будете привязаны к нему; если нет — создан личный профиль.',
      )
    }
  }

  return (
    <div>
      <h1>Завершите профиль</h1>
      {err && <p className="error">{err}</p>}

      <div className="setup-grid">
        {showCompany && (
          <form className="card" onSubmit={submitCompany}>
            <h2>Компания</h2>
            <label>
              Название
              <input value={companyName} onChange={(e) => setCompanyName(e.target.value)} required />
            </label>
            <label>
              NIP
              <input value={nip} onChange={(e) => setNip(e.target.value)} required />
            </label>
            <button type="submit" className="btn">
              Создать компанию
            </button>
          </form>
        )}

        {showEmployee && (
          <form className="card" onSubmit={submitEmployee}>
            <h2>Сотрудник</h2>
            <label>
              Имя
              <input value={fn} onChange={(e) => setFn(e.target.value)} required />
            </label>
            <label>
              Фамилия
              <input value={ln} onChange={(e) => setLn(e.target.value)} required />
            </label>
            <label>
              PESEL (11 цифр)
              <input value={pesel} onChange={(e) => setPesel(e.target.value.replace(/\D/g, '').slice(0, 11))} required />
            </label>
            <label>
              Ставка за час (PLN)
              <input
                type="number"
                step="0.01"
                min={0}
                value={rate}
                onChange={(e) => setRate(Number(e.target.value))}
              />
            </label>
            <button type="submit" className="btn">
              Создать профиль сотрудника
            </button>
          </form>
        )}
      </div>
    </div>
  )
}
