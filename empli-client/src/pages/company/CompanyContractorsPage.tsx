import { useCallback, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import * as api from '../../api/empliApi'
import type { Contractor } from '../../api/types'
import { useConfirm } from '../../components/ConfirmDialog'

export function CompanyContractorsPage() {
  const { confirm, confirmDialog } = useConfirm()
  const [contractors, setContractors] = useState<Contractor[]>([])
  const [loading, setLoading] = useState(true)
  const [err, setErr] = useState<string | null>(null)

  const [newName, setNewName] = useState('')
  const [newRate, setNewRate] = useState('')
  const [addErr, setAddErr] = useState<string | null>(null)

  const load = useCallback(async () => {
    setErr(null)
    try {
      const list = await api.getContractorsByCompany()
      setContractors(list)
    } catch {
      setErr('Не удалось загрузить контрагентов')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => { void load() }, [load])

  async function addContractor(e: React.FormEvent) {
    e.preventDefault()
    const name = newName.trim()
    if (!name) return
    setAddErr(null)
    try {
      await api.createContractor(name, Number(newRate) || 0)
      setNewName('')
      setNewRate('')
      await load()
    } catch {
      setAddErr('Не удалось добавить контрагента')
    }
  }

  async function remove(id: string) {
    if (!(await confirm('Удалить контрагента?'))) return
    try {
      await api.deleteContractor(id)
      await load()
    } catch {
      setErr('Не удалось удалить контрагента')
    }
  }

  if (loading) return <p className="muted">Загрузка…</p>

  return (
    <>
      {confirmDialog}
      <h1>Контрагенты</h1>
      {err && <p className="error">{err}</p>}

      <section className="card">
        <h2 className="card-heading">Добавить контрагента</h2>
        {addErr && <p className="error">{addErr}</p>}
        <form className="row gap wrap" onSubmit={addContractor}>
          <label style={{ flex: '2 1 180px' }}>
            Название
            <input
              value={newName}
              onChange={(e) => setNewName(e.target.value)}
              placeholder="ООО Название"
              required
            />
          </label>
          <label style={{ flex: '1 1 120px' }}>
            Ставка (PLN/ч)
            <input
              type="number"
              step="0.01"
              min={0}
              value={newRate}
              onChange={(e) => setNewRate(e.target.value)}
              placeholder="0"
              required
            />
          </label>
          <button type="submit" className="btn" style={{ alignSelf: 'flex-end' }}>
            Добавить
          </button>
        </form>
      </section>

      <section className="card">
        <h2 className="card-heading">Список контрагентов</h2>
        {contractors.length === 0 ? (
          <p className="muted small">Контрагенты не добавлены</p>
        ) : (
          <div className="table-wrap">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Название</th>
                  <th>Ставка (PLN/ч)</th>
                  <th />
                  <th />
                </tr>
              </thead>
              <tbody>
                {contractors.map((c) => (
                  <tr key={c.id}>
                    <td>
                      <Link to={`/company/contractors/${c.id}`}>{c.name}</Link>
                    </td>
                    <td>{c.ratePerHour.toFixed(2)}</td>
                    <td>
                      <Link to={`/company/contractors/${c.id}`} className="btn ghost">
                        Открыть
                      </Link>
                    </td>
                    <td>
                      <button
                        type="button"
                        className="btn ghost danger"
                        onClick={() => void remove(c.id)}
                      >
                        Удалить
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </section>
    </>
  )
}