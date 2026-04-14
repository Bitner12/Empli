import { useEffect, useState } from 'react'
import { NavLink, Navigate, Outlet } from 'react-router-dom'
import * as api from '../api/empliApi'
import { UserType } from '../api/types'
import { useAuth } from '../context/AuthContext'
import './CabinetLayout.css'

export function CompanyCabinetLayout() {
  const { profile } = useAuth()
  const [companyName, setCompanyName] = useState('Компания')

  useEffect(() => {
    void (async () => {
      try {
        const c = await api.getCompany()
        setCompanyName(c.name?.trim() || 'Компания')
      } catch {
        setCompanyName('Компания')
      }
    })()
  }, [])

  if (!profile) return <Navigate to="/login" replace />
  if (profile.userType === UserType.Empty) return <Navigate to="/setup" replace />
  if (profile.userType === UserType.Employee) return <Navigate to="/employee" replace />

  return (
    <div className="cabinet-shell">
      <aside className="cabinet-sidebar">
        <div className="cabinet-sidebar-title">{companyName}</div>
        <nav className="cabinet-nav">
          <NavLink to="/company" end className={({ isActive }) => (isActive ? 'active' : '')}>
            Сотрудники
          </NavLink>
          <NavLink to="/company/profile" className={({ isActive }) => (isActive ? 'active' : '')}>
            Профиль компании
          </NavLink>
          <NavLink to="/company/workers/new" className={({ isActive }) => (isActive ? 'active' : '')}>
            Новый сотрудник
          </NavLink>
        </nav>
      </aside>
      <div className="cabinet-content">
        <Outlet />
      </div>
    </div>
  )
}
