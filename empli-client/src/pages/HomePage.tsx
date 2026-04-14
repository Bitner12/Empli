import { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { UserType } from '../api/types'

export function HomePage() {
  const { profile, loading } = useAuth()
  const nav = useNavigate()

  useEffect(() => {
    if (loading) return
    if (!profile) {
      nav('/login', { replace: true })
      return
    }
    if (profile.userType === UserType.Empty) nav('/setup', { replace: true })
    else if (profile.userType === UserType.Company) nav('/company', { replace: true })
    else nav('/employee', { replace: true })
  }, [loading, profile, nav])

  return <p className="muted center">Загрузка…</p>
}
