export const formatDate = (date: Date, format: string) => {
  return new Intl.DateTimeFormat(format).format(date)
}
