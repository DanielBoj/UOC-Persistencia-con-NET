// Esta interfaz modeliza los datos de la clase Horario.

// Importamos la interfaz de Clase, Reserva y Espera.
import { Clase } from './clase.model';
import { Reserva } from './reserva.model';
import { Espera } from './espera.model';

export interface Horario {
  id?: string,
  dia: number,
  hora: string,
  clase: Clase,
  reservas?: Array<Reserva>,
  esperas?: Array<Espera>
}
