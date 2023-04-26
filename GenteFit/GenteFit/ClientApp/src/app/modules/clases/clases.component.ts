import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { Observable, map } from 'rxjs';
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
export class ClasesComponent implements OnInit, AfterViewInit {

  // Estado obtenido del servicio redux
  tipoUsuario: string = '';
  idUsuario: string = '';

  // Contenedor para las clases y una clase en concreto
  clases$: Observable<Clase[]> = new Observable<Clase[]>();
  clase$: Observable<Clase> = new Observable<Clase>();

  // Observable
  dataObservable = new Observable(observer => {
    observer.next(this.getClases());
  });

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
    private apiClases: ClasesService) { }

  ngOnInit(): void {

    // Obtenemos el estado
    this.getLocalStore();
    this.tipoUsuario = 'admin';

    // Cargamos las clases
    this.getClases();

    // Asignamos el datasource a la tabla
    this.dataSource = new MatTableDataSource<Clase>(this.redux.getClases());
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  // Obtenemos el estado
  getLocalStore = (): void => {
    this.tipoUsuario = this.redux.getTipoUsuario();
    this.idUsuario = this.redux.getIdUsuario();
  }

  // Obtenemos todas las clases
  getClases = () => {
    this.apiClases.getClases().pipe(data => this.clases$ = data);
    // Creamos un contenedor temporal
    // let clases: Clase[];

    // this.clases$.subscribe(data => clases = data);

    // Guardamos las clases en el store
    this.clases$.subscribe(data => this.redux.setClases(data));
  }

  // Obtenemos una clase por id
  getClase = (id: string) => {
    this.apiClases.getClase(id).pipe(data => this.clase$ = data);
    // Creamos un contenedor temporal
    let clase: Clase;

    // Guardamos las clase en el store
    this.clase$.subscribe(data => this.redux.setClases(data));
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
