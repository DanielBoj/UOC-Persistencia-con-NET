import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription, filter, map } from 'rxjs';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { ReduxService } from 'src/app/services/redux.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent implements OnInit, OnDestroy {

  // Estado obtenido del servicio redux
  tipoUsuario: string = '';
  idUsuario: string = '';

  // Contenedor para los datos del cliente => Para la vista del cliente
  cliente$: Subscription = new Subscription();
  cliente: Cliente = {} as Cliente;
  // Contenedor para los datos de todos los clientes => Para la vista del administrador y del empleado
  clientes$: Subscription = new Subscription();
  clientes: Cliente[] = [];

  subscripts: Array<Subscription> = [];

  // Información para la cabecera de la página
  title: string = "GentFit";
  subtitle: string = "Ponte en forma con gente como tú";

  // Información para el contenido de la card
  cuentaTitle: string = "Cuenta Usuario";
  cuentaSubtitle: string = "Aquí puedes ver la información de tu cuenta";

  clienteData: any[] = [];

  // Contenedor para los datos del cliente
  //cliente$: Observable<Cliente> = new Observable<Cliente>();

  // Contenedor para todos los clientes, para la vista del administrador y del empleado
  //clientes$: Observable<Cliente[]> = new Observable<Cliente[]>();

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
    reservas: [],
    esperas: [],
    tipo: 'cliente'
  };

  // Manejamos los datos para generar la tabla
  // Tabla dinámica
  displayedColumns: string[] = ['email', 'nombre', 'nif', 'telefono', 'iban', 'editar', 'eliminar'];
  dataSource: MatTableDataSource<Cliente> = new MatTableDataSource<Cliente>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  // Flags para las acciones secundarias
  isClienteEdit: boolean = false;


  constructor(private localStorage: ReduxService,
    private apiUsuarios: UserService) {
    // Para poder cargar los datos, tenemos que obtener el estado
    this.getLocalStore();

    // Obtenemos la lista de clientes y los guardamos en el contenedor
    this.subscripts.push(
      this.clientes$ = this.apiUsuarios.getClientes().subscribe((clientes) => {
        // Consumimos las API => Obtenemos los clientes
        this.clientes = clientes;
        // Guardamos los datos en el localStore
        this.localStorage.setClientes(clientes);
        // Asignamos el datasource a la tabla
        this.dataSource = new MatTableDataSource<Cliente>(clientes);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      })
    );

    // Obtenemos el cliente y lo guardamos en el contenedor
    this.subscripts.push(
      this.cliente$ = this.apiUsuarios.getCliente(this.idUsuario).subscribe((cliente) => {
        // Consumimos las API => Obtenemos el cliente
        this.cliente = cliente;
        // Guardamos los datos en el localStore
        this.localStorage.setCliente(cliente);
        // Actualizamos los datos de la card
        this.updateCard(cliente);
        // Actualizamos el modelo para el formulario
        this.updateForm(cliente);
      }
      )
    );

    this.getLocalStore();
  }

  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();

    // Obtenemos las clases a las que está apuntado el cliente

    // Obtenemos los horarios reservados del cliente

    // Obtenemos la lista de espera del cliente

    // Cargamos los datos de la card
    // this.updateCard();

    // Cargamos el modelo para el formulario
    // this.updateForm();
  }

  ngOnDestroy(): void {
    // Desuscribimos los observables
    this.subscripts.forEach(sub => sub.unsubscribe());
  }

  // Asignamos el paginador y el ordenador para nuestra tabla dinámica
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  // Obtenemos el estado
  getLocalStore = (): void => {
    this.tipoUsuario = this.localStorage.getTipoUsuario().toString();
    this.idUsuario = this.localStorage.getIdUsuario().toString();
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
      { name: 'Código postal', value: cliente.direccion.cp },
      { name: 'País', value: cliente.direccion.pais },
      { name: 'Teléfono', value: cliente.telefono },
      { name: 'Genero', value: cliente.email },
      { name: 'IBAN', value: cliente.iban },
    ];
  }

  // Actualizar los datos del formulario
  updateForm = (cliente: any): void => {
    // Actualizamos el modelo del formulario
    this.clienteModel = cliente;
  }

  // Enviamos el formulario al servicio
  onSubmit = (): void => { }

  // Editar el cliente
  editarCliente = (cliente: Cliente): void => {
    // Actualizamos el modelo del formulario
    this.clienteModel = cliente;
    // Actualizamos el flag
    this.isClienteEdit = true;
  }

  // Eliminar el cliente
  eliminarCliente = (cliente: Cliente): void => { }

  // Filtramos los datos de la tabla
  applyFilter = (event: Event) => {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
