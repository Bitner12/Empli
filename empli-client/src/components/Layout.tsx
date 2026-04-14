import { Link, Outlet, useLocation } from 'react-router-dom'
import { UserType } from '../api/types'
import { useAuth } from '../context/AuthContext'
import './Layout.css'

export function Layout() {
  const { profile, logout } = useAuth()
  const { pathname } = useLocation()
  const isCabinet = pathname.startsWith('/company') || pathname.startsWith('/employee')

  return (
    <div className="layout">
      <header className="layout-header">
        <Link to="/" className="logo">
          Empli
        </Link>
        <nav>
          {profile && (
            <>
              {profile.userType === UserType.Company && (
                <Link to="/company" className="nav-link">
                  Кабинет компании
                </Link>
              )}
              {profile.userType === UserType.Employee && (
                <Link to="/employee" className="nav-link">
                  Кабинет сотрудника
                </Link>
              )}
              {profile.userType === UserType.Empty && (
                <Link to="/setup" className="nav-link">
                  Настройка профиля
                </Link>
              )}
              <span className="muted">{profile.email}</span>
              <button type="button" className="btn ghost" onClick={() => void logout()}>
                Выйти
              </button>
            </>
          )}
        </nav>
      </header>
      <main className={`layout-main${isCabinet ? ' layout-main--cabinet' : ''}`}>
        <Outlet />
      </main>
    </div>
  )
}
