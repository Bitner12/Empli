import { NavLink, Navigate, Outlet } from 'react-router-dom'
import { UserType } from '../api/types'
import { useAuth } from '../context/AuthContext'
import './CabinetLayout.css'

export function EmployeeCabinetLayout() {
  const { profile } = useAuth()

  if (!profile) return <Navigate to="/login" replace />
  if (profile.userType === UserType.Empty) return <Navigate to="/setup" replace />
  if (profile.userType === UserType.Company) return <Navigate to="/company" replace />

  return (
    <div className="cabinet-shell">
      <aside className="cabinet-sidebar">
        <div className="cabinet-sidebar-title">Сотрудник</div>
        <nav className="cabinet-nav">
          <NavLink to="/employee" end className={({ isActive }) => (isActive ? 'active' : '')}>
            Мои часы
          </NavLink>
          <NavLink to="/employee/profile" className={({ isActive }) => (isActive ? 'active' : '')}>
            Мой профиль
          </NavLink>
        </nav>
      </aside>
      <div className="cabinet-content">
        <Outlet />
      </div>
    </div>
  )
}
