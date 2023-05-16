import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { OdooService } from 'src/app/services/odoo.service';
import { Proveedor } from 'src/app/models/interfaces/proveedor.model';
import { Producto } from 'src/app/models/interfaces/producto.model';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-proveedoresodoo',
    templateUrl: './proveedoresodoo.component.html',
    styleUrls: ['./proveedoresodoo.component.css']
})
export class ProveedoresodooComponent implements OnInit, OnDestroy {
    // Datos de redux
    cache$: Subscription = new Subscription();
    cache: Cache = {
        tipoUsuario: '',
        idUsuario: ''
    };

    // Datos de odoo
    proveedores$: Subscription = new Subscription();
    proveedores: Proveedor[] = [];
    nombresProveedores: string[] = [];

    subscrips: Subscription[] = [];

    // Modelo para el formulario de edición y creación de proveedores
    proveedorModel: Proveedor = {
        id: 0,
        name: '',
        nif: '',
        direccion: {
            domicilio: '',
            poblacion: '',
            cp: 0,
            pais: ''
        },
        email: '',
        phone: '',
        website: ''
    };

    // Manejamos los datos para las tablas de clientes, proveedores y productos
    proveedoresDataSource: MatTableDataSource<Proveedor> = new MatTableDataSource<Proveedor>();
    displayedProveedoresColumns: string[] = ['name', 'nif', 'email', 'phone', 'website', 'direccion'];

    // Paginadores y ordenadores de las tablas
    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    // Actualizadores
    @ViewChild('proveedoresTabla') proveedoresTabla: MatTable<any> | undefined;

    // Flags para las acciones secundarias
    flagCrearProveedor: boolean = false;
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
        this.subscrips.push(this.proveedores$ = this.odoo.getProveedores().subscribe(
            (proveedores) => {
                // Cargamos la lista de nombres de proveedores
                proveedores.forEach((proveedor: any) => {
                    // Añadimos el nombre del proveedor a la lista
                    this.nombresProveedores.push(proveedor.name);
                    // Añadimos el proveedor a la lista
                    this.proveedores.push(proveedor);

                    // Cargamos los datos de la tabla dinámica
                    this.proveedoresDataSource = new MatTableDataSource<Proveedor>(this.proveedores);
                    this.proveedoresDataSource.paginator = this.paginator;
                    this.proveedoresDataSource.sort = this.sort;

                    // Filtramos los datos de la tabla
                    this.proveedoresDataSource.filterPredicate = (data, filter) => {
                        const dataStr = JSON.stringify(data).toLowerCase();
                        return dataStr.indexOf(filter) != -1;
                    }

                    // Ocultamos el spinner
                    this.isLoading = false;
                });
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
        this.proveedoresDataSource.paginator = this.paginator;
        this.proveedoresDataSource.sort = this.sort;
    }

    // Acciones para la tabla de proveedores
    // Acción para crear un proveedor
    crearProveedor = () => {
        // Nos aseguramos de que el id sea correcto
        this.proveedorModel.id = 0;

        this.proveedorModel.direccion.cp = parseInt(this.proveedorModel.direccion.cp.toString().replace(/^0+/, ''));

        // Creamos el proveedor que vamos a enviar
        const toSave: Proveedor = this.proveedorModel

        // Enviamos el proveedor a Odoo
        try {
            this.odoo.createProveedor(toSave);
        } catch (error) {
            // Mostramos un snackbar indicando que ha habido un error
            this.snackbar.open('Ha habido un error al crear el proveedor', 'Cerrar', {
                duration: 3000,
                verticalPosition: 'top',
                horizontalPosition: 'center'
            });
        }

        this.flagCrearProveedor = false;

        // Limpiamos el formulario
        this.limpiarFormularios();

        // Actualizamos
        this.subscrips.push(this.proveedores$ = this.odoo.getProveedores().subscribe(
            (proveedores) => {
                // Cargamos la lista de nombres de proveedores
                proveedores.forEach((proveedor: any) => {
                    // Añadimos el nombre del proveedor a la lista
                    this.nombresProveedores.push(proveedor.name);
                    // Añadimos el proveedor a la lista
                    this.proveedores.push(proveedor);

                    // Cargamos los datos de la tabla dinámica
                    this.proveedoresDataSource = new MatTableDataSource<Proveedor>(this.proveedores);
                    this.proveedoresDataSource.paginator = this.paginator;
                    this.proveedoresDataSource.sort = this.sort;

                    // Filtramos los datos de la tabla
                    this.proveedoresDataSource.filterPredicate = (data, filter) => {
                        const dataStr = JSON.stringify(data).toLowerCase();
                        return dataStr.indexOf(filter) != -1;
                    }
                });
            }
        ));
        this.proveedoresTabla?.renderRows();

        // Mostramos el mensaje de éxito
        this.snackbar.open('Proveedor creado correctamente', 'Cerrar', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
        });
    }

    // Limpiamos los formularios
    limpiarFormularios = () => {
        // Limpiamos el formulario de proveedores
        this.proveedorModel = {
            id: 0,
            name: '',
            nif: '',
            direccion: {
                domicilio: '',
                poblacion: '',
                cp: 0,
                pais: ''
            },
            email: '',
            phone: '',
            website: ''
        };
    }

    // Filtramos los datos de la tabla
    applyFilter = (event: Event) => {
        const filterValue = (event.target as HTMLInputElement).value;
        this.proveedoresDataSource.filter = filterValue.trim().toLowerCase();

        if (this.proveedoresDataSource.paginator) {
            this.proveedoresDataSource.paginator.firstPage();
        }
    }
}

