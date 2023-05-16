import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { Cache } from 'src/app/models/interfaces/cache.model';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnDestroy, OnInit {

  // Datos del estado de la aplicación
  cache$: Subscription = new Subscription();
  cache: Cache = {
    tipoUsuario: '',
    idUsuario: ''
  };

  subscrips: Subscription[] = [this.cache$];

  constructor(private redux: ReduxService) { }

  ngOnInit() {
    // Cargamos el estado de la aplicación.
    this.subscrips.push(this.cache$ = this.redux.getCache().subscribe((data: Cache) => {
      this.cache = data;
    }
    ));
  }

  ngOnDestroy() {
  }

}
