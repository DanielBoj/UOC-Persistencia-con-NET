// Esta interface modeliza los datos de la clase Centro.

// Importamos la interface Direccion.
import { Direccion } from './direccion.model';

export interface Centro {
  id?: string,
  nombre: string,
  direccion: Direccion,
  descripcion: string,
  telefono: string,
  email: string
}
