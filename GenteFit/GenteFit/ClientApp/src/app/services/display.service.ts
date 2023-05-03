import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

// Este servicio emite un observable al que nos podemos subscribir para emitir valores
@Injectable({
  providedIn: 'root'
})
export class DisplayService {

  // Creamoos un objeto que nos servirá para indicar que no debe mostrarse el menú
  constructor() {
    this.showHeader$ = this.showHeader.asObservable();
  }

  public showHeader$: Observable<boolean>;

  private showHeader: Subject<boolean> = new Subject<boolean>();

  public setShowHeader = (show: boolean) => this.showHeader.next(show);
}
