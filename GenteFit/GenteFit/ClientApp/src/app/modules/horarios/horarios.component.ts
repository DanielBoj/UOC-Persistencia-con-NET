import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Horario } from 'src/app/models/interfaces/horario.model';
import { HorariosService } from 'src/app/services/horarios.service';
import { Observable, Subscription, from, of } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-horarios',
  templateUrl: './horarios.component.html',
  styleUrls: ['./horarios.component.css']
})
export class HorariosComponent implements OnInit, OnDestroy {

  // Estado obtenido del servicio redux
  tipoUsuario: string = '';
  idUsuario: string = '';

  // Contenedor para manejar los horarios de forma asíncrona
  horarios$: Subscription = new Subscription();
  horarios: Horario[] = [];
  subscripts: Array<Subscription> = [];

  // Título y subtítulo de la card
  title: string = "Horarios";
  subtitle: string = "Clases todos los días, nos adaptamos a tu horario"

  // Manejamos los datos para generar la tabla
  // Tabla dinámica
  displayedColumns: string[] = ['dia', 'hora', 'clase']
  displayedAdminColumns: string[] = ['dia', 'hora', 'clase', 'editar', 'eliminar']
  dataSource: MatTableDataSource<Horario> = new MatTableDataSource<Horario>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  constructor(private apiHorario: HorariosService,
    private redux: ReduxService) {

    // Obtenemos los horarios, los guardamos en el contenedor
    this.subscripts.push(
      this.horarios$ = this.apiHorario.getHorarios().subscribe((horarios) => {
        // Consumimos la API => Obtenemos los horarios
        this.horarios = horarios;
        // Guardamos los horarios en el estado
        this.redux.setHorarios(horarios);
        // Asignamos el datasource a la tabla
        this.dataSource = new MatTableDataSource<Horario>(horarios);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      })
    );
  }

  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();
    this.tipoUsuario = 'admin';
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

  // Obtenemos el estado
  getLocalStore = (): void => {
    this.tipoUsuario = this.redux.getTipoUsuario();
    this.idUsuario = this.redux.getIdUsuario();
  }

  // Borramos un horario
  deleteHorario = (id: string) => { }

  // Filtramos los datos de la tabla
  applyFilter = (event: Event) => {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
