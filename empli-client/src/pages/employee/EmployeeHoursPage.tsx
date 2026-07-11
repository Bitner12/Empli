import { useEffect, useState } from 'react'
import * as api from '../../api/empliApi'
import type { Contractor, EmployeeProfileResponse } from '../../api/types'
import { HoursEditor } from '../../components/HoursEditor'

export function EmployeeHoursPage() {
  const [emp, setEmp] = useState<EmployeeProfileResponse | null>(null)
  const [contractors, setContractors] = useState<Contractor[]>([])
  const [loading, setLoading] = useState(true)
  const [err, setErr] = useState<string | null>(null)

  useEffect(() => {
    void (async () => {
      try {
        const data = await api.getEmployee()
        setEmp(data)
        if (data.workerId) {
          try {
            const list = await api.getContractorsByWorker(data.workerId)
            setContractors(list)
          } catch {
            // контрагенты не обязательны
          }
        }
      } catch {
        setErr('Не удалось загрузить профиль')
      } finally {
        setLoading(false)
      }
    })()
  }, [])

  if (loading) return <p className="muted">Загрузка…</p>
  if (err && !emp) return <p className="error">{err}</p>
  if (!emp?.workerId) return <p className="error">Нет привязанного работника для учёта часов.</p>

  return (
    <>
      <h1>Мои часы</h1>
      <HoursEditor
        workerId={emp.workerId}
        title="Учёт рабочего времени"
        hourlyRate={emp.costPerHour ?? 0}
        contractors={contractors}
      />
    </>
  )
}