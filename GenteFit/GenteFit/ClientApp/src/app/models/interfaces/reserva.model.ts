// Esta interfaz modeliza los datos de la clase Reserva.

// Importamos la interfaz de Horario y de Cliente.
import { Horario } from './horario.model';
import { Cliente } from './cliente.model';

export interface Reserva {
  id?: string,
  horario: Horario,
  cliente: Cliente
}
