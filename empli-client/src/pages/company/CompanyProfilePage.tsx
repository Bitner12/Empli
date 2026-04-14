import { useCallback, useEffect, useState } from 'react'
import * as api from '../../api/empliApi'
import type { Company } from '../../api/types'

export function CompanyProfilePage() {
  const [company, setCompany] = useState<Company | null>(null)
  const [cName, setCName] = useState('')
  const [cNip, setCNip] = useState('')
  const [loading, setLoading] = useState(true)
  const [err, setErr] = useState<string | null>(null)
  const [ok, setOk] = useState<string | null>(null)

  const load = useCallback(async () => {
    setErr(null)
    try {
      const c = await api.getCompany()
      setCompany(c)
      setCName(c.name ?? '')
      setCNip(c.nip ?? '')
    } catch {
      setErr('Не удалось загрузить данные компании')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void load()
  }, [load])

  async function saveCompany(e: React.FormEvent) {
    e.preventDefault()
    if (!company) return
    setErr(null)
    setOk(null)
    try {
      await api.updateCompany(company.id, cName.trim(), cNip.trim())
      setOk('Сохранено')
      await load()
    } catch {
      setErr('Не удалось обновить компанию')
    }
  }

  if (loading) return <p className="muted">Загрузка…</p>

  return (
    <>
      <h1>Профиль компании</h1>
      {err && <p className="error">{err}</p>}
      {ok && <p className="muted small">{ok}</p>}

      {company && (
        <section className="card">
          <form onSubmit={saveCompany} className="form-stack">
            <label>
              Название
              <input value={cName} onChange={(e) => setCName(e.target.value)} required />
            </label>
            <label>
              NIP
              <input value={cNip} onChange={(e) => setCNip(e.target.value)} required />
            </label>
            <button type="submit" className="btn">
              Сохранить
            </button>
          </form>
        </section>
      )}
    </>
  )
}
