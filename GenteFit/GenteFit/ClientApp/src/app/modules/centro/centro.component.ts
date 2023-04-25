import { Component, OnInit } from '@angular/core';
import { Centro } from 'src/app/models/interfaces/centro.model';

@Component({
  selector: 'app-centro',
  templateUrl: './centro.component.html',
  styleUrls: ['./centro.component.css']
})
export class CentroComponent implements OnInit {

  // Estado obtenido del servicio redux
  tipoUsuario: string | null = '';
  idUsuario: string | null = '';

  // Información para la card
  title: string = "GentFit";
  subtitle: string = "Bienvenido a GentFit";

  // Contenedor para la información del centro
  centro: Centro | any;;

  // Información para la card
  centroData: any[] = [];

  // Flags para el display de elementos
  showEdit: boolean = false;
  showEdited: boolean = false;

  // Modelo de formulario
  centroModel: Centro = {
    id: '',
    nombre: '',
    descripcion: '',
    direccion: {
      domicilio: '',
      poblacion: '',
      cp: 0,
      pais: ''
    },
    telefono: '',
    email: ''
  };

  constructor() { }

  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();

    // Obtenemos la información del centro
    this.centro = this.getCentro();

    // Montamos el modelo para el formulario
    this.centroModel = this.centro;

    // Actualizamos los datos de la card
    this.updateCard();

    this.tipoUsuario = "admin"
  }

  getLocalStore = (): void => {
    console.log(this.tipoUsuario);
    console.log(this.idUsuario);
  }

  getCentro = (): Centro => {
    return {
      id: '1',
      nombre: 'Centro de prueba',
      descripcion: 'Este es un centro de prueba',
      direccion: {
        domicilio: 'Calle falsa 123',
        poblacion: 'Madrid',
        cp: 28001,
        pais: 'España'
      },
      telefono: '123456789',
      email: 'info@gentefit.com'
    }
  }

  // Enviamos el formulario de edición
  editCentro = (): void => {
    // Actualizamos el centro
    this.centro = this.centroModel;

    // Enviar el centro a la API

    // Actualizamos los datos de la card
    this.updateCard();

    // Reseteamos y ocultamos el formulario
    this.resetCentro();
    this.showEdit = false;

    // Mostramos el mensaje de edición
    this.showEdited = true;
  }

  // Actualizar los datos de la card
  updateCard = (): void => {
    this.centroData = [
      { name: 'Nombre', value: this.centro.nombre },
      { name: 'Descripción', value: this.centro.descripcion },
      { name: 'Dirección', value: this.centro.direccion.domicilio },
      { name: 'Población', value: this.centro.direccion.poblacion },
      { name: 'Código postal', value: this.centro.direccion.cp },
      { name: 'País', value: this.centro.direccion.pais },
      { name: 'Teléfono', value: this.centro.telefono },
      { name: 'Email', value: this.centro.email }
    ];
  }

  // Reseteamos el formulario
  resetCentro = (): void => {
    this.centroModel = this.centro;
  }

  // Validador del email
  validateEmail = (): boolean => {
    const re = /\S+@\S+\.\S+/;
    return re.test(this.centroModel.email);
  }

  // Constructor del mensaje de error
  getErrorMessage = (): string => this.validateEmail() ? '' : 'Email no válido';
}
