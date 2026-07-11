import { useEffect, useState } from 'react'


import { Link, useParams } from 'react-router-dom'
import * as api from '../../api/empliApi'
import type { Worker } from '../../api/types'
import { useConfirm } from '../../components/ConfirmDialog'

export function CompanyWorkerProfilePage() {
  const { workerId } = useParams<{ workerId: string }>()
  const { confirm, confirmDialog } = useConfirm()
  const [worker, setWorker] = useState<Worker | null>(null)
  const [err, setErr] = useState<string | null>(null)
  const [editing, setEditing] = useState(false)
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [pesel, setPesel] = useState('')
  const [rate, setRate] = useState(0)

  async function load() {
    if (!workerId) return
    try {
      const w = await api.getWorkerById(workerId)
      setWorker(w)
      setFirstName(w.firstName ?? '')
      setLastName(w.lastName ?? '')
      setPesel(w.pesel ?? '')
      setRate(w.costPerHour ?? 0)
      setErr(null)
    } catch {
      setErr('Не удалось загрузить сотрудника')
    }
  }

  useEffect(() => {
    void load()
  }, [workerId])

  function cancelEdit() {
    if (worker) {
      setFirstName(worker.firstName ?? '')
      setLastName(worker.lastName ?? '')
      setRate(worker.costPerHour ?? 0)
    }
    setEditing(false)
  }

  if (!workerId) return <p className="error">Некорректная ссылка</p>
  if (err && !worker) return <p className="error">{err}</p>
  if (!worker) return <p className="muted">Загрузка…</p>

  async function saveWorker(e: React.FormEvent) {
    e.preventDefault()
    if (!workerId) return
    setErr(null)
    try {
      await api.updateCompanyWorker(workerId, firstName.trim(), lastName.trim(), rate)
      await load()
      setEditing(false)
    } catch {
      setErr('Не удалось сохранить сотрудника')
    }
  }

  async function removeWorker() {
    if (!workerId) return
    if (!(await confirm('Удалить сотрудника из компании?'))) return
    try {
      await api.deleteCompanyWorker(workerId)
      window.location.href = '/company'
    } catch {
      setErr('Не удалось удалить сотрудника')
    }
  }

  const fullName = `${worker.firstName} ${worker.lastName}`.trim()

  return (
    <>
      {confirmDialog}
      <p className="cabinet-back">
        <Link to="/company">← К списку сотрудников</Link>
      </p>
      <h1>{fullName || 'Сотрудник'}</h1>
      {err && <p className="error">{err}</p>}

      {editing ? (
        <section className="card narrow-card">
          <h2 className="card-heading">Редактирование</h2>
          <form onSubmit={(e) => void saveWorker(e)} className="form-stack">
            <label>
              Имя
              <input
                type="text"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                required
              />
            </label>
            <label>
              Фамилия
              <input
                type="text"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                required
              />
            </label>
            <label>
              PESEL
              <input type="text" value={pesel} disabled />
            </label>
            <label>
              Ставка / час (PLN)
              <input
                type="number"
                step="0.01"
                min={0}
                value={rate}
                onChange={(e) => setRate(Number(e.target.value))}
                required
              />
            </label>
            <div className="actions">
              <button type="submit" className="btn">
                Сохранить
              </button>
              <button type="button" className="btn secondary" onClick={cancelEdit}>
                Отмена
              </button>
            </div>
          </form>
        </section>
      ) : (
        <section className="card narrow-card">
          <dl className="profile-dl">
            <dt>Имя</dt>
            <dd>{worker.firstName}</dd>
            <dt>Фамилия</dt>
            <dd>{worker.lastName}</dd>
            <dt>PESEL</dt>
            <dd>{worker.pesel ?? '—'}</dd>
            <dt>Ставка / час</dt>
            <dd>{(worker.costPerHour ?? 0).toFixed(2)} PLN</dd>
          </dl>
          <div className="actions worker-profile-actions">
            <Link to={`/company/workers/${workerId}/hours`} className="btn secondary link-btn">
              Часы
            </Link>
            <button type="button" className="btn secondary" onClick={() => setEditing(true)}>
              Редактировать
            </button>
            <button type="button" className="btn ghost danger" onClick={() => void removeWorker()}>
              Удалить
            </button>
          </div>
        </section>
      )}
    </>
  )
}
