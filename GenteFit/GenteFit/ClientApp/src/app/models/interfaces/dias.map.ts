// Creamos el enumerador para los días de la semana.
export const DiasMap = {
  1: 'Lunes',
  2: 'Martes',
  3: 'Miércoles',
  4: 'Jueves',
  5: 'Viernes',
  6: 'Sábado',
}

// Creamos la función para obtener el día de la semana a partir del enumerador
export function getDia(dia: number): string {
  return DiasMap[dia];
}