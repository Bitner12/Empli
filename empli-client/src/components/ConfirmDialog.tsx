import { useCallback, useEffect, useState } from 'react'
import { createPortal } from 'react-dom'

type ConfirmOptions = {
  message: string
  title?: string
  confirmText?: string
  cancelText?: string
  danger?: boolean
}

type DialogProps = ConfirmOptions & {
  onConfirm: () => void
  onCancel: () => void
}

function ConfirmDialog({
  message,
  title = 'Подтверждение',
  confirmText = 'Удалить',
  cancelText = 'Отмена',
  danger = true,
  onConfirm,
  onCancel,
}: DialogProps) {
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onCancel()
      if (e.key === 'Enter') onConfirm()
    }
    document.addEventListener('keydown', onKey)
    return () => document.removeEventListener('keydown', onKey)
  }, [onCancel, onConfirm])

  return createPortal(
    <div className="modal-overlay" onClick={onCancel}>
      <div
        className="modal-card"
        role="dialog"
        aria-modal="true"
        onClick={(e) => e.stopPropagation()}
      >
        <h3 className="modal-title">{title}</h3>
        <p className="modal-message">{message}</p>
        <div className="modal-actions">
          <button type="button" className="btn ghost" onClick={onCancel}>
            {cancelText}
          </button>
          <button
            type="button"
            className={danger ? 'btn danger' : 'btn'}
            onClick={onConfirm}
            autoFocus
          >
            {confirmText}
          </button>
        </div>
      </div>
    </div>,
    document.body,
  )
}

/**
 * Replacement for window.confirm — renders an in-app modal instead of the
 * native browser bar at the top of the window.
 *
 * const { confirm, confirmDialog } = useConfirm()
 * if (!(await confirm('Удалить?'))) return
 * ...render {confirmDialog} somewhere in the component tree
 */
export function useConfirm() {
  const [state, setState] = useState<
    (ConfirmOptions & { resolve: (value: boolean) => void }) | null
  >(null)

  const confirm = useCallback((options: ConfirmOptions | string) => {
    const opts = typeof options === 'string' ? { message: options } : options
    return new Promise<boolean>((resolve) => {
      setState({ ...opts, resolve })
    })
  }, [])

  const close = useCallback((result: boolean) => {
    setState((s) => {
      s?.resolve(result)
      return null
    })
  }, [])

  const confirmDialog = state ? (
    <ConfirmDialog
      {...state}
      onConfirm={() => close(true)}
      onCancel={() => close(false)}
    />
  ) : null

  return { confirm, confirmDialog }
}