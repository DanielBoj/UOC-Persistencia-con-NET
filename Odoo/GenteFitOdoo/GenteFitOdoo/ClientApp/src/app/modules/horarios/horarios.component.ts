import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Horario } from 'src/app/models/interfaces/horario.model';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { HorariosService } from 'src/app/services/horarios.service';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dias } from 'src/app/models/dias';

@Component({
  selector: 'app-horarios',
  templateUrl: './horarios.component.html',
  styleUrls: ['./horarios.component.css']
})
export class HorariosComponent implements OnInit, OnDestroy {

  // Estado obtenido del servicio redux
  cache$: Subscription = new Subscription();
  cache!: Cache;
  tipoUsuario!: string;
  idUsuario!: string;

  // Contenedor para manejar los horarios de forma asíncrona
  horarios$: Subscription = new Subscription();
  horarios: Horario[] = [];
  subscripts: Array<Subscription> = [];

  // Título y subtítulo de la card
  title: string = "Horarios";
  subtitle: string = "Clases todos los días, nos adaptamos a tu horario"

  // Flag para mostrar el formulario de creación
  showForm: boolean = false;

  // Flags mensajería
  isDeleted: boolean = false;
  isDeletedError: boolean = false;

  // Manejamos los datos para generar la tabla
  // Tabla dinámica
  displayedColumns: string[] = ['dia', 'hora', 'clase.nombre', 'detalles']
  displayedAdminColumns: string[] = ['dia', 'hora', 'clase.nombre', 'detalles', 'eliminar']
  dataSource: MatTableDataSource<Horario> = new MatTableDataSource<Horario>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  constructor(private apiHorario: HorariosService,
    private redux: ReduxService,) { }

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

    // Obtenemos los horarios, los guardamos en el contenedor
    this.subscripts.push(
      this.horarios$ = this.apiHorario.getHorarios().subscribe((horarios) => {
        // Consumimos la API => Obtenemos los horarios
        this.horarios = horarios;
        // Asignamos los valores a los días de la semana meidante el enum Dias
        this.horarios.forEach((horario) => {
          horario.dia = Dias[horario.dia as keyof typeof Dias];
        });
        // Asignamos el datasource a la tabla
        this.dataSource = new MatTableDataSource<Horario>(horarios);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;

        // Añadimos el filtrado por datos anidados
        this.dataSource.filterPredicate = (data, filter) => {
          const dataStr = JSON.stringify(data).toLowerCase();
          return dataStr.indexOf(filter) != -1;
        }
      })
    );
  }

  ngOnDestroy(): void {
    // Destruimos el lazo entre el servicio y el componente
    this.subscripts.forEach((sub) => sub.unsubscribe());
  }

  // Asignamos el paginador y el ordenador para nuestra tabla dinámica
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  // Borramos un horario
  deleteHorario = (id: string) => {
    console.log(id);
    // Consumimos la API => Borramos el horario
    try {
      // Tras el borrado, eliminamos el horario del varaiable local para reflejarlo en la tabla
      this.apiHorario.deleteHorario(id).subscribe((data) => this.isDeleted = true);
    } catch (error) {
      this.isDeletedError = true;
    }
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
