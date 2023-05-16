import { Component, OnInit, OnDestroy } from '@angular/core';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { Genero } from 'src/app/models/genero';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { ReduxService } from 'src/app/services/redux.service';
import { UserService } from 'src/app/services/user.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-editcliente',
  templateUrl: './editcliente.component.html',
  styleUrls: ['./editcliente.component.css']
})
export class EditclienteComponent implements OnInit, OnDestroy {

  // Estado de la aplicación
  cache$: Subscription = new Subscription();
  cache!: Cache;
  idUsuario!: string;
  tipoUsuario!: string;

  // Contenedor para los datos del cliente
  cliente$: Subscription = new Subscription();
  cliente!: Cliente
  response!: any;

  subscripts: Array<Subscription> = [];

  // Información para la cabecera de la página
  title: string = "GentFit";
  subtitle: string = "Ponte en forma con gente como tú";
  cuentaTitle!: string;
  cuentaSubtitle: string = "Edita tus datos";

  // Modelo cliente para el formulario
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

  constructor(private redux: ReduxService,
    private api: UserService,
    private url: ActivatedRoute,
    private router: Router,
    private snackbar: MatSnackBar) { }

  ngOnInit(): void {
    // Obtenemos el estado de la aplicación para poder mostrar las opciones correspondientes
    this.subscripts.push(this.cache$ = this.redux.getCache().subscribe(
      (data) => {
        // Cacheamos los datos del estado
        this.cache = data;
        this.tipoUsuario = this.cache.tipoUsuario;
        this.idUsuario = this.cache.idUsuario;
      }
    ));

    // Obtenemos el id del cliente de la url
    this.subscripts.push(this.url.params.subscribe(
      (params) => {
        this.idUsuario = params.id;
      }
    ));

    // Obtenemos los datos del cliente
    this.subscripts.push(this.cliente$ = this.api.getCliente(this.idUsuario).subscribe(
      (data) => {
        // Consumimos los datos de la API
        this.cliente = data;
        // Asignamos el valor de género según el enum Genero
        this.cliente.genero = Genero[this.cliente.genero as keyof typeof Genero];
        // Asignamos los datos del cliente al modelo para el formulario
        this.clienteModel = this.cliente;

        // Asignamos el título de la cuenta
        this.cuentaTitle = this.cliente.nombre;
      }
    ));
  }

  ngOnDestroy(): void {
    // Desuscribimos los observables
    this.subscripts.forEach(sub => sub.unsubscribe());
  }

  // Actualizamos los datos del cliente
  onSubmit = async () => {
    // Pasamos el género del cliente a número y aseguramos el id
    this.clienteModel.genero = Genero[this.clienteModel.genero as keyof typeof Genero];
    let id = this.idUsuario;
    this.clienteModel.tipo = 'cliente';

    // Creamos el objeto
    const toSave: Cliente = this.clienteModel;

    // Editamos el cliente a través de la API
    try {
      this.api.editCliente(id, toSave);
    } catch (error) {
      // Mostramos un mensaje de error
      this.snackbar.open('Error al editar el cliente', 'Cerrar', {
        duration: 3000,
        // Arriba y centrado
        verticalPosition: 'top',
        horizontalPosition: 'center',
      });

      // Salimos del método
      return;
    }

    // Mostramos un mensaje de éxito
    this.snackbar.open('Cliente editado correctamente', 'Cerrar', {
      duration: 3000,
      // Arriba y centrado
      verticalPosition: 'top',
      horizontalPosition: 'center',
      // Declaramos una acción para el snackbar
    }).onAction().subscribe(() => {
      this.volver();
    });
  }

  // Si cancelamos, volvemos a la vista de clientes
  onCancel = () => this.router.navigate(['/clientes']);

  // Creamos una acción para el snackbar de confirmación que vuelva al componente clientes
  volver = () => this.router.navigate(['/clientes']);
}
