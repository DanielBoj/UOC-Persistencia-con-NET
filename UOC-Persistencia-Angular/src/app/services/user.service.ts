import { Injectable } from '@angular/core';
import { Observable, of, map, catchError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { User, UserBody } from '../models/interfaces/user.model';
import { environment as env } from '../../environments/environment';
import { Cliente } from '../models/interfaces/cliente.model';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    private url: string = `${env.api}` //'http://localhost:5000/api';

    constructor(private http: HttpClient) { }

    // Obtenemos todos los usuarios
    getUsers = (): Observable<any> => {
        // Generamos la URL para la petición
        const url = `${this.url}users`;

        // Realizamos la petición
        // Obtenemos los datos de la api, nos devuelve un array de usuarios
        return this.http.get<User[]>(url).pipe(catchError(error => {
            console.log(error);

            // En caso de error devolvemos un array vacío
            return of([]);
        }));
    }

    // Obtenemos el listado de clientes
    getClientes = (): Observable<any> => {
        // Generamos la URL para la petición
        const url = `${this.url}cliente`;

        // Realizamos la petición
        // Obtenemos los datos de la api, nos devuelve un array de clientes
        return this.http.get<Cliente[]>(url).pipe(catchError(error => {
            console.log(error);
            // En caso de error devolvemos un array vacío
            return of([]);
        }));
    }

    // Obtenemos el cliente por su id
    getCliente = (id: string): Observable<any> => {
        // Generamos la URL para la petición
        const url = `${this.url}cliente/${id}`;

        // Realizamos la petición
        // Obtenemos los datos de la api, nos devuelve un cliente
        return this.http.get<Cliente>(url).pipe(catchError(error => {
            console.log(error);
            // En caso de error devolvemos un array vacío
            return of();
        }));
    }

    // Creamos un cliente
    createCliente = (cliente: Cliente): Observable<any> => {
        // Generamos la URL para la petición
        const url = `${this.url}cliente`;

        // Realizamos la petición
        let res: any;

        try {
            this.http.post(url, cliente).subscribe((data) => res = data);
        } catch (error: any) {
            res = error.message;
        }

        return res;
    }

    // Editamos un cliente
    editCliente = (id: string, cliente: Cliente): Observable<any> => {
        // Generamos la URL para la petición
        const url = `${this.url}cliente/${id}`;

        // Realizamos la petición
        let res: any;

        try {
            this.http.put(url, cliente).subscribe((data) => res = data);
        } catch (error: any) {
            res = error.message;
        }

        return res;
    }

    // Borramos un cliente
    deleteCliente = (id: string): Observable<any> => {
        // Generamos la URL para la petición
        const url = `${this.url}cliente/${id}`;

        // Realizamos la petición
        try {
            this.http.delete<any>(url).subscribe((data) => data);
            return of(true);
        } catch (error) {
            return of(false);
        }
    }
}
