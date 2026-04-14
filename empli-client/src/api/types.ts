export const UserType = {
  Empty: 0,
  Company: 1,
  Employee: 2,
} as const

export type UserTypeValue = (typeof UserType)[keyof typeof UserType]

export interface LoginResponse {
  id: string | null
  email: string | null
  refreshToken: string | null
  accessToken: string | null
}

export interface UserProfileDto {
  userType: number
  email: string | null
  companyId: string | null
  employeeId: string | null
  workerId: string | null
}

export interface EmployeeProfileResponse {
  id: string
  firstName: string | null
  lastName: string | null
  pesel: string | null
  costPerHour: number | null
  workerId: string | null
  isLinkedToCompany: boolean
}

export interface Company {
  id: string
  name: string | null
  nip: string | null
  userId: string | null
}

export interface Hour {
  id: string
  date: string
  hours: number
  workerId?: string
}

export interface Worker {
  id: string
  pesel?: string | null
  firstName: string
  lastName: string
  costPerHour?: number | null
  companyId?: string | null
  hours?: Hour[] | null
}
