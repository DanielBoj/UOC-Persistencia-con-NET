import { Injectable } from '@angular/core';
import { Observable, of, map, catchError, Subscription, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Horario } from '../models/interfaces/horario.model';
import { Espera } from '../models/interfaces/espera.model';
import { Reserva } from '../models/interfaces/reserva.model';
import { environment as env } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HorariosService {

  private urlHorario: string = `${env.api}horario` //'http://localhost:5000/api/horario';
  private urlReserva: string = `${env.api}reserva` //'http://localhost:5000/api/reserva';
  private urlEspera: string = `${env.api}espera` //'http://localhost:5000/api/espera';

  res$: Subscription = new Subscription();
  res: any;

  constructor(private http: HttpClient) { }

  // Obtenemos todos los horarios
  getHorarios = (): Observable<any> => {
    // Construimos la url
    const url = `${this.urlHorario}`;

    // Realizamos la petición
    return this.http.get(url).pipe(catchError(error => {
      console.log(error);
      return of([]);
    }));
  }

  // Obtenemos un horario
  getHorario = (id: string): Observable<any> => {
    // Construimos la url
    const url = `${this.urlHorario}/${id}`;

    // Realizamos la petición
    return this.http.get(url).pipe(catchError(error => {
      console.log(error);
      return of([]);
    }));
  }

  // Creamos un horario
  createHorario = (id: string, horario: Horario): Observable<any> => {
    // Construimos la url
    const url = `${this.urlHorario}/${id}`;

    // Realizamos la petición
    try {
      // Obtenemos los datos de la api
      let res: any;
      this.http.post(url, horario).subscribe(data => res = data);
      return res;
    } catch (error) {
      console.log(error);
      return of();
    }
  }

  // Actualizamos un horario
  updateHorario = (horario: Horario): Observable<any> => {
    // Construimos la url
    const url = `${this.urlHorario}/${horario.id}`;

    // Realizamos la petición
    try {
      // Obtenemos los datos de la api
      let res: any;
      this.http.put(url, horario).subscribe(data => res = data);
      return res;
    } catch (error) {
      console.log(error);
      return of();
    }
  }

  // Eliminamos un horario
  deleteHorario = (id: string): Observable<any> => {
    // Construimos la URL
    const url = `${this.urlHorario}/${id}`;

    // Realizamos la petición
    try {
      // Obtenemos los datos de la api
      let res: any;
      this.http.delete(url).subscribe(data => res = data);
      return res;
    } catch (error) {
      console.log(error);
      return of(false);
    }
  }

  // Obtenemos los horarios de un usuario

  // Obtenemos todas las reservas
  getReservas = (): Observable<any> => {
    // Construimos la url
    const url = `${this.urlReserva}`;

    // Realizamos la petición
    // Obtenemos los datos de la api.
    return this.http.get(url).pipe(catchError(error => {
      return of([]);
    }));
  }

  // Obtenemos una reserva
  getReserva = (id: string): Observable<any> => {
    // Construimos la url
    const url = `${this.urlReserva}/${id}`;

    // Realizamos la petición
    // Obtenemos los datos de la api.
    return this.http.get(url).pipe(catchError(error => {
      console.log(error);
      return of([]);
    }));
  }

  // Creamos una reserva
  createReserva = (idCliente: string, idHorario: string, reserva: Reserva): Observable<any> => {
    //Construimos la url
    const url = `${this.urlReserva}/${idCliente};${idHorario}`;

    return this.http.post(url, reserva).pipe(catchError((error: HttpErrorResponse) => {
      return throwError(() => { return { ok: false, error: error.error }; });
    }));
  }

  // TODO: Actualizamos una reserva

  // Eliminamos una reserva
  deleteReserva = (id: string): Observable<any> => {
    // Construimos la URL
    const url = `${this.urlReserva}/${id}`;

    // Realizamos la petición
    try {
      // Obtenemos los datos de la api
      let res: any;
      this.http.delete(url).subscribe(data => res = data);
      return res;
    } catch (error) {
      return of(false);
    }
  }

  // Obtenemos todas las esperas
  getEsperas = (): Observable<any> => {
    // Construimos la URL
    const url = `${this.urlEspera}`;

    // Realizamos la petición
    // Obtenemos los datos de la api.
    return this.http.get(url).pipe(catchError(error => {
      return error.message;
    }
    ));
  }

  // Obtenemos una espera
  getEspera = (id: string): Observable<any> => {
    // Construimos la url
    const url = `${this.urlEspera}/${id}`;

    // Realizamos la petición
    // Obtenemos los datos de la api.
    return this.http.get(url).pipe(catchError(error => {
      console.log(error);
      return of([]);
    }));
  }

  // Creamos una espera
  createEspera = (idCliente: string, idHorario: string, espera: Espera): Observable<any> => {
    // Construimos la url
    const url = `${this.urlEspera}/${idCliente};${idHorario}`;
    console.log(url, espera);
    // Obtenemos los datos de la api
    return this.http.post(url, espera).pipe(catchError((error: HttpErrorResponse) => {
      return throwError(() => { return { ok: false, error: error.error }; });
    }));
  }

  // TODO: Actualizamos una espera

  // Eliminamos una espera
  deleteEspera = (id: string): Observable<any> => {
    // Construimos la url
    const url = `${this.urlEspera}/${id}`;

    // Realizamos la petición
    try {
      // Obtenemos los datos de la API
      let res: any;
      this.http.delete(url).subscribe(data => res = data);
      return res;
    } catch (error) {
      return of(false);
    }
  }

}
