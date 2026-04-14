import { Navigate, Route, Routes } from 'react-router-dom'
import { Layout } from './components/Layout'
import './App.css'
import { CompanyCabinetLayout } from './layouts/CompanyCabinetLayout'
import { EmployeeCabinetLayout } from './layouts/EmployeeCabinetLayout'
import { CompanyAddWorkerPage } from './pages/company/CompanyAddWorkerPage'
import { CompanyProfilePage } from './pages/company/CompanyProfilePage'
import { CompanyWorkerHoursPage } from './pages/company/CompanyWorkerHoursPage'
import { CompanyWorkerProfilePage } from './pages/company/CompanyWorkerProfilePage'
import { CompanyWorkersPage } from './pages/company/CompanyWorkersPage'
import { EmployeeHoursPage } from './pages/employee/EmployeeHoursPage'
import { EmployeeProfilePage } from './pages/employee/EmployeeProfilePage'
import { HomePage } from './pages/HomePage'
import { LoginPage } from './pages/LoginPage'
import { RegisterPage } from './pages/RegisterPage'
import { SetupPage } from './pages/SetupPage'

export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/setup" element={<SetupPage />} />

        <Route path="/company" element={<CompanyCabinetLayout />}>
          <Route index element={<CompanyWorkersPage />} />
          <Route path="profile" element={<CompanyProfilePage />} />
          <Route path="workers/new" element={<CompanyAddWorkerPage />} />
          <Route path="workers/:workerId" element={<CompanyWorkerProfilePage />} />
          <Route path="workers/:workerId/hours" element={<CompanyWorkerHoursPage />} />
        </Route>

        <Route path="/employee" element={<EmployeeCabinetLayout />}>
          <Route index element={<EmployeeHoursPage />} />
          <Route path="profile" element={<EmployeeProfilePage />} />
        </Route>

        <Route path="*" element={<Navigate to="/" replace />} />
      </Route>
    </Routes>
  )
}
