/* Este servicio maneja los estados de la aplicación, intercambia valores entre componentes y mantiene la integridad de los datos. */
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Cache } from '../models/interfaces/cache.model';

@Injectable({
  providedIn: 'root'
})
export class ReduxService {

  // Generamos un observable para poder subscribirnos a él
  localCache$: Observable<Cache> = new Observable<Cache>();
  // Decalramos un Behavior para que almacene los datos
  private localCache: BehaviorSubject<Cache> = new BehaviorSubject<Cache>(
    {
      tipoUsuario: '',
      idUsuario: '',
    }
  );

  constructor() {
    // Inicializamos el observable
    this.localCache$ = this.localCache as Observable<Cache>;
  }

  // El componente de login nos envía el id de usuario y su tipo
  setCache = (cache: Cache) => this.localCache.next(cache);

  // Enviamos el id de usuario y su tipo a cualquier otro componente
  getCache = (): Observable<Cache> => this.localCache$;
}
