// Esta interfaz modeliza los datos de la clase Cliente.

// Importamos la interfaz de Direccion, User, Reserva y Espera.
import { Direccion } from './direccion.model';
import { User } from './user.model';
import { Reserva } from './reserva.model';
import { Espera } from './espera.model';

export interface Cliente extends User {
  nombre: string,
  nif: string,
  direccion: Direccion,
  telefono: string,
  genero: number,
  iban: string,
  reservas?: Array<Reserva>,
  esperas?: Array<Espera>
}
