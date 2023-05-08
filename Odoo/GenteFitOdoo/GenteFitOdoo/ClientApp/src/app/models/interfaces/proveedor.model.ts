import { Direccion } from './direccion.model';

export interface Proveedor {
  id?: string,
  name: string,
  nif: string,
  direccion: Direccion,
  email: string,
  phone: string,
  website: string
}
