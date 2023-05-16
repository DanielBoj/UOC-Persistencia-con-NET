import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { OdooService } from 'src/app/services/odoo.service';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Empleado } from 'src/app/models/interfaces/empleado.model';

@Component({
    selector: 'app-empleadosodoo',
    templateUrl: './empleadosodoo.component.html',
    styleUrls: ['./empleadosodoo.component.css']
})
export class EmpleadosodooComponent implements OnInit, OnDestroy {
    // Datos de redux
    cache$: Subscription = new Subscription();
    cache: Cache = {
        tipoUsuario: '',
        idUsuario: ''
    };

    // Datos de odoo
    // Producto 4 -> Añadimos las entidades Empleados y Pedidos
    empleados$: Subscription = new Subscription();
    empleados: any[] = [];

    subscrips: Subscription[] = [];


    // Manejamos los datos para las tablas de clientes, proveedores y productos
    // Producto 4 -> Añadimos las tablas Empleados y Pedidos
    empleadosDataSource: MatTableDataSource<Empleado> = new MatTableDataSource<any>();
    displayedEmpleadosColumns: string[] = ['name', 'email', 'department'];


    // Paginadores y ordenadores de las tablas
    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    // Producto 4 -> Añadimos los actualizadores de las tablas Empleados y Pedidos
    @ViewChild('empleadosTabla') empleadosTabla: MatTable<any> | undefined;

    // Flags para las acciones secundarias
    isLoading: boolean = false;

    constructor(private redux: ReduxService,
        private odoo: OdooService,) { }

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
        // Producto 4 -> Añadimos los datos de Empleados y Pedidos
        this.subscrips.push(this.empleados$ = this.odoo.getEmpleados().subscribe(
            (empleados) => {
                // Cargamos la lista de empleados
                this.empleados = empleados;
                // Cargamos los datos de la tabla dinámica
                this.empleadosDataSource = new MatTableDataSource<Empleado>(this.empleados);
                this.empleadosDataSource.paginator = this.paginator;
                this.empleadosDataSource.sort = this.sort;
                // Filtramos los datos de la tabla
                this.empleadosDataSource.filterPredicate = (data, filter) => {
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
        this.empleadosDataSource.paginator = this.paginator;
        this.empleadosDataSource.sort = this.sort;
    }

    // Filtramos los datos de la tabla
    applyFilter = (event: Event) => {
        const filterValue = (event.target as HTMLInputElement).value;
        this.empleadosDataSource.filter = filterValue.trim().toLowerCase();

        if (this.empleadosDataSource.paginator) {
            this.empleadosDataSource.paginator.firstPage();
        }
    }
}

