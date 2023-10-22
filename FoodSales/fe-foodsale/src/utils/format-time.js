import { format, getTime, formatDistanceToNow } from 'date-fns';

// ----------------------------------------------------------------------

export function fDate(date, newFormat) {
  const fm = newFormat || 'dd MMM yyyy';

  const dateStr = date?.at(-1) === 'Z' ? date : `${date}Z`

  return date ? format(new Date(dateStr), fm) : '';
}

export function fDateTime(date, newFormat) {
  const fm = newFormat || 'dd MMM yyyy p';

  const dateStr = date?.at(-1) === 'Z' ? date : `${date}Z`

  return date ? format(new Date(dateStr), fm) : '';
}

export function fTimestamp(date) {
  const dateStr = date?.at(-1) === 'Z' ? date : `${date}Z`

  return date ? getTime(new Date(dateStr)) : '';
}

export function fToNow(date) {
  const dateStr = date?.at(-1) === 'Z' ? date : `${date}Z`

  return date
    ? formatDistanceToNow(new Date(dateStr), {
        addSuffix: true,
      })
    : '';
}
