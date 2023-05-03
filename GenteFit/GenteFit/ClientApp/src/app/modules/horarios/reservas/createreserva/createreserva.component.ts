import { Component, ViewChild } from '@angular/core';
import { Horario } from 'src/app/models/interfaces/horario.model';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { Reserva } from 'src/app/models/interfaces/reserva.model';
import { Dias } from 'src/app/models/dias';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { ReduxService } from 'src/app/services/redux.service';
import { HorariosService } from 'src/app/services/horarios.service';
import { UserService } from 'src/app/services/user.service';
import { Observable, Subscription, catchError, map, of, pipe, tap, throwError } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Espera } from 'src/app/models/interfaces/espera.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Genero } from 'src/app/models/genero';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-createreserva',
  templateUrl: './createreserva.component.html',
  styleUrls: ['./createreserva.component.css']
})
export class CreatereservaComponent {

  // Datos del estado de la apliación, lo necesitamos para obtener el usuario logeado
  cache$: Subscription = new Subscription();
  cache!: Cache;
  idUsuario!: string;
  tipoUsuario!: string;


  // Creamos los contenedores para trabajar con la clase
  horarios$: Subscription = new Subscription();
  horarios: Horario[] = [];
  horario!: Horario;
  cliente$: Subscription = new Subscription();
  //clientes: Cliente[] = [];
  cliente!: Cliente;

  // Insertamos los array para la búsqueda y filtrado
  nombresClases: string[] = [];

  subscriptions: Array<Subscription> = [];

  // Creamos un array con los horarios filtrados
  horariosFiltrados: Horario[] = [];

  // Creamos los días para el select y el día seleccionado
  dias: Array<string> = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];

  // Creamos los datos para generar la tabla de opciones
  // Tabla dinámica
  displayedColumns: string[] = ['dia', 'hora', 'clase.nombre', 'crear'];
  dataSource: MatTableDataSource<Horario> = new MatTableDataSource<Horario>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // Modelo vacío para la reserva
  reservaModel: Reserva = {
    horario: {
      dia: 0,
      hora: '',
      clase: {
        id: '',
        nombre: '',
        descripcion: '',
        profesor: '',
        duracion: 0,
        plazas: 0
      },
    },
    cliente: {
      id: '',
      email: '',
      pass: '',
      nombre: '',
      nif: '',
      direccion: {
        id: '',
        domicilio: '',
        poblacion: '',
        cp: 0,
        pais: ''
      },
      telefono: '',
      genero: 0,
      iban: '',
      tipo: 'cliente'
    }
  }

  // Modelo vacío para la espera
  esperaModel: Espera = {
    horario: {
      dia: 0,
      hora: '',
      clase: {
        id: '',
        nombre: '',
        descripcion: '',
        profesor: '',
        duracion: 0,
        plazas: 0
      },
    },
    cliente: {
      id: '',
      email: '',
      pass: '',
      nombre: '',
      nif: '',
      direccion: {
        id: '',
        domicilio: '',
        poblacion: '',
        cp: 0,
        pais: ''
      },
      telefono: '',
      genero: 0,
      iban: '',
      tipo: 'cliente'
    }
  }

  constructor(private apiHorario: HorariosService,
    private apiCliente: UserService,
    private redux: ReduxService,
    private snackbar: MatSnackBar) { }

  ngOnInit(): void {
    // Obtenemos antes que nada el usuario logeado
    this.subscriptions.push(this.cache$ = this.redux.getCache().subscribe(
      (cache) => {
        // Guardamos el usuario logeado
        this.cache = cache;
        // Guardamos el id del usuario logeado
        this.idUsuario = this.cache.idUsuario;
        // Guardamos el tipo de usuario logeado
        this.tipoUsuario = this.cache.tipoUsuario;
      }
    ));

    // Obtenemos todos los horarios
    this.subscriptions.push(this.horarios$ = this.apiHorario.getHorarios().subscribe(
      (horarios) => {
        // Guardamos los horarios obtenidos
        this.horarios = horarios;
        // Convertimos el día a string usando la clase enum Dias
        this.horarios.forEach(
          (horario) => {
            horario.dia = Dias[horario.dia as keyof typeof Dias];
          }
        )
        // Generamos un array con los nombres de las clases y eliminamos los duplicados convirtiendo el Array en un Set, ya que los set no pueden contener duplicados.
        this.nombresClases = this.horarios.map(horario => horario.clase.nombre);
        // Ordenamos el array alfabéticamente
        this.nombresClases.sort();
        // Lo convertimos en un Set para eliminar duplicados
        this.nombresClases = [...new Set(this.nombresClases)];
        // Generamos un array con los horarios filtrados
        this.horariosFiltrados = this.horarios;
        // Creamos los datos para la tabla dinámica
        this.dataSource = new MatTableDataSource(this.horariosFiltrados);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        // Añadimos el filtrado por datos anidados
        this.dataSource.filterPredicate = (data, filter) => {
          const dataStr = JSON.stringify(data).toLowerCase();
          return dataStr.indexOf(filter) != -1;
        }
      }));
    // Obtenemos el cliente
    this.subscriptions.push(this.cliente$ = this.apiCliente.getCliente(this.idUsuario).subscribe(
      (cliente) => {
        // Guardamos el cliente obtenido
        this.cliente = cliente;
      }
    ));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  // Asignamos el paginador y el ordenador para nuestra tabla dinámica
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  // Guardamos la reserva en la BD.
  createReserva = async () => {
    // Generamos el elemento a crear
    this.reservaModel.horario = this.horario;
    this.reservaModel.cliente = this.cliente;
    // Convertimos el dia a número usando la clase enum Dias
    this.reservaModel.horario.dia = Dias[this.reservaModel.horario.dia as keyof typeof Dias];

    // Creamos el objeto a guardar
    const toSave: Reserva = this.reservaModel;
    toSave.id = '';

    // Obtenemos la id del cliente
    const idCliente: string = this.cliente.id as string;

    // Obtenemos la id del horario
    const idHorario: string = this.horario.id as string;

    // Llamamos a la función de creación del servicio
    // Hay que enviarle el id del cliente y el id del horario
    this.apiHorario.createReserva(idCliente, idHorario, toSave).pipe(
      tap((res: any) => {
        if (res) {
          this.createReservaSuccess();
        }
      }),
      catchError((error: HttpErrorResponse) => {
        this.createReservaError();
        return throwError(() => error.ok);
      })).subscribe();

    // this.apiHorario.createReserva(idCliente, idHorario, toSave).subscribe(
    //   (res: any) => {
    //     console.log(res);
    //     if (res.ok) {
    //       this.createReservaSuccess();
    //     } else {
    //       this.createReservaError();
    //     }
    //   },
    //   (error: HttpErrorResponse) => {
    //     this.createReservaError();
    //   }
    // );

  }

  // Mensaje de éxito al crear una espera
  createReservaSuccess = () => {
    this.snackbar.open('Reserva creada con éxito', 'Cerrar', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }
  // Creamos un mensaje de error mediante snackbar, ofrecerá la posibilidad
  // crear una espera en caso de que la reserva no se haya podido crear
  // o de cancelar la creación de la reserva
  createReservaError = () => {
    const snackbarRef = this.snackbar.open('No se ha podido crear la reserva, ¿desea crear una espera?', 'Crear Espera', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });

    // Si se pulsa el botón de crear, se creará una espera
    snackbarRef.onAction().subscribe(
      () => {
        this.createEspera();
      }
    );
  }

  // Guardamos la reserva en la BD.
  createEspera = async () => {
    // Generamos el elemento a crear
    this.esperaModel.horario = this.horario;
    this.esperaModel.cliente = this.cliente;


    // Creamos el objeto a guardar
    const toSave: Espera = this.esperaModel;
    toSave.id = '';

    // Obtenemos la id del cliente
    const idCliente: string = this.cliente.id || '';

    // Obtenemos la id del horario
    const idHorario: string = this.horario.id || '';

    // Llamamos a la función de creación del servicio
    // Hay que enviarle el id del cliente y el id del horario
    this.apiHorario.createEspera(idCliente, idHorario, toSave).pipe(
      tap((res: any) => {
        if (res) {
          this.createEsperaSuccess();
        }
      }),
      catchError((error: HttpErrorResponse) => {
        this.createEsperaError();
        return throwError(() => error.ok);
      })).subscribe();
  }

  // Mensaje de éxito al crear una espera
  createEsperaSuccess = () => {
    this.snackbar.open('Espera creada con éxito', 'Cerrar', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }
  // Creamos un mensaje de error mediante snackbar, ofrecerá la posibilidad
  // crear una espera en caso de que la reserva no se haya podido crear
  // o de cancelar la creación de la reserva
  createEsperaError = () => {
    const snackbarRef = this.snackbar.open('No se ha podido crear la espera', 'Cerrar', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  // Obtenemos una lista filtrada con los nombres de las clases disponibles
  onSearch = (event: Event) => {
    // Obtenemos el valor del input
    const nombre = (event.target as HTMLInputElement).value;

    // Filtramos los horarios por el nombre de la clase y los asignamos al array de horarios filtrados
    this.horariosFiltrados = this.horarios.filter(horario => horario.clase.nombre.toLowerCase().includes(nombre.toLowerCase()));

    // Renovamos los datos de la tabla dinámica
    this.dataSource = new MatTableDataSource(this.horariosFiltrados);
  }

  // Repetimos la lógica para un retorno de datos al seleccionar una opción de la lista
  onSelectChange = (nombre: string) => {
    // Filtramos los horarios por el nombre de la clase y los asignamos al array de horarios filtrados
    this.horariosFiltrados = this.horarios.filter(horario => horario.clase.nombre.toLowerCase().includes(nombre.toLowerCase()));

    // Renovamos los datos de la tabla dinámica
    this.dataSource = new MatTableDataSource(this.horariosFiltrados);
  }

  // Creamos la lógica para la selección de un horario de la tabla
  onSelect = (id: string) => {
    // Asignamos el horario seleccionado
    this.horario = this.horarios.find(horario => horario.id === id)!;

    // Llamamos a la función de creación de la reserva
    this.createReserva();
  }

  // Obtenemos el estado de la aplicación
  // getLocalStore = (): void => {
  //   this.tipoUsuario = this.redux.getTipoUsuario();
  //   this.idUsuario = this.redux.getIdUsuario();
  // }

  // Filtramos los datos de la tabla
  applyFilter = (event: Event) => {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
