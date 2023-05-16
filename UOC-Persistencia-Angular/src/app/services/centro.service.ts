import { Injectable } from '@angular/core';
import { Observable, of, map, catchError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Centro } from '../models/interfaces/centro.model';
import { environment as env } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CentroService {

  private url: string = `${env.api}centro` //'http://localhost:5000/api/centro';

  constructor(private http: HttpClient) { }

  // Obtenemos el centro
  getCentro = (): Observable<any> => {
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

  // Actualizamos el centro
  editCentro = (id: string, centro: Centro): Observable<any> => {
    // Generamos la URL para la petición
    const url = `${this.url}/${id}`;

    // Realizamos la petición
    let res: any;

    try {
      this.http.put(url, centro).subscribe((data) => res = data);
    } catch (error: any) {
      res = error.message;
    }

    return res;
  }
}
