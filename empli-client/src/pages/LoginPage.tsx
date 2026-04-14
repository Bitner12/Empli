import { useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { UserType } from '../api/types'

export function LoginPage() {
  const { login, profile, loading } = useAuth()
  const nav = useNavigate()

  useEffect(() => {
    if (loading) return
    if (!profile) return
    if (profile.userType === UserType.Empty) nav('/setup', { replace: true })
    else if (profile.userType === UserType.Company) nav('/company', { replace: true })
    else if (profile.userType === UserType.Employee) nav('/employee', { replace: true })
  }, [loading, profile, nav])
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [err, setErr] = useState<string | null>(null)

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault()
    setErr(null)
    try {
      const p = await login(email, password)
      if (!p) {
        setErr('Неверный email или пароль')
        return
      }
      if (p.userType === UserType.Empty) nav('/setup')
      else if (p.userType === UserType.Company) nav('/company')
      else if (p.userType === UserType.Employee) nav('/employee')
      else nav('/')
    } catch {
      setErr('Неверный email или пароль')
    }
  }

  if (loading) return <p className="muted center">Загрузка…</p>
  if (profile) return <p className="muted center">Перенаправление…</p>

  return (
    <div className="auth-card card">
      <h1>Вход</h1>
      <form onSubmit={onSubmit}>
        <label>
          Email
          <input
            type="email"
            autoComplete="username"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </label>
        <label>
          Пароль
          <input
            type="password"
            autoComplete="current-password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </label>
        {err && <p className="error">{err}</p>}
        <button type="submit" className="btn wide" disabled={loading}>
          Войти
        </button>
      </form>
      <p className="muted small">
        Нет аккаунта? <Link to="/register">Регистрация</Link>
      </p>
    </div>
  )
}
