import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { OdooService } from 'src/app/services/odoo.service';
import { Proveedor } from 'src/app/models/interfaces/proveedor.model';
import { Producto } from 'src/app/models/interfaces/producto.model';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { ProductoCategorias } from 'src/app/models/producto.categorias';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Empleado } from 'src/app/models/interfaces/empleado.model';
import { Pedido } from 'src/app/models/interfaces/pedido.model';

@Component({
    selector: 'app-productosodoo',
    templateUrl: './productosodoo.component.html',
    styleUrls: ['./productosodoo.component.css']
})
export class ProductosodooComponent {
    // Datos de redux
    cache$: Subscription = new Subscription();
    cache: Cache = {
        tipoUsuario: '',
        idUsuario: ''
    };

    // Datos de odoo
    productos$: Subscription = new Subscription();
    productos: Producto[] = [];

    subscrips: Subscription[] = [];

    // Lista de categorías de productos
    categorias: string[] = ProductoCategorias;

    // Modelo para el formulario de edición y creación de productos
    productoModel: Producto = {
        id: 0,
        defaultCode: '',
        name: '',
        categ: '',
        listPrice: 0.00,
        standardPrice: 0.00,
    };

    // Manejamos los datos para las tablas de clientes, proveedores y productos
    productosDataSource: MatTableDataSource<Producto> = new MatTableDataSource<Producto>();
    displayedProductosColumns: string[] = ['defaultCode', 'name', 'categ', 'standardPrice'];

    // Paginadores y ordenadores de las tablas
    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    // Actualizadores
    @ViewChild('productosTabla') productosTabla: MatTable<any> | undefined;

    // Flags para las acciones secundarias
    flagCrearProducto: boolean = false;
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
        this.subscrips.push(this.productos$ = this.odoo.getProductos().subscribe(
            (productos) => {
                // Cargamos la lista de productos
                this.productos = productos;

                // Cargamos los datos de la tabla dinámica
                this.productosDataSource = new MatTableDataSource<Producto>(this.productos);
                this.productosDataSource.paginator = this.paginator;
                this.productosDataSource.sort = this.sort;

                // Filtramos los datos de la tabla
                this.productosDataSource.filterPredicate = (data, filter) => {
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
        this.productosDataSource.paginator = this.paginator;
        this.productosDataSource.sort = this.sort;
    }

    // Acciones para la tabla de productos
    // Acción para crear un producto
    crearProducto = () => {
        // Nos aseguramos de que el id sea correcto
        this.productoModel.id = 0;
        this.productoModel.listPrice = this.productoModel.standardPrice;
        this.productoModel.id = 0;

        // Creamos el producto que vamos a enviar
        const toSave: Producto = this.productoModel

        // Enviamos el producto a Odoo
        try {
            this.odoo.createProducto(toSave);
        } catch (error) {
            // Mostramos un snackbar indicando que ha habido un error
            this.snackbar.open('Ha habido un error al crear el producto', 'Cerrar', {
                duration: 3000,
                verticalPosition: 'top',
                horizontalPosition: 'center'
            });
        }

        this.flagCrearProducto = false;

        // Limpiamos el formulario
        this.limpiarFormularios();

        this.subscrips.push(this.productos$ = this.odoo.getProductos().subscribe(
            (productos) => {
                // Cargamos la lista de productos
                this.productos = productos;

                // Cargamos los datos de la tabla dinámica
                this.productosDataSource = new MatTableDataSource<Producto>(this.productos);
                this.productosDataSource.paginator = this.paginator;
                this.productosDataSource.sort = this.sort;

                // Filtramos los datos de la tabla
                this.productosDataSource.filterPredicate = (data, filter) => {
                    const dataStr = JSON.stringify(data).toLowerCase();
                    return dataStr.indexOf(filter) != -1;
                }
            }));
        this.productosTabla?.renderRows();

        // Mostramos el mensaje de éxito
        this.snackbar.open('Producto creado correctamente', 'Cerrar', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
        });
    }

    // Limpiamos los formularios
    limpiarFormularios = () => {
        // Limpiamos el formulario de productos
        this.productoModel = {
            id: 0,
            defaultCode: '',
            name: '',
            categ: '',
            listPrice: 0.00,
            standardPrice: 0.00,
        };
    }

    // Filtramos los datos de la tabla
    applyFilter = (event: Event) => {
        const filterValue = (event.target as HTMLInputElement).value;
        this.productosDataSource.filter = filterValue.trim().toLowerCase();

        if (this.productosDataSource.paginator) {
            this.productosDataSource.paginator.firstPage();
        }
    }
}

