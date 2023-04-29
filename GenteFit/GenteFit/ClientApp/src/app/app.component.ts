import { Component } from '@angular/core';
import { DisplayService } from './services/display.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'GenteFit v1.0';

  // Añadimos el controlador para mostrar el menú, usaremos un input para recibir el valor del hijo
  showHeader$: Subscription = new Subscription();
  showHeader: boolean = true;

  constructor(private displayService: DisplayService) { }

  ngOnInit(): void {
    // Nos subscribimos al observable del servicio para recibir los valores
    this.showHeader$ = this.displayService.showHeader$.subscribe(
      (show: boolean) => this.showHeader = show
    );
  }

  ngOnDestroy(): void {
    // Nos desubscribimos del observable
    this.showHeader$.unsubscribe();
  }
}
