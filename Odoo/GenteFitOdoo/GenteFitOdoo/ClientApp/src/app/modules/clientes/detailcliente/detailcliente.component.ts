import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { Cliente } from 'src/app/models/interfaces/cliente.model';
import { Genero } from 'src/app/models/genero';
import { ReduxService } from 'src/app/services/redux.service';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-detailcliente',
  templateUrl: './detailcliente.component.html',
  styleUrls: ['./detailcliente.component.css']
})
export class DetailclienteComponent {

  // Estado de la aplicación
  cache$: Subscription = new Subscription();
  cache!: any;
  idUsuario!: string;
  tipoUsuario!: string;

  // Datos obtenidos de la url
  id!: string;

  // Información para la cabecera de la página
  title: string = "GentFit";
  subtitle: string = "Información del cliente";

  // Contenedor para los datos del cliente obtenidos de la API
  cliente$: Subscription = new Subscription();
  cliente!: Cliente;
  nombreCliente!: string;

  subscripts: Array<Subscription> = [];

  // Modelo de cliente para la card de información del cliente
  clienteData: any = [
    { name: 'Nombre', value: '' },
    { name: 'Email', value: '' },
    { name: 'NIF', value: '' },
    { name: 'Dirección', value: '' },
    { name: 'Población', value: '' },
    { name: 'Código postal', value: '' },
    { name: 'País', value: '' },
    { name: 'Teléfono', value: '' },
    { name: 'Género', value: '' },
    { name: 'IBAN', value: '' },
  ]

  // URL anterior
  previousUrl!: string;

  constructor(private reduxService: ReduxService,
    private userService: UserService,
    private route: ActivatedRoute,
    private location: Location) {
    // Almacena la URL anterior en la variable previousUrl
    this.location.onUrlChange((url: string) => {
      this.previousUrl = url;
    });
  }

  ngOnInit(): void {
    // Primero obtenemos la ID de la url para poder buscar el cliente en la BD.
    // Lo obtenemos como una subscripción para que tenga un carácter asíncrono\
    this.subscripts.push(this.route.params.subscribe(
      (params) => this.id = params['id']
    ));

    // Obtenemos el estado de la aplicación
    this.subscripts.push(this.reduxService.getCache().subscribe(
      (state) => {
        this.cache = state;
        this.idUsuario = state.idUsuario;
        this.tipoUsuario = state.tipoUsuario;
      }));

    // Consumimos la API para obtener los datos del cliente
    this.subscripts.push(this.userService.getCliente(this.id).subscribe((cliente: Cliente) => {
      // Cargamos los datos del cliente
      this.cliente = cliente;
      this.nombreCliente = cliente.nombre;

      // Convertimos el género a travé del enum Genero
      this.cliente.genero = Genero[this.cliente.genero as keyof typeof Genero];

      // Actualizamos el modelo de la card de información del cliente
      this.updateCardData(cliente);
    }
    ));
  }

  ngOnDestroy(): void {
    // Dejamos de oir los eventos asíncronos al destruir el componente
    this.subscripts.forEach(sub => sub.unsubscribe());
  }

  updateCardData = (cliente: Cliente) => {
    this.clienteData = [
      { name: 'Nombre', value: cliente.nombre },
      { name: 'Email', value: cliente.email },
      { name: 'NIF', value: cliente.nif },
      { name: 'Dirección', value: cliente.direccion.domicilio },
      { name: 'Población', value: cliente.direccion.poblacion },
      { name: 'Código postal', value: cliente.direccion.cp },
      { name: 'País', value: cliente.direccion.pais },
      { name: 'Teléfono', value: cliente.telefono },
      { name: 'Género', value: cliente.genero },
      { name: 'IBAN', value: cliente.iban },
    ]
  }

  // Función para volver a la página anterior
  goBack = () => {
    this.location.back();
  }
}
