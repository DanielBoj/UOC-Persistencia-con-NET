import { Component, OnInit, AfterViewInit, ViewChild, OnDestroy } from '@angular/core';
import { Observable, Subscription, map } from 'rxjs';
import { Clase } from 'src/app/models/interfaces/clase.model';
import { ClasesService } from 'src/app/services/clases.service';
import { ReduxService } from 'src/app/services/redux.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-clases',
  templateUrl: './clases.component.html',
  styleUrls: ['./clases.component.css']
})
export class ClasesComponent implements OnInit, AfterViewInit, OnDestroy {

  // Estado obtenido del servicio redux
  tipoUsuario: string = '';
  idUsuario: string = '';

  // Contenedor para las clases y una clase en concreto
  clases$: Subscription = new Subscription();
  clases: Clase[] = [];
  subscripts: Array<Subscription> = [];

  // Clase seleccionada
  claseId: string | undefined;

  // Título y subtítulo de la card
  title: string = "Clases";
  subtitle: string = "Escoge entre una gran variedad de clases"

  // Tabla dinámica
  displayedColumns: string[] = ['nombre', 'descripcion', 'profesor', 'duracion', 'plazas', 'detalles']
  displayedAdminColumns: string[] = ['nombre', 'descripcion', 'profesor', 'duracion', 'plazas', 'detalles', 'editar', 'eliminar']
  dataSource: MatTableDataSource<Clase> = new MatTableDataSource<Clase>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private redux: ReduxService,
    private apiClases: ClasesService) {
    // Cargamos las clases como una subscripción a un observable para manejar la asincronía	
    this.subscripts.push(
      this.clases$ = this.apiClases.getClases().subscribe((clases) => {
        // Consumimos la API => Obtenemos las clases
        this.clases = clases;
        // Guardamos las clases en el estado
        this.redux.setClases(clases);
        // Asignamos el datasource a la tabla
        this.dataSource = new MatTableDataSource<Clase>(clases);
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

  // Asignamos el paginador y el ordenador para nuestra tabla dinámica
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  // Nos desuscribimos de los observables al destruir el componente
  ngOnDestroy(): void {
    this.subscripts.forEach(sub => sub.unsubscribe());
  }

  // Obtenemos el estado
  getLocalStore = (): void => {
    this.tipoUsuario = this.redux.getTipoUsuario();
    this.idUsuario = this.redux.getIdUsuario();
  }

  // Borramos una clase
  deleteClase = (id: string) => {
    this.apiClases.deleteClase(id);

    // Recargamos las clases
    //this.getClases();
  }

  // Filtramos la tabla
  applyFilter = (event: Event) => {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
