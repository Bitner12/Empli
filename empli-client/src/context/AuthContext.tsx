import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import { loadTokensFromStorage, setTokens } from '../api/http'
import * as api from '../api/empliApi'
import type { UserProfileDto } from '../api/types'
import { UserType } from '../api/types'

type AuthState = {
  profile: UserProfileDto | null
  loading: boolean
  login: (email: string, password: string) => Promise<UserProfileDto | null>
  register: (email: string, password: string) => Promise<UserProfileDto | null>
  logout: () => Promise<void>
  refreshProfile: () => Promise<UserProfileDto | null>
}

const AuthContext = createContext<AuthState | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [profile, setProfile] = useState<UserProfileDto | null>(null)
  const [loading, setLoading] = useState(true)

  const refreshProfile = useCallback(async () => {
    loadTokensFromStorage()
    const hasRefresh = !!localStorage.getItem('refreshToken')
    if (!hasRefresh) {
      setProfile(null)
      setLoading(false)
      return null
    }
    try {
      const p = await api.fetchProfile()
      setProfile(p)
      return p
    } catch {
      setProfile(null)
      setTokens(null, null)
      return null
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void refreshProfile()
  }, [refreshProfile])

  const login = useCallback(async (email: string, password: string) => {
    setLoading(true)
    try {
      await api.login(email, password)
      const p = await api.fetchProfile()
      setProfile(p)
      return p
    } catch {
      setProfile(null)
      return null
    } finally {
      setLoading(false)
    }
  }, [])

  const register = useCallback(async (email: string, password: string) => {
    setLoading(true)
    try {
      await api.register(email, password)
      const p = await api.fetchProfile()
      setProfile(p)
      return p
    } catch {
      setProfile(null)
      return null
    } finally {
      setLoading(false)
    }
  }, [])

  const logout = useCallback(async () => {
    setLoading(true)
    try {
      await api.logout()
    } catch {
      setTokens(null, null)
    }
    setProfile(null)
    setLoading(false)
  }, [])

  const value = useMemo(
    () => ({
      profile,
      loading,
      login,
      register,
      logout,
      refreshProfile,
    }),
    [profile, loading, login, register, logout, refreshProfile],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}

export function useIsCompany() {
  return useAuth().profile?.userType === UserType.Company
}

export function useIsEmployee() {
  return useAuth().profile?.userType === UserType.Employee
}
