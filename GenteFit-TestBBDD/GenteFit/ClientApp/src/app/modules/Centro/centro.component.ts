import { Component, OnInit } from '@angular/core';
import { Centro } from 'src/app/models/interfaces/centro.model';

@Component({
  selector: 'app-centro',
  templateUrl: './centro.component.html',
  styleUrls: ['./centro.component.css']
})
export class CentroComponent implements OnInit {

  // Variables para el tipo y el ID del usuario
  tipoUsuario: string | null = '';
  idUsuario: string | null = '';

  // Variables para el título y el subtítulo
  title = "GentFit";
  subtitle = "Bienvenido a GentFit";

  // Objeto del centro y modelo del objeto
  centro: Centro = {
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
  centroModel = { ...this.centro };

  // Array para almacenar los datos del centro que se mostrarán en la vista
  centroData: any[] = [];

  // Variables para controlar la visualización del formulario de edición
  showEdit = false;
  showEdited = false;

  constructor() { }

  ngOnInit(): void {

    // Obtiene los datos del usuario de la localStorage
    this.getLocalStore();

    // Obtiene el objeto del centro
    this.centro = this.getCentro();

    // Actualiza los datos que se mostrarán en la vista
    this.updateCard();

    // Asigna el tipo de usuario "admin"
    this.tipoUsuario = "admin";
  }

  // Método para obtener los datos del usuario desde la localStorage
  getLocalStore(): void {
    console.log(this.tipoUsuario);
    console.log(this.idUsuario);
  }

  // Método para obtener el objeto del centro
  getCentro(): Centro {
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
    };
  }

  // Método para editar el objeto del centro
  editCentro(): void {

    // Asigna los nuevos valores al objeto del centro
    this.centro = this.centroModel;

    // Actualiza los datos que se mostrarán en la vista
    this.updateCard();

    // Reinicia el modelo del centro y oculta el formulario de edición
    this.resetCentro();
    this.showEdit = false;

    // Muestra un mensaje de confirmación de edición
    this.showEdited = true;
  }

  // Método para actualizar los datos que se mostrarán en la vista
  updateCard(): void {
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

  resetCentro(): void {
    this.centroModel = this.centro;
  }

  //Validar correo

  validateEmail = (): boolean => {
    const re = /\S+@\S+\.\S+/;
    return re.test(this.centroModel.email);
  }


  getErrorMessage = (): string => this.validateEmail() ? '' : 'Email no válido';
}
