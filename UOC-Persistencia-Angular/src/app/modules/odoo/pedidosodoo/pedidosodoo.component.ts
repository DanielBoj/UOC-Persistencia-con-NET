import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { OdooService } from 'src/app/services/odoo.service';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Pedido } from 'src/app/models/interfaces/pedido.model';

@Component({
    selector: 'app-pedidosodoo',
    templateUrl: './pedidosodoo.component.html',
    styleUrls: ['./pedidosodoo.component.css']
})
export class PedidosodooComponent implements OnInit, OnDestroy {
    // Datos de redux
    cache$: Subscription = new Subscription();
    cache: Cache = {
        tipoUsuario: '',
        idUsuario: ''
    };

    // Datos de odoo
    // Producto 4 -> Añadimos las entidades Empleados y Pedidos
    pedidos$: Subscription = new Subscription();
    pedidos: any[] = [];

    subscrips: Subscription[] = [];

    // Manejamos los datos para las tablas de clientes, proveedores y productos
    // Producto 4 -> Añadimos las tablas Empleados y Pedidos
    pedidosDataSource: MatTableDataSource<Pedido> = new MatTableDataSource<any>();
    displayedPedidosColumns: string[] = ['displayName', 'dateOrder', 'company', 'partner', 'amountUntaxed', 'amountTotal', 'taxGroupName', 'state'];

    // Paginadores y ordenadores de las tablas
    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    // Actualizadores
    // Producto 4 -> Añadimos los actualizadores de las tablas Empleados y Pedidos
    @ViewChild('pedidosTabla') pedidosTabla: MatTable<any> | undefined;

    // Flags para las acciones secundarias
    isLoading: boolean = false;

    constructor(private redux: ReduxService,
        private odoo: OdooService,
        private snackbar: MatSnackBar) { }

    // Acciones en la inicialización del componente
    ngOnInit(): void {
        // Mostramos un spinner mientras se cargan los datos
        this.isLoading = true;

        // Obtenemos el estado de la aplicación
        this.subscrips.push(this.cache$ = this.redux.getCache().subscribe((cache: Cache) => {
            this.cache = cache;
        }));

        // TODO -> Eliminar tras los tests
        this.cache.tipoUsuario = 'admin';

        // Obtenemos los datos de Odoo: Clientes, proveedores y productos
        this.subscrips.push(this.pedidos$ = this.odoo.getPedidos().subscribe(
            (pedidos) => {
                // Seteamos el estado de los pedidos
                pedidos.forEach(
                    (pedido: Pedido) => this.setPedidoState(pedido)
                );
                // Cargamos la lista de pedidos
                this.pedidos = pedidos;
                // Cargamos los datos de la tabla dinámica
                this.pedidosDataSource = new MatTableDataSource<Pedido>(this.pedidos);
                this.pedidosDataSource.paginator = this.paginator;
                this.pedidosDataSource.sort = this.sort;
                // Filtramos los datos de la tabla
                this.pedidosDataSource.filterPredicate = (data, filter) => {
                    const dataStr = JSON.stringify(data).toLowerCase();
                    return dataStr.indexOf(filter) != -1;
                }

                // Ocultamos el spinner
                this.isLoading = false;
            }
        ));
    }

    // Acciones en la destrucción del componente
    ngOnDestroy(): void {
        // Cancelamos las subscripciones a la API
        this.subscrips.forEach(sub => sub.unsubscribe());
    }

    // Asignamos el paginador y el ordenador para nuestra tabla dinámica
    ngAfterViewInit() {
        this.pedidosDataSource.paginator = this.paginator;
        this.pedidosDataSource.sort = this.sort;
    }

    // Seteamos el estado de los pedidos
    setPedidoState = (pedido: Pedido) => {
        switch (pedido.deliveryStatus) {
            case 'full': pedido.state = 'Entregado';
                break;
            case 'partial': pedido.state = 'Parcialmente entregado';
                break;
            default: pedido.state = 'No entregado';
        }
    }

    // Filtramos los datos de la tabla
    applyFilter = (event: Event) => {
        const filterValue = (event.target as HTMLInputElement).value;
        this.pedidosDataSource.filter = filterValue.trim().toLowerCase();

        if (this.pedidosDataSource.paginator) {
            this.pedidosDataSource.paginator.firstPage();
        }
    }
}

