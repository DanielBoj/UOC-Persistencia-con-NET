import { Component, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, filter, map } from 'rxjs';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { ReduxService } from 'src/app/services/redux.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent implements OnInit {

  // Estado obtenido del servicio redux
  tipoUsuario: string = '';
  idUsuario: string = '';

  // Información para la cabecera de la página
  title: string = "GentFit";
  subtitle: string = "Ponte en forma con gente como tú";

  // Información para el contenido de la card
  cuentaTitle: string = "Cuenta Usuario";
  cuentaSubtitle: string = "Aquí puedes ver la información de tu cuenta";

  clienteData: any[] = [];

  // Contenedor para los datos del cliente
  cliente$: Observable<Cliente> = new Observable<Cliente>();

  // Contenedor para todos los clientes, para la vista del administrador y del empleado
  clientes$: Observable<Cliente[]> = new Observable<Cliente[]>();

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

  // Modelo de la tabla de clientes
  displayedColumns: string[] = ['email', 'nombre', 'nif', 'telefono', 'iban'];

  // Flags para las acciones secundarias
  isClienteEdit: boolean = false;


  constructor(private localStorage: ReduxService,
    private apiUsuarios: UserService) { }

  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();

    // Listamos todos los clientes para el administrador y el empleado
    this.clientes$ = this.getClientes();
    this.idUsuario = '6429a7d7e05768574498e20e';
    // Obtenemos el cliente para la vista de cliente
    this.cliente$ = this.getCliente();

    // Obtenemos las clases a las que está apuntado el cliente

    // Obtenemos los horarios reservados del cliente

    // Obtenemos la lista de espera del cliente

    // Cargamos los datos de la card
    this.updateCard();

    // Cargamos el modelo para el formulario
    this.updateForm();


  }

  // Obtenemos el estado
  getLocalStore = (): void => {
    this.tipoUsuario = this.localStorage.getTipoUsuario();
    this.idUsuario = this.localStorage.getIdUsuario();
  }

  // Todos los usuario
  getClientes = (): Observable<Cliente[]> => this.apiUsuarios.getClientes().pipe(map(clientes => clientes));

  // Obtenemos el cliente
  getCliente = (): Observable<Cliente> => this.apiUsuarios.getCliente(this.idUsuario).pipe(map(cliente => cliente));

  // Actualizar los datos de la card
  updateCard = (): void => {
    // Actualizmos los datos de la card desde el observable
    this.cliente$.subscribe(cliente => {
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
    });
  }

  // Actualizar los datos del formulario
  updateForm = (): void => {
    // Actualizamos el modelo del formulario
    this.cliente$.subscribe(cliente => {
      this.clienteModel = cliente;
    });
  }

  // Enviamos el formulario al servicio
  onSubmit = (): void => { }

  // Editar el cliente

  // Filtro para la tabla de clientes
  applyFilter = (event: Event) => {
    const filterValue = (event.target as HTMLInputElement).value;
    // Filtramos la lista de clientes
    this.clientes$.pipe(map(clientes => clientes.filter(cliente => cliente.email.toLowerCase().includes(filterValue))));
  }
}
