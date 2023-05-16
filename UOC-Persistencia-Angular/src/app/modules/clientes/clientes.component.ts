import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription, filter, map } from 'rxjs';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { Espera } from 'src/app/models/interfaces/espera.model';
import { Reserva } from 'src/app/models/interfaces/reserva.model';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { Genero } from 'src/app/models/genero';
import { ReduxService } from 'src/app/services/redux.service';
import { UserService } from 'src/app/services/user.service';
import { HorariosService } from 'src/app/services/horarios.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent implements OnInit, OnDestroy {

  // Estado obtenido del servicio redux
  cache$: Subscription = new Subscription();
  cache!: Cache;
  tipoUsuario!: string;
  idUsuario!: string;

  // Contenedor para los datos del cliente => Para la vista del cliente
  cliente$: Subscription = new Subscription();
  cliente!: Cliente;;
  // Contenedor para los datos de todos los clientes => Para la vista del administrador y del empleado
  clientes$: Subscription = new Subscription();
  clientes: Cliente[] = [];

  // Contenedores auxiliares para los horarios => Para la vista del cliente, incluye reservas y esperas
  reservas$: Subscription = new Subscription();
  reservas: Reserva[] = [];
  esperas$: Subscription = new Subscription();
  esperas: Espera[] = [];

  subscripts: Array<Subscription> = [];

  // Información para la cabecera de la página
  title: string = "Información del cliente";
  subtitle: string = "Ponte en forma con gente como tú";

  // Información para el contenido de la card
  cuentaTitle: string = "Cuenta Usuario";
  cuentaSubtitle: string = "Aquí puedes ver la información de tu cuenta";

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

  // Modelo para la card de información del cliente
  clienteData: any = [
    { name: 'Email', value: '' },
    { name: 'Nombre', value: '' },
    { name: 'Dirección', value: '' },
    { name: 'Población', value: '' },
    { name: 'Código postal', value: '' },
    { name: 'País', value: '' },
    { name: 'Teléfono', value: '' },
    { name: 'Genero', value: '' },
    { name: 'IBAN', value: '' },
  ]

  // Manejamos los datos para generar la tabla
  // Tabla dinámica
  displayedColumns: string[] = ['email', 'nombre', 'nif', 'telefono', 'iban', 'detalle', 'editar', 'eliminar'];
  displayedEmpleadoColumns: string[] = ['email', 'nombre', 'nif', 'telefono', 'iban', 'detalle'];
  dataSource: MatTableDataSource<Cliente> = new MatTableDataSource<Cliente>();
  displayedReservaColumns: string[] = ['dia', 'hora', 'clase', 'profesor', 'duracion', 'eliminar']
  dataSourceReservas: MatTableDataSource<Reserva> = new MatTableDataSource<Reserva>();
  displayedEsperaColumns: string[] = ['dia', 'hora', 'clase', 'profesor', 'duracion', 'eliminar']
  dataSourceEsperas: MatTableDataSource<Espera> = new MatTableDataSource<Espera>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  // Flags para las acciones secundarias
  isClienteEdit: boolean = false;
  isClienteCreate: boolean = false;


  constructor(private localStorage: ReduxService,
    private apiUsuarios: UserService,
    private apiHorarios: HorariosService,
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    // Obtenemos el estado
    this.subscripts.push(this.cache$ = this.localStorage.getCache().subscribe(
      (cache) => {
        // Obtenemos el cache
        this.cache = cache;
        // Actualizamos el estado
        this.idUsuario = this.cache.idUsuario;
        this.tipoUsuario = this.cache.tipoUsuario;
      }));

    // Obtenemos la lista de clientes y los guardamos en el contenedor
    this.subscripts.push(
      this.clientes$ = this.apiUsuarios.getClientes().subscribe((clientes) => {
        // Consumimos las API => Obtenemos los clientes
        this.clientes = clientes;

        // Asignamos el datasource a la tabla
        this.dataSource = new MatTableDataSource<Cliente>(this.clientes);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;

        // Añadimos el filtrado por datos anidados
        this.dataSource.filterPredicate = (data, filter) => {
          const dataStr = JSON.stringify(data).toLowerCase();
          return dataStr.indexOf(filter) != -1;
        }
      })
    );

    // Obtenemos el cliente y lo guardamos en el contenedor
    this.subscripts.push(
      this.cliente$ = this.apiUsuarios.getCliente(this.idUsuario).subscribe((cliente) => {
        // Consumimos las API => Obtenemos el cliente
        this.cliente = cliente;

        // Actualizamos los datos de la card
        this.updateCard(this.cliente);
        // Actualizamos el modelo para el formulario
        this.updateForm(this.cliente);
      })
    );

    // Obtenemos las reservas, las filtramos y las guardamos en el contenedor
    this.subscripts.push(this.reservas$ = this.apiHorarios.getReservas().subscribe(
      (reservas) => {
        // Filtramos y asiganmos una lista de reservas
        this.reservas = reservas.filter((reserva: Reserva) => reserva.cliente.id === this.idUsuario);
        this.dataSourceReservas = new MatTableDataSource<Reserva>(this.reservas);
        this.dataSourceReservas.paginator = this.paginator;
        this.dataSourceReservas.sort = this.sort;

        // Añadimos el filtrado por datos anidados
        this.dataSourceReservas.filterPredicate = (data, filter) => {
          const dataStr = JSON.stringify(data).toLowerCase();
          return dataStr.indexOf(filter) != -1;
        }
      }
    ));

    // Obtenemos las esperas, las filtramos y las guardamos en el contenedor
    this.subscripts.push(this.esperas$ = this.apiHorarios.getEsperas().subscribe(
      (esperas) => {
        // Filtramos y asiganmos una lista de esperas
        this.esperas = esperas.filter((espera: Espera) => espera.cliente.id === this.idUsuario);
        this.dataSourceEsperas = new MatTableDataSource<Espera>(this.esperas);

        this.dataSourceEsperas.paginator = this.paginator;
        this.dataSourceEsperas.sort = this.sort;

        // Añadimos el filtrado por datos anidados
        this.dataSourceEsperas.filterPredicate = (data, filter) => {
          const dataStr = JSON.stringify(data).toLowerCase();
          return dataStr.indexOf(filter) != -1;
        }
      }
    ));
  }

  ngOnDestroy(): void {
    // Desuscribimos los observables
    this.subscripts.forEach(sub => sub.unsubscribe());
  }

  // Asignamos el paginador y el ordenador para nuestra tabla dinámica
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSourceReservas.paginator = this.paginator;
    this.dataSourceReservas.sort = this.sort;
    this.dataSourceEsperas.paginator = this.paginator;
    this.dataSourceEsperas.sort = this.sort;
  }

  // Todos los usuario
  getClientes = (): Observable<Cliente[]> => this.apiUsuarios.getClientes().pipe(map(clientes => clientes));

  // Obtenemos el cliente
  getCliente = (): Observable<Cliente> => this.apiUsuarios.getCliente(this.idUsuario).pipe(map(cliente => cliente));

  // Actualizar los datos de la card
  updateCard = (cliente: any): void => {
    // Actualizmos los datos de la card desde el observable
    this.clienteData = [
      { name: 'Email', value: cliente.email },
      { name: 'Nombre', value: cliente.nombre },
      { name: 'Dirección', value: cliente.direccion.domicilio },
      { name: 'Población', value: cliente.direccion.poblacion },
      { name: 'Código postal', value: cliente.direccion.cp as Number },
      { name: 'País', value: cliente.direccion.pais },
      { name: 'Teléfono', value: cliente.telefono },
      { name: 'Genero', value: Genero[cliente.genero as keyof typeof Genero] },
      { name: 'IBAN', value: cliente.iban },
    ];
  }

  // Actualizar los datos del formulario
  updateForm = (cliente: any): void => {
    // Actualizamos el modelo del formulario
    this.clienteModel = cliente;
  }

  // Enviamos el formulario al servicio
  onSubmit = (): void => {
    // Nos aseguramos de que el id sea correcto
    if (this.isClienteCreate) {
      this.clienteModel.id = this.idUsuario;
    } else {
      this.clienteModel.id = '';
    }

    // Parseamos el género mediante el enum Genero
    this.clienteModel.genero = parseInt(this.clienteModel.genero.toString());

    // Creamos el cliente que vamos a enviar
    const toSave: Cliente = this.clienteModel;

    // Enviamos la petición a la API
    try {
      if (this.isClienteEdit) this.apiUsuarios.editCliente(this.idUsuario, toSave);
      else this.apiUsuarios.createCliente(toSave);
    } catch (error) {
      // Mostramos un snackbar indicando que ha habido un error
      this.snackBar.open('Ha habido un error al editar el cliente', 'Cerrar', {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'center'
      });
    }

    // Actualizamos el flag
    this.isClienteEdit = false;
    this.isClienteCreate = false;

    // Limpiamos el formulario
    this.clienteData.forEach((data: any) => {
      if (typeof data.value === 'string') data.value = '';
      else if (typeof data.value === 'number') data.value = 0;
    });

    // Mostramos el mensaje de éxito
    this.snackBar.open('Cliente editado correctamente', 'Cerrar', {
      duration: 3000,
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
  }

  // Editar el cliente
  editarCliente = (cliente: Cliente): void => {
    // Actualizamos el modelo del formulario
    this.clienteModel = cliente;
    // Actualizamos el flag
    this.isClienteEdit = true;
  }

  // Eliminar el cliente
  eliminarCliente = (id: string): void => {
    // Nos aseguramos de que el id se pase de forma correcta
    let idToDelete: string = id.toString();

    // Enviamos la petición a la API
    try {
      this.apiUsuarios.deleteCliente(idToDelete);
    } catch (error) {
      // Mostramos un snackbar indicando que ha habido un error
      this.snackBar.open('Ha habido un error al eliminar el cliente', 'Cerrar', {
        duration: 3000,
        // Arriba y centrado
        verticalPosition: 'top',
        horizontalPosition: 'center'
      });

      // Salimos de la función
      return;
    }

    // Mostramos el mensaje de éxito
    this.snackBar.open('Cliente eliminado correctamente', 'Cerrar', {
      duration: 3000,
      // Arriba y centrado
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
  }

  eliminarReserva = (id: string): void => {
    // Enviamos la petición a la API
    try {
      this.apiHorarios.deleteReserva(id);
    } catch (error) {
      // Mostramos un snackbar indicando que ha habido un error
      this.snackBar.open('Ha habido un error al eliminar la reserva', 'Cerrar', {
        duration: 3000,
        // Arriba y centrado
        verticalPosition: 'top',
        horizontalPosition: 'center'
      });

      // Salimos de la función
      return;
    }

    // Mostramos el mensaje de éxito
    this.snackBar.open('Reserva eliminada correctamente', 'Cerrar', {
      duration: 3000,
      // Arriba y centrado
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
  }

  eliminarEspera = (id: string): void => {
    // Enviamos la petición a la API
    try {
      this.apiHorarios.deleteEspera(id);
    } catch (error) {
      // Mostramos un snackbar indicando que ha habido un error
      this.snackBar.open('Ha habido un error al eliminar la espera', 'Cerrar', {
        duration: 3000,
        // Arriba y centrado
        verticalPosition: 'top',
        horizontalPosition: 'center'
      });

      // Salimos de la función
      return;
    }

    // Mostramos el mensaje de éxito
    this.snackBar.open('Espera eliminada correctamente', 'Cerrar', {
      duration: 3000,
      // Arriba y centrado
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
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
