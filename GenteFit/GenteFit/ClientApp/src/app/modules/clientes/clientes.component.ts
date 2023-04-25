import { Component, OnInit } from '@angular/core';
import { Cliente } from 'src/app/models/interfaces/cliente.model';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})

export class ClientesComponent implements OnInit {

  // Estado obtenido del servicio
  tipoUsuario: string | null = '';
  idUsuario: string | null = '';

  // Información para la card
  title: string = "GentFit";
  subtitle: string = "Bienvenido a GentFit";

  // Contenedor para la información del centro
  cliente: Cliente | any;;

  // Información para la card
  clienteData: any[] = [];

  // Flags para el display de elementos
  showEdit: boolean = false;
  showEdited: boolean = false;
  showEdit2: boolean = false;

  // Contenedor para la información del cliente
  clienteModel: Cliente = {
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
    email: '',
    pass: '',
    tipo: ''
  };

  // Constructor
  constructor() { }

  // Método de ciclo de vida OnInit
  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();

    // Obtenemos la información del cliente
    this.cliente = this.getCliente();

    // Montamos el modelo para la formulario
    this.clienteData = this.getClienteData();

    // Actualizamos los datos de la card
    this.updateCard();

    this.tipoUsuario = "admin"
  }

  // Obtiene el estado del servicio
  getLocalStore = (): void => {
    console.log(this.tipoUsuario);
    console.log(this.idUsuario);
  }

  // Obtiene la información del cliente
  getCliente = (): Cliente => {
    return {
      nombre: 'Juan',
      nif: '12345678A',
      direccion: {
        domicilio: 'Calle Falsa',
        poblacion: 'Madrid',
        cp: 28000,
        pais: 'España'
      },
      telefono: '123456789',
      genero: 0,
      iban: 'ES0123456789012345678901',
      email: 'juan@gmail.com',
      pass: '',
      tipo: ''
    };
  };

  // Obtiene la información del cliente para la card
  getClienteData = (): any[] => {
    return [
      { name: 'Nombre', value: this.cliente.nombre },
      { name: 'Nif', value: this.cliente.nif },
      { name: 'Dirección', value: this.cliente.direccion.domicilio },
      { name: 'Ciudad', value: this.cliente.direccion.poblacion },
      { name: 'Código Postal', value: this.cliente.direccion.cp },
      { name: 'País', value: this.cliente.direccion.pais },
      { name: 'Telefono', value: this.cliente.telefono },
      { name: 'Genero', value: this.cliente.genero },
      { name: 'IBAN', value: this.cliente.iban },
    ];
  }

  // Enviamos el formulario de edición
  editCliente = (): void => {
    // Actualizamos el cliente
    this.cliente = this.clienteModel;

    // Enviar el cliente a la API

    // Actualizamos los datos de la card
    this.updateCard();

    // Reseteamos y ocultamos el formulario
    this.resetCliente();
    this.showEdit = false;

    // Mostramos el mensaje de edición
    this.showEdited = true;
  }

  // Actualizar los datos de la card
  updateCard = (): void => {
    this.clienteData = this.getClienteData();
  }

  // Reseteamos el formulario
  resetCliente = (): void => {
    this.clienteModel = this.cliente;
  }

  // Validador del email
  validateEmail = (): boolean => {
    const re = /\S+@\S+\.\S+/;
    return re.test(this.clienteModel.email);
  }

  // Constructor del mensaje de error
  getErrorMessage = (): string => this.validateEmail() ? '' : 'Email no válido';
}
