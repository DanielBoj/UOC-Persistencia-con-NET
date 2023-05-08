import { Injectable } from '@angular/core';
import { Observable, of, map, catchError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Clase } from '../models/interfaces/clase.model';
import { environment as env } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClasesService {

  private url: string = `${env.api}clase`  //'http://localhost:5000/api/clase';

  constructor(private http: HttpClient) { }

  // Obtenemos las clases
  getClases = (): Observable<any> => {
    // Generamos la URL para la petición
    const url = `${this.url}`;

    // Realizamos la petición
    // Obtenemos los datos de la api, nos devuelve un array de usuarios
    return this.http.get(url).pipe(catchError(error => {
      console.log(error);
      // En caso de error devolvemos un array vacío
      return of([]);
    }));
  }

  // Obtenemos una clase por id
  getClase = (id: string): Observable<any> => {
    // Generamos la URL para la petición
    const url = `${this.url}/${id}`;
    console.log(url.toString());

    // Realizamos la petición
    // Obtenemos los datos de la api, nos devuelve un array de usuarios
    return this.http.get(url).pipe(catchError(error => {
      console.log(error);
      // En caso de error devolvemos un array vacío
      return of([]);
    }));
  }

  // Creamos una nueva clase
  createClase = (clase: Clase): Observable<any> => {
    // Generamos la URL para la petición
    const url = `${this.url}`;

    // Realizamos la petición
    // Obtenemos los datos de la api, nos devuelve un array de usuarios
    // let res: any;
    // this.http.post(url, clase).subscribe(data => res = data);
    // return res;

    try {
      // Realizamos la petición
      // Obtenemos los datos de la api, nos devuelve un array de usuarios
      let res: any;
      this.http.post(url, clase).subscribe(data => res = data);
      return res;
    } catch (error) {
      console.log(error);
      return of();
    }
  }

  // Actualizamos una clase
  updateClase = (clase: Clase): Observable<any> => {
    // Generamos la URL para la petición
    const url = `${this.url}/${clase.id}`;

    // Realizamos la petición
    // Obtenemos los datos de la api, nos devuelve un array de usuarios
    return this.http.put(url, clase).pipe(catchError(error => {
      console.log(error);

      // En caso de error devolvemos un array vacío
      return of([]);
    }));
  }

  // Borramos una clase
  deleteClase = (id: string): Observable<any> => {
    // Generamos la URL para la petición
    const url = `${this.url}/${id}`;

    try {
      // Realizamos la petición
      // Obtenemos los datos de la api, nos devuelve un array de usuarios
      let res: any;
      this.http.delete<any>(url).subscribe(data => res = data);
      return of(true);
    } catch (error) {
      console.log(error);
      return of(false);
    }
  }
}
