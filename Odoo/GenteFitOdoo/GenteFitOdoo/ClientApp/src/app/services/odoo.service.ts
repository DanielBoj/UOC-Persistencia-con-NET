import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, tap, throwError } from 'rxjs';
import { environment as env } from 'src/environments/environment';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { Proveedor } from '../models/interfaces/proveedor.model';
import { Producto } from '../models/interfaces/producto.model';

@Injectable({
  providedIn: 'root'
})
export class OdooService {

  // Url de conexión a la API
  private urlClientes: string = `${env.api}odoo/clientes`
  private urlProveedores: string = `${env.api}odoo/proveedores`
  private urlProductos: string = `${env.api}odoo/productos`

  constructor(private http: HttpClient) { }

  // Obtenemos los datos de los clientes
  getClientes = (): Observable<any> => {
    // Construimos la url
    const url = `${this.urlClientes}`;

    // Realizamos la petición
    return this.http.get(url).pipe(catchError(error => {
      console.log(error.message);
      // Si hay un error devolvemos un array vacío
      return of({})
    }));
  }

  // Creamos un nuevo cliente
  createCliente = (cliente: Cliente): Observable<any> => {
    // Construimos la url
    const url = `${this.urlClientes}`;

    // Realizamos la petición
    return this.http.post(url, cliente).pipe(catchError(error => {
      console.log(error.message);
      return throwError(() => { return { ok: false, error: error.error }; });
    }));
  }

  // Obtenemos los datos de los proveedoes
  getProveedores = (): Observable<any> => {
    // Construimos la url
    const url = `${this.urlProveedores}`;

    // Realizamos la petición e imprimimos por consola el resultado o el error
    return this.http.get(url).pipe(catchError(error => {
      console.log(error.message);
      // Si hay un error devolvemos un array vacío
      return of({})
    }),
      // Imprimimos por consola el resultado
      /*tap((result) => console.log(result))*/);
  }

  // Creamos un nuevo proveedor
  createProveedor = (proveedor: Proveedor): Observable<any> => {
    // Construimos la url
    const url = `${this.urlProveedores}`;

    // Realizamos la petición
    return this.http.post(url, proveedor).pipe(catchError(error => {
      console.log(error.message);
      return throwError(() => { return { ok: false, error: error.error }; });
    }));
  }

  // Obtenemos los datos de los productos
  getProductos = (): Observable<any> => {
    // Construimos la url
    const url = `${this.urlProductos}`;

    // Realizamos la petición
    return this.http.get(url).pipe(catchError(error => {
      console.log(error.message);
      // Si hay un error devolvemos un array vacío
      return of({})
    }));
  }

  // Creamos un nuevo producto
  createProducto = (producto: Producto): Observable<any> => {
    // Construimos la url
    const url = `${this.urlProductos}`;

    // Realizamos la petición
    return this.http.post(url, producto).pipe(catchError(error => {
      console.log(error.message);
      return throwError(() => { return { ok: false, error: error.error }; });
    }));
  }
}
