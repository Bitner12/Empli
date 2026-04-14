/** Сегодняшняя дата в локальном календаре YYYY-MM-DD (для input[type=date]). */
export function localDateInputString(d = new Date()): string {
  const y = d.getFullYear()
  const m = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${y}-${m}-${day}`
}

export function parseDateInput(s: string): number {
  const [y, m, d] = s.split('-').map(Number)
  return new Date(y, m - 1, d).getTime()
}

export function isDateOnOrBeforeToday(dateInput: string): boolean {
  return parseDateInput(dateInput) <= parseDateInput(localDateInputString())
}
