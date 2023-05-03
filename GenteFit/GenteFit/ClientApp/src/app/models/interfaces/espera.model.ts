// Esta interfaz modeliza los datos de la clase Espera.

// Importamos las interfaces de Cliente y Horario.
import { Cliente } from './cliente.model';
import { Horario } from './horario.model';

export interface Espera {
  id?: string,
  cliente: Cliente,
  horario: Horario
}
