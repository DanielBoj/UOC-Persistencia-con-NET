import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Espera } from 'src/app/models/interfaces/espera.model';
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
  selector: 'app-esperas',
  templateUrl: './esperas.component.html',
  styleUrls: ['./esperas.component.css']
})
export class EsperasComponent implements OnInit, OnDestroy {

  // Estado obtenido del servicio redux
  cache$: Subscription = new Subscription();
  cache!: Cache;
  tipoUsuario!: string;
  idUsuario!: string;

  // Contenedor para manejar los horarios de forma asíncrona
  esperas$: Subscription = new Subscription();
  esperas: Espera[] = [];
  subscripts: Array<Subscription> = [];

  // Título y subtítulo de la card
  title: string = "Reservas";
  subtitle: string = "Lista de reservas para las clases"

  // Flag para mostrar el formulario de creación
  showForm: boolean = false;

  // Flags mensajería
  isDeleted: boolean = false;
  isDeletedError: boolean = false;

  // Manejamos los datos para generar la tabla
  // Tabla dinámica
  displayedColumns: string[] = ['clase', 'dia', 'hora', 'nombre', 'nif']
  displayedAdminColumns: string[] = ['clase', 'dia', 'hora', 'nombre', 'nif', 'borrar']
  dataSource: MatTableDataSource<Espera> = new MatTableDataSource<Espera>();

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
        this.tipoUsuario = this.cache.tipoUsuario;
        this.idUsuario = this.cache.idUsuario;
      }
    ));

    // Obtenemos las reservas, los guardamos en el contenedor
    this.subscripts.push(this.esperas$ = this.apiHorario.getEsperas().subscribe((esperas) => {
      // Consumimos la API => Obtenemos las reservas
      this.esperas = esperas;
      // Asignamos los valores a los días de la semana meidante el enum Dias
      this.esperas.forEach((espera) => {
        espera.horario.dia = Dias[espera.horario.dia as keyof typeof Dias];
      });
      // Asignamos los valores a la tabla
      this.dataSource = new MatTableDataSource(this.esperas);
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
    // Llamamos al servicio para borrar la reserva
    try {
      this.apiHorario.deleteEspera(id);
    } catch (error) {
      // Mostramos un mensaje de error con snackbar
      this.snackbar.open('Error al borrar la reserva', 'Cerrar', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom'
      });

      // Salimos de la función
      return;
    }

    // Mostramos un mensaje de éxito con snackbar
    this.snackbar.open('Reserva borrada con éxito', 'Cerrar', {
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
