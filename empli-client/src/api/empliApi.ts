import api, { rawClient, setTokens } from './http'
import type {
  Company,
  Contractor,
  EmployeeProfileResponse,
  Hour,
  LoginResponse,
  UserProfileDto,
  Worker,
} from './types'

export async function login(email: string, password: string) {
  const { data } = await rawClient.post<LoginResponse>('/User/login', {
    email,
    password,
  })
  if (data.accessToken && data.refreshToken) {
    setTokens(data.accessToken, data.refreshToken)
  }
  return data
}

export async function register(email: string, password: string) {
  const { data } = await rawClient.post<LoginResponse>('/User/register', {
    email,
    password,
  })
  if (data.accessToken && data.refreshToken) {
    setTokens(data.accessToken, data.refreshToken)
  }
  return data
}

export async function logout() {
  try {
    await api.post('/User/logout')
  } finally {
    setTokens(null, null)
  }
}

export async function fetchProfile() {
  const { data } = await api.get<UserProfileDto>('/User/me')
  return data
}

export async function createCompany(name: string, nip: string) {
  const { data } = await api.post<Company>('/Company/create', { name, nip })
  return data
}

export async function getCompany() {
  const { data } = await api.get<Company>('/Company/get')
  return data
}

export async function updateCompany(id: string, name: string, nip: string) {
  await api.put(`/Company/update?id=${encodeURIComponent(id)}`, { name, nip })
}

export async function createEmployee(body: {
  firstName: string
  lastName: string
  pesel: string
  costPerHour?: number | null
}) {
  await api.post('/Employee/create', body)
}

export async function getEmployee() {
  const { data } = await api.get<EmployeeProfileResponse>('/Employee/get')
  return data
}

export async function updateEmployee(body: {
  firstName: string
  lastName: string
  pesel?: string
  costPerHour?: number | null
}) {
  const { data } = await api.put<EmployeeProfileResponse>('/Employee/update', body)
  return data
}

export async function getCompanyWorkers() {
  const { data } = await api.get<Worker[]>('/Manager/getWorkers')
  return data
}

export async function addCompanyWorker(body: {
  pesel: string
  firstName: string
  lastName: string
  costPerHour: number
}) {
  await api.post('/Manager/addWorker', body)
}

export async function updateCompanyWorker(
  id: string,
  firstName: string,
  lastName: string,
  costPerHour: number,
) {
  const q = new URLSearchParams({
    id,
    firstName,
    lastName,
    costPerHour: String(costPerHour),
  })
  await api.put(`/Manager/updateWorker?${q.toString()}`)
}

export async function deleteCompanyWorker(id: string) {
  await api.delete(`/Manager/deleteWorker?id=${encodeURIComponent(id)}`)
}

export async function getWorkerById(id: string) {
  const { data } = await api.get<Worker>(`/Worker/by-Id?id=${encodeURIComponent(id)}`)
  return data
}

export async function getHoursAll(workerId: string) {
  const { data } = await api.get<Hour[]>(`/HourContoller/all?workerId=${encodeURIComponent(workerId)}`)
  return data
}

export async function getHoursPeriod(
  workerId: string,
  dateStart: string,
  dateEnd: string,
  contractorId?: string,
) {
  const q = new URLSearchParams({ id: workerId, dateStart, dateEnd })
  if (contractorId) q.set('contractorId', contractorId)
  const { data } = await api.get<Hour[]>(`/HourContoller/period?${q.toString()}`)
  return data
}

export async function createHour(
  workerId: string,
  date: string,
  hours: number,
  contractorId?: string | null,
  comment?: string | null,
) {
  await api.post('/HourContoller', { workerId, date, hours, contractorId, comment })
}

export async function updateHour(
  workerId: string,
  date: string,
  hours: number,
  contractorId?: string | null,
  comment?: string | null,
) {
  const q = new URLSearchParams({ id: workerId, hour: String(hours), date })
  if (contractorId) q.set('contractorId', contractorId)
  if (comment !== undefined && comment !== null) q.set('comment', comment)
  await api.patch(`/HourContoller?${q.toString()}`)
}

export async function getContractorsByCompany() {
  const { data } = await api.get<Contractor[]>('/Contractor/list')
  return data
}

export async function getContractorsByWorker(workerId: string) {
  const { data } = await api.get<Contractor[]>(`/Contractor/byWorker?workerId=${encodeURIComponent(workerId)}`)
  return data
}

export async function createContractor(name: string, ratePerHour: number) {
  const { data } = await api.post<Contractor>('/Contractor', { name, ratePerHour })
  return data
}

export async function updateContractor(id: string, name: string, ratePerHour: number) {
  const { data } = await api.put<Contractor>(`/Contractor?id=${encodeURIComponent(id)}`, { name, ratePerHour })
  return data
}

export async function deleteContractor(id: string) {
  await api.delete(`/Contractor?id=${encodeURIComponent(id)}`)
}

export async function getHoursByContractor(contractorId: string, dateStart?: string, dateEnd?: string) {
  const q = new URLSearchParams({ contractorId })
  if (dateStart) q.set('dateStart', dateStart)
  if (dateEnd) q.set('dateEnd', dateEnd)
  const { data } = await api.get<Hour[]>(`/HourContoller/byContractor?${q.toString()}`)
  return data
}

export async function deleteHour(workerId: string, date: string) {
  const q = new URLSearchParams({ id: workerId, date })
  await api.delete(`/HourContoller?${q.toString()}`)
}
