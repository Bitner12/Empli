import axios from 'axios'
import type { LoginResponse } from './types'

const baseURL = import.meta.env.VITE_API_URL ?? 'http://localhost:5275'

export const rawClient = axios.create({
  baseURL,
  headers: { 'Content-Type': 'application/json' },
})

const api = axios.create({
  baseURL,
  headers: { 'Content-Type': 'application/json' },
})

let accessToken: string | null = null
let refreshToken: string | null = null

export function setTokens(access: string | null, refresh: string | null) {
  accessToken = access
  refreshToken = refresh
  if (access) localStorage.setItem('accessToken', access)
  else localStorage.removeItem('accessToken')
  if (refresh) localStorage.setItem('refreshToken', refresh)
  else localStorage.removeItem('refreshToken')
}

export function loadTokensFromStorage() {
  accessToken = localStorage.getItem('accessToken')
  refreshToken = localStorage.getItem('refreshToken')
}

api.interceptors.request.use((config) => {
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`
  }
  return config
})

api.interceptors.response.use(
  (r) => r,
  async (error) => {
    const original = error.config as typeof error.config & { _retry?: boolean }
    if (error.response?.status === 401 && refreshToken && original && !original._retry) {
      original._retry = true
      try {
        const { data } = await rawClient.post<LoginResponse>('/User/refresh', {
          refreshToken,
        })
        if (data.accessToken && data.refreshToken) {
          setTokens(data.accessToken, data.refreshToken)
          original.headers.Authorization = `Bearer ${data.accessToken}`
          return api(original)
        }
      } catch {
        setTokens(null, null)
      }
    }
    return Promise.reject(error)
  },
)

export default api
