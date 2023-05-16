import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { Cache } from 'src/app/models/interfaces/cache.model';

@Component({
    selector: 'app-odoo',
    templateUrl: './odoo.component.html',
    styleUrls: ['./odoo.component.css']
})
export class OdooComponent implements OnInit, OnDestroy {
    // Datos de redux
    cache$: Subscription = new Subscription();
    cache: Cache = {
        tipoUsuario: '',
        idUsuario: ''
    };

    subscrips: Subscription[] = [];

    // Título y subtítulo del componente
    title: string = 'Pasarela Comercial Odoo'
    subtitle: string = 'Información sobre los clientes, proveedores y productos en venta en los gimnasios Gente Fit'

    constructor(private redux: ReduxService) { }

    // Acciones en la inicialización del componente
    ngOnInit(): void {
        // Obtenemos el estado de la aplicación
        this.subscrips.push(this.cache$ = this.redux.getCache().subscribe((cache: Cache) => {
            this.cache = cache;
        }));

        // TODO -> Eliminar tras los tests
        this.cache.tipoUsuario = 'admin';
    }

    // Acciones en la destrucción del componente
    ngOnDestroy(): void {
        // Cancelamos las subscripciones a la API
        this.subscrips.forEach(sub => sub.unsubscribe());
    }
}
