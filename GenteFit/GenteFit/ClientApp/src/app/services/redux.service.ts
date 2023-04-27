/* Este servicio maneja los estados de la aplicación, intercambia valores entre componentes y mantiene la integridad de los datos. */
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReduxService {

  localCache!: Cache;;

  constructor() { }

  // El componente de login nos envía el tipo de usuario
  setTipoUsuario = (tipo: string) => this.localCache.tipoUsuario = tipo; //localStorage.setItem('tipoUsuario', tipo);

  // Enviamos el tipo de usuuario a cualquier otro componente
  getTipoUsuario = (): string => { return 'cliente' }//this.localCache.tipoUsuario }//localStorage.getItem('tipoUsuario') || '';

  // El componente de login nos envía el id de usuario
  setIdUsuario = (id: string) => this.localCache.idUsuario = id //localStorage.setItem('idUsuario', '');

  // Enviamos el id de usuario a cualquier otro componente
  getIdUsuario = (): string => {
    return '6449b42aee2262f01223068f'
  } //this.localCache.idUsuario }//localStorage.getItem('idUsuario') || '';

  // Guardamos las clases en el store
  setClases = (clases: any): void => localStorage.setItem('clases', JSON.stringify(clases));

  // Obtenemos las clases del store
  getClases = (): any => JSON.parse(localStorage.getItem('clases') || '[]');

  // Guardamos los horarios en el store
  setHorarios = (horarios: any): void => localStorage.setItem('horarios', JSON.stringify(horarios));

  // Obtenemos los horarios del store
  getHorarios = (): any => JSON.parse(localStorage.getItem('horarios') || '[]');

  // Guardamos los clientes en el store
  setClientes = (clientes: any): void => localStorage.setItem('clientes', JSON.stringify(clientes));

  // Obtenemos los clientes del store
  getClientes = (): any => JSON.parse(localStorage.getItem('clientes') || '[]');

  // Guardamos el cliente en el store
  setCliente = (cliente: any): void => localStorage.setItem('cliente', JSON.stringify(cliente));

  // Obtenemos el cliente del store
  getCliente = (): any => JSON.parse(localStorage.getItem('cliente') || '{}');
}

export interface Cache {
  tipoUsuario: string;
  idUsuario: string;
  clases: any;
  horarios: any;
  clientes: any;
  cliente: any;
}
