// Esta interfaz modeliza los datos de la clase Cliente.

// Importamos la interfaz de Direccion, User, Reserva y Espera.
import { Direccion } from './direccion.model';
import { User } from './user.model';

export interface Cliente extends User {
    id?: string,
    nombre: string,
    nif: string,
    direccion: Direccion,
    telefono: string,
    genero: string | number,
    iban: string,
}
