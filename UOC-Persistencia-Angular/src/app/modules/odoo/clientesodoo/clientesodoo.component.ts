import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { OdooService } from 'src/app/services/odoo.service';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
    selector: 'app-clientesodoo',
    templateUrl: './clientesodoo.component.html',
    styleUrls: ['./clientesodoo.component.css']
})
export class ClientesodooComponent implements OnInit, OnDestroy {
    // Datos de redux
    cache$: Subscription = new Subscription();
    cache: Cache = {
        tipoUsuario: '',
        idUsuario: ''
    };

    // Datos de odoo
    clientes$: Subscription = new Subscription();
    clientes: Cliente[] = [];

    subscrips: Subscription[] = [];

    // Modelo para el formulario de edición y creación de clientes
    clienteModel: Cliente = {
        id: '',
        email: '',
        pass: '',
        nombre: '',
        nif: '',
        direccion: {
            domicilio: '',
            poblacion: '',
            cp: 0,
            pais: ''
        },
        telefono: '',
        genero: 0,
        iban: '',
        tipo: 'cliente'
    };

    // Manejamos los datos para las tablas de clientes, proveedores y productos
    dataSource: MatTableDataSource<Cliente> = new MatTableDataSource<Cliente>();
    displayedColumns: string[] = ['nombre', 'email', 'telefono', 'direccion', 'detalle'];

    // Paginadores y ordenadores de las tablas
    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    // Actualizadores
    @ViewChild('clientesTabla') clientesTabla: MatTable<any> | undefined;

    // Flags para las acciones secundarias
    flagCrearCliente: boolean = false;
    isLoading: boolean = false;

    constructor(private redux: ReduxService,
        private odoo: OdooService,
        private snackbar: MatSnackBar,
        private router: Router) { }

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
        this.subscrips.push(this.clientes$ = this.odoo.getClientes().subscribe(
            (clientes) => {
                // Cargamos la lista de clientes
                this.clientes = clientes;

                // Cargamos los datos de la tabla dinámica
                this.dataSource = new MatTableDataSource<Cliente>(this.clientes);
                this.dataSource.paginator = this.paginator;
                this.dataSource.sort = this.sort;

                // Filtramos los datos de la tabla
                this.dataSource.filterPredicate = (data, filter) => {
                    const dataStr = JSON.stringify(data).toLowerCase();
                    return dataStr.indexOf(filter) != -1;
                }

                // Ocultamos el spinner
                this.isLoading = false;
            }));
    }

    // Acciones en la destrucción del componente
    ngOnDestroy(): void {
        // Cancelamos las subscripciones a la API
        this.subscrips.forEach(sub => sub.unsubscribe());
    }

    // Asignamos el paginador y el ordenador para nuestra tabla dinámica
    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    // Acciones para la tabla de clientes
    // Acción para mostrar el formulario de creación de clientes

    // Acción para crear un cliente
    crearCliente = () => {
        // Nos aseguramos de que el id sea correcto
        this.clienteModel.id = '';
        // Nos aseguramos de que no haya 0 a la izquierda en el CP
        this.clienteModel.direccion.cp = parseInt(this.clienteModel.direccion.cp.toString().replace(/^0+/, ''));

        // Seteamos un género por defecto
        this.clienteModel.genero = 4;
        this.clienteModel.tipo = 'cliente';

        // Creamos el cliente que vamos a enviar
        const toSave: Cliente = this.clienteModel

        // Enviamos el cliente a Odoo
        try {
            this.odoo.createCliente(toSave);
        } catch (error) {
            // Mostramos un snackbar indicando que ha habido un error
            this.snackbar.open('Ha habido un error al crear el cliente', 'Cerrar', {
                duration: 3000,
                verticalPosition: 'top',
                horizontalPosition: 'center'
            });
        }

        this.flagCrearCliente = false;

        // Limpiamos el formulario
        this.limpiarFormularios();

        // Actualizamos
        this.subscrips.push(this.clientes$ = this.odoo.getClientes().subscribe(
            (clientes) => {
                // Cargamos la lista de clientes
                this.clientes = clientes;

                // Cargamos los datos de la tabla dinámica
                this.dataSource = new MatTableDataSource<Cliente>(this.clientes);
                this.dataSource.paginator = this.paginator;
                this.dataSource.sort = this.sort;

                // Filtramos los datos de la tabla
                this.dataSource.filterPredicate = (data, filter) => {
                    const dataStr = JSON.stringify(data).toLowerCase();
                    return dataStr.indexOf(filter) != -1;
                }
            }));
        this.clientesTabla?.renderRows();

        // Mostramos el mensaje de éxito
        this.snackbar.open('Cliente editado correctamente', 'Cerrar', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
        });
    }

    // Limpiamos los formularios
    limpiarFormularios = () => {
        // Limpiamos el formulario de clientes
        this.clienteModel = {
            id: '',
            email: '',
            pass: '',
            nombre: '',
            nif: '',
            direccion: {
                domicilio: '',
                poblacion: '',
                cp: 0,
                pais: ''
            },
            telefono: '',
            genero: 0,
            iban: '',
            tipo: 'cliente'
        };
    }

    // Navegar a la vista de detalle de un cliente
    verCliente = (id: string) => {
        const url: string = '/clientedetalle/' + id;
        this.router.navigateByUrl(url);
    }

    // Filtramos los datos de la tabla
    applyFilter = (event: Event) => {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();

        if (this.dataSource.paginator) {
            this.dataSource.paginator.firstPage();
        }
    }
}
