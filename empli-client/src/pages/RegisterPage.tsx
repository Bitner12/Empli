import { useEffect, useRef, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import * as api from '../api/empliApi'
import { useAuth } from '../context/AuthContext'
import { UserType } from '../api/types'

export function RegisterPage() {
  const { register, profile, loading, refreshProfile } = useAuth()
  const nav = useNavigate()
  const completingProfileRef = useRef(false)

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [password2, setPassword2] = useState('')
  const [intent, setIntent] = useState<'company' | 'employee'>('employee')
  const [companyName, setCompanyName] = useState('')
  const [nip, setNip] = useState('')
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [pesel, setPesel] = useState('')
  const [costPerHour, setCostPerHour] = useState(50)
  const [err, setErr] = useState<string | null>(null)

  useEffect(() => {
    if (loading) return
    if (!profile) return
    if (profile.userType === UserType.Company) nav('/company', { replace: true })
    else if (profile.userType === UserType.Employee) nav('/employee', { replace: true })
    else if (profile.userType === UserType.Empty && !completingProfileRef.current) {
      nav('/setup', { replace: true })
    }
  }, [loading, profile, nav])

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault()
    setErr(null)
    if (password !== password2) {
      setErr('Пароли не совпадают')
      return
    }
    if (intent === 'company') {
      if (!companyName.trim() || !nip.trim()) {
        setErr('Укажите название компании и NIP')
        return
      }
    } else {
      if (!firstName.trim() || !lastName.trim()) {
        setErr('Укажите имя и фамилию')
        return
      }
      if (!/^\d{11}$/.test(pesel)) {
        setErr('PESEL должен содержать 11 цифр')
        return
      }
    }

    completingProfileRef.current = true
    try {
      const p = await register(email, password)
      if (!p) {
        setErr('Не удалось зарегистрироваться')
        return
      }
      if (p.userType === UserType.Empty) {
        try {
          if (intent === 'company') {
            await api.createCompany(companyName.trim(), nip.trim())
          } else {
            await api.createEmployee({
              firstName: firstName.trim(),
              lastName: lastName.trim(),
              pesel,
              costPerHour,
            })
          }
        } catch {
          setErr(
            'Аккаунт создан, но не удалось создать профиль. Заполните данные на странице настройки или войдите снова.',
          )
          nav('/setup', { replace: true, state: { intent } })
          return
        }
        const updated = await refreshProfile()
        if (updated?.userType === UserType.Company) nav('/company', { replace: true })
        else if (updated?.userType === UserType.Employee) nav('/employee', { replace: true })
        else nav('/setup', { replace: true, state: { intent } })
        return
      }
      if (p.userType === UserType.Company) nav('/company', { replace: true })
      else if (p.userType === UserType.Employee) nav('/employee', { replace: true })
    } catch {
      setErr('Ошибка регистрации (возможно, email уже занят)')
    } finally {
      completingProfileRef.current = false
    }
  }

  if (loading) return <p className="muted center">Загрузка…</p>
  if (profile) return <p className="muted center">Перенаправление…</p>

  return (
    <div className="auth-card card">
      <h1>Регистрация</h1>
      <form onSubmit={onSubmit}>
        <fieldset className="segmented">
          <legend>Тип аккаунта</legend>
          <label className="radio-inline">
            <input
              type="radio"
              name="intent"
              checked={intent === 'employee'}
              onChange={() => setIntent('employee')}
            />
            Сотрудник
          </label>
          <label className="radio-inline">
            <input
              type="radio"
              name="intent"
              checked={intent === 'company'}
              onChange={() => setIntent('company')}
            />
            Компания
          </label>
        </fieldset>

        {intent === 'company' ? (
          <>
            <label>
              Название компании
              <input
                value={companyName}
                onChange={(e) => setCompanyName(e.target.value)}
                required
                autoComplete="organization"
              />
            </label>
            <label>
              NIP
              <input value={nip} onChange={(e) => setNip(e.target.value)} required autoComplete="off" />
            </label>
          </>
        ) : (
          <>
            <label>
              Имя
              <input
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                required
                autoComplete="given-name"
              />
            </label>
            <label>
              Фамилия
              <input
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                required
                autoComplete="family-name"
              />
            </label>
            <label>
              PESEL (11 цифр)
              <input
                value={pesel}
                onChange={(e) => setPesel(e.target.value.replace(/\D/g, '').slice(0, 11))}
                required
                inputMode="numeric"
                autoComplete="off"
              />
            </label>
            <label>
              Ставка за час (PLN)
              <input
                type="number"
                step="0.01"
                min={0}
                value={costPerHour}
                onChange={(e) => setCostPerHour(Number(e.target.value))}
              />
            </label>
          </>
        )}

        <label>
          Email
          <input
            type="email"
            autoComplete="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </label>
        <label>
          Пароль
          <input
            type="password"
            autoComplete="new-password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            minLength={6}
          />
        </label>
        <label>
          Повтор пароля
          <input
            type="password"
            autoComplete="new-password"
            value={password2}
            onChange={(e) => setPassword2(e.target.value)}
            required
          />
        </label>
        {err && <p className="error">{err}</p>}
        <button type="submit" className="btn wide" disabled={loading}>
          Создать аккаунт
        </button>
      </form>
      <p className="muted small">
        Уже есть аккаунт? <Link to="/login">Войти</Link>
      </p>
    </div>
  )
}
