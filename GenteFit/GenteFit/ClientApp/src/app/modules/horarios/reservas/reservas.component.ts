import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Reserva } from 'src/app/models/interfaces/reserva.model';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { HorariosService } from 'src/app/services/horarios.service';
import { Subscription, from, of } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dias } from 'src/app/models/dias';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-reservas',
  templateUrl: './reservas.component.html',
  styleUrls: ['./reservas.component.css']
})
export class ReservasComponent implements OnInit, OnDestroy {

  // Estado obtenido del servicio redux
  cache$: Subscription = new Subscription();
  cache!: Cache;
  tipoUsuario!: string;
  idUsuario!: string;

  // Contenedor para manejar los horarios de forma asíncrona
  reservas$: Subscription = new Subscription();
  reservas: Reserva[] = [];
  subscripts: Array<Subscription> = [];

  // Título y subtítulo de la card
  title: string = "Reservas";
  subtitle: string = "Lista de reservas para las clases"

  // // Flag para mostrar el formulario de creación
  // showForm: boolean = false;

  // Manejamos los datos para generar la tabla
  // Tabla dinámica
  displayedColumns: string[] = ['clase', 'dia', 'hora', 'nombre', 'nif']
  displayedAdminColumns: string[] = ['clase', 'dia', 'hora', 'nombre', 'nif', 'borrar']
  dataSource: MatTableDataSource<Reserva> = new MatTableDataSource<Reserva>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  constructor(private apiHorario: HorariosService,
    private redux: ReduxService,
    private snackbar: MatSnackBar) { }


  ngOnInit(): void {
    // Obtenemos el estado
    this.subscripts.push(this.cache$ = this.redux.getCache().subscribe(
      (cache) => {
        // Cargamos los datos del estado
        this.cache = cache;
        this.tipoUsuario = cache.tipoUsuario;
        this.idUsuario = cache.idUsuario;
      }
    ));

    // Obtenemos las reservas, los guardamos en el contenedor
    this.subscripts.push(this.reservas$ = this.apiHorario.getReservas().subscribe((reservas) => {
      // Consumimos la API => Obtenemos las reservas
      this.reservas = reservas;
      // Asignamos los valores a los días de la semana meidante el enum Dias
      this.reservas.forEach((reserva) => {
        reserva.horario.dia = Dias[reserva.horario.dia as keyof typeof Dias];
      });
      // Asignamos los valores a la tabla
      this.dataSource = new MatTableDataSource(this.reservas);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

      // Añadimos el filtrado por datos anidados
      this.dataSource.filterPredicate = (data, filter) => {
        const dataStr = JSON.stringify(data).toLowerCase();
        return dataStr.indexOf(filter) != -1;
      }
    }
    ));
  }

  ngOnDestroy(): void {
    this.subscripts.forEach(sub => sub.unsubscribe());
  }

  // Asignamos el paginador y el ordenador para nuestra tabla dinámica
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  // Borramos una reserva
  deleteReserva = (id: string) => {
    // Enviamos la petición a la API
    try {
      this.apiHorario.deleteReserva(id);
    } catch (error) {
      // Mostaramos un mensaje de error mediante snackbar
      this.snackbar.open('Error al borrar la reserva', 'Cerrar', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom'
      });

      // Salimos del método
      return;
    }

    // Mostramos un mensaje de éxito mediante snackbar
    this.snackbar.open('Reserva borrada correctamente', 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom'
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
