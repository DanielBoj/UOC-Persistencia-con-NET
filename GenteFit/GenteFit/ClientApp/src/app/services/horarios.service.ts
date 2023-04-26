import { Injectable } from '@angular/core';
import { Observable, of, map, catchError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Horario } from '../models/interfaces/horario.model';

@Injectable({
  providedIn: 'root'
})
export class HorariosService {

  private url: string = 'http://localhost:5000/api/horario';

  constructor(private http: HttpClient) { }

  // Obtenemos todos los horarios
  getHorarios = (): Observable<any> => {
    // Construimos la url
    const url = `${this.url}`;

    // Realizamos la petición
    return this.http.get(url).pipe(catchError(error => {
      console.log(error);
      return of([]);
    }));
  }
}
