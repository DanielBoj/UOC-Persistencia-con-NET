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

  // Datos de odoo
  clientes$: Subscription = new Subscription();
  clientes: Cliente[] = [];
  proveedores$: Subscription = new Subscription();
  proveedores: Proveedor[] = [];
  nombresProveedores: string[] = [];
  productos$: Subscription = new Subscription();
  productos: Producto[] = [];

  subscrips: Subscription[] = [];

  // Lista de categorías de productos
  categorias: string[] = ProductoCategorias;

  // Título y subtítulo del componente
  title: string = 'Pasarela Comercial Odoo'
  subtitle: string = 'Información sobre los clientes, proveedores y productos en venta en los gimnasios Gente Fit'

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
  clientesDataSource: MatTableDataSource<Cliente> = new MatTableDataSource<Cliente>();
  displayedClientesColumns: string[] = ['nombre', 'email', 'telefono', 'direccion', 'detalle'];
  proveedoresDataSource: MatTableDataSource<Proveedor> = new MatTableDataSource<Proveedor>();
  displayedProveedoresColumns: string[] = ['name', 'nif', 'email', 'phone', 'website', 'direccion'];
  productosDataSource: MatTableDataSource<Producto> = new MatTableDataSource<Producto>();
  displayedProductosColumns: string[] = ['defaultCode', 'name', 'categ', 'standardPrice'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // Actualizadores
  @ViewChild('clientesTabla') clientesTabla: MatTable<any> | undefined;
  @ViewChild('proveedoresTabla') proveedoresTabla: MatTable<any> | undefined;
  @ViewChild('productosTabla') productosTabla: MatTable<any> | undefined;

  // Flags para las acciones secundarias
  flagCrearCliente: boolean = false;
  flagCrearProveedor: boolean = false;
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

    // Obtenemos los datos de Odoo: Clientes, proveedores y productos
    this.subscrips.push(this.clientes$ = this.odoo.getClientes().subscribe(
      (clientes) => {
        // Cargamos la lista de clientes
        this.clientes = clientes;

        // Cargamos los datos de la tabla dinámica
        this.clientesDataSource = new MatTableDataSource<Cliente>(this.clientes);
        this.clientesDataSource.paginator = this.paginator;
        this.clientesDataSource.sort = this.sort;

        // Filtramos los datos de la tabla
        this.clientesDataSource.filterPredicate = (data, filter) => {
          const dataStr = JSON.stringify(data).toLowerCase();
          return dataStr.indexOf(filter) != -1;
        }
      }));

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
    this.clientesDataSource.paginator = this.paginator;
    this.clientesDataSource.sort = this.sort;
    this.proveedoresDataSource.paginator = this.paginator;
    this.proveedoresDataSource.sort = this.sort;
    this.productosDataSource.paginator = this.paginator;
    this.productosDataSource.sort = this.sort;
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
        this.clientesDataSource = new MatTableDataSource<Cliente>(this.clientes);
        this.clientesDataSource.paginator = this.paginator;
        this.clientesDataSource.sort = this.sort;

        // Filtramos los datos de la tabla
        this.clientesDataSource.filterPredicate = (data, filter) => {
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
    this.clientesDataSource.filter = filterValue.trim().toLowerCase();
    this.proveedoresDataSource.filter = filterValue.trim().toLowerCase();
    this.productosDataSource.filter = filterValue.trim().toLowerCase();

    if (this.clientesDataSource.paginator) {
      this.clientesDataSource.paginator.firstPage();
    }
    if (this.proveedoresDataSource.paginator) {
      this.proveedoresDataSource.paginator.firstPage();
    }
    if (this.productosDataSource.paginator) {
      this.productosDataSource.paginator.firstPage();
    }
  }
}
