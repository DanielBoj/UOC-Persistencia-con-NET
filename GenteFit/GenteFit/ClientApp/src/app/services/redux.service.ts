/* Este servicio maneja los estados de la aplicación, intercambia valores entre componentes y mantiene la integridad de los datos. */
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReduxService {

  constructor() { }

  // El componente de login nos envía el tipo de usuario
  setTipoUsuario = (tipo: string): void => localStorage.setItem('tipoUsuario', tipo);

  // El componente de login nos envía el id de usuario
  setIdUsuario = (id: string): void => localStorage.setItem('idUsuario', id);

  // Enviamos el tipo de usuuario a cualquier otro componente
  getTipoUsuario = (): string | null => localStorage.getItem('tipoUsuario');

  // Enviamos el id de usuario a cualquier otro componente
  getIdUsuario = (): string | null => localStorage.getItem('idUsuario');
}
