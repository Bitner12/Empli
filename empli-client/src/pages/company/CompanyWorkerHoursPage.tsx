import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import * as api from '../../api/empliApi'
import type { Worker } from '../../api/types'
import { HoursEditor } from '../../components/HoursEditor'

export function CompanyWorkerHoursPage() {
  const { workerId } = useParams<{ workerId: string }>()
  const [worker, setWorker] = useState<Worker | null>(null)
  const [err, setErr] = useState<string | null>(null)

  async function load() {
    if (!workerId) return
    try {
      const w = await api.getWorkerById(workerId)
      setWorker(w)
      setErr(null)
    } catch {
      setErr('Не удалось загрузить сотрудника')
    }
  }

  useEffect(() => {
    void load()
  }, [workerId])

  if (!workerId) return <p className="error">Некорректная ссылка</p>
  if (err && !worker) return <p className="error">{err}</p>

  const title = worker ? `Часы: ${worker.firstName} ${worker.lastName}` : 'Часы сотрудника'

  return (
    <>
      <p className="cabinet-back">
        <Link to={`/company/workers/${workerId}`}>← К профилю сотрудника</Link>
      </p>
      {err && <p className="error">{err}</p>}
      <HoursEditor
        workerId={workerId}
        title={title}
        hourlyRate={worker?.costPerHour ?? 0}
      />
    </>
  )
}
