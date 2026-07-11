import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import * as api from '../../api/empliApi'
import type { Contractor, Worker } from '../../api/types'
import { HoursEditor } from '../../components/HoursEditor'

export function CompanyWorkerHoursPage() {
  const { workerId } = useParams<{ workerId: string }>()
  const [worker, setWorker] = useState<Worker | null>(null)
  const [contractors, setContractors] = useState<Contractor[]>([])
  const [err, setErr] = useState<string | null>(null)

  useEffect(() => {
    if (!workerId) return
    void (async () => {
      try {
        const [w, list] = await Promise.all([
          api.getWorkerById(workerId),
          api.getContractorsByWorker(workerId),
        ])
        setWorker(w)
        setContractors(list)
        setErr(null)
      } catch {
        setErr('Не удалось загрузить данные')
      }
    })()
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
        contractors={contractors}
        showContractorFilter
      />
    </>
  )
}