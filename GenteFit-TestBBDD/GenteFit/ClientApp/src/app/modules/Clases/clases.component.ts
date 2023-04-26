import { Component, OnInit } from '@angular/core';
import { Clase } from 'src/app/models/interfaces/clase.model';

@Component({
  selector: 'app-centro',
  templateUrl: './clases.component.html',
  styleUrls: ['./clases.component.css']
})
export class ClasesComponent implements OnInit {

  // Estado obtenido del servicio redux
  tipoUsuario: string | null = '';
  idUsuario: string | null = '';

  // Información para la card
  title: string = "GentFit";
  subtitle: string = "Bienvenido a GentFit";

  // Contenedor para la información del centro
  clase: Clase | any;

  // Información para la card
  claseData: { name: string, value: string }[] = [];

  // Flags para el display de elementos
  showEdit: boolean = false;
  showEdited: boolean = false;

  // Modelo de formulario
  claseModel: Clase = {
    id: '',
    nombre: '',
    descripcion: '',
    profesor: '',
    duracion: '',
    plazas: ''
  };

  constructor() { }

  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();

    // Obtenemos la información del centro
    this.clase = this.getClase();

    // Montamos el modelo para el formulario
    this.claseModel = this.clase;

    // Actualizamos los datos de la card
    this.updateCard();

    this.tipoUsuario = "cliente"
  }

  getLocalStore(): void {
    console.log(this.tipoUsuario);
    console.log(this.idUsuario);
  }

  getClase(): Clase {
    return {
      id: '1',
      nombre: 'Clase',
      descripcion: 'Yoga',
      profesor: 'Iñaki',
      duracion: '60',
      plazas: '10'
    }
  }

  // Enviamos el formulario de edición
  editClase(): void {
    // Actualizamos el centro
    this.clase = this.claseModel;

    // Enviar el centro a la API

    // Actualizamos los datos de la card
    this.updateCard();

    // Reseteamos y ocultamos el formulario
    this.resetClase();
    this.showEdit = false;

    // Mostramos el mensaje de edición
    this.showEdited = true;
  }

  // Actualizar los datos de la card
  updateCard(): void {
    this.claseData = [
      { name: 'Nombre', value: this.clase.nombre },
      { name: 'Descripción', value: this.clase.descripcion },
      { name: 'Profesor', value: this.clase.profesor },
      { name: 'Duración', value: this.clase.duracion },
      { name: 'Plazas', value: this.clase.plazas }
    ];
  }

  // Reseteamos el formulario
  resetClase(): void {
    this.claseModel = this.clase;
  }
}


