import { Component, OnInit } from '@angular/core';
import { Clase } from 'src/app/models/interfaces/clase.model';

@Component({
  selector: 'app-clases',
  templateUrl: './clases.component.html',
  styleUrls: ['./clases.component.css']
})
export class ClasesComponent implements OnInit {
  // Estado obtenido del servicio
  tipoUsuario: string | null = '';
  idUsuario: string | null = '';

  // Información para la card
  title: string = "Nuestras Clases";
  subtitle: string = "Eliga la suya";

  // Contenedor para la información del centro
  clase: Clase | any;;

  // Información para la card
  claseData:  any[] = [];

  // Flags para el display de elementos
  showEdit: boolean = false;
  showEdited: boolean = false;

  //Contenedor para la clase
  claseModel: Clase = {
    nombre: '',
    descripcion: '',
    profesor: '',
    duracion: 0,
    plazas: 0
  }

  //Constructor
  constructor() { }

  // Método de ciclo de vida OnInit
  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();

    // Obtenemos la información del cliente
   /* this.clase = this.getClase();*/

    // Montamos el modelo para la formulario
    this.claseData = this.getClaseData();

    // Actualizamos los datos de la card
    this.updateCard();

    this.tipoUsuario = "admin"
  }
  // Obtiene el estado del servicio
  getLocalStore = (): void => {
    console.log(this.tipoUsuario);
    console.log(this.idUsuario);
  }

  //Obtiene la información de la clase
  /*getClase = (): Clase => {
    return {
      nombre: 'Zumba',
      descripcion: 'Baile perfecto para los abejorros',
      profesor: 'ms Bee',
      duracion: 3,
      plazas: 2 
    }
  }*/

  getClaseData = (): any[] => {
    return [
      { name: 'Nombre', value: this.clase.nombre },
      { name: 'Descripción', value: this.clase.descripcion },
      { name: 'Profesor', value: this.clase.profesor },
      { name: 'Duración', value: this.clase.duracion },
      { name: 'Plazas', value: this.clase.plazas },
    ];
  }

  clasesDisponibles: Clase[] = [
    { nombre: 'Zumba', descripcion: 'Baile perfecto para los abejorros', profesor: 'ms Bee', duracion: 1, plazas: 20 },
    { nombre: 'Yoga', descripcion: 'Vuelvete mas elastico que el yogurt', profesor: 'danone', duracion: 1, plazas: 15 },
    { nombre: 'Pilates', descripcion: 'Soporta el mundo a tus espaldas', profesor: 'Atlas', duracion: 1, plazas: 10 },
    // ...
  ];
  onClaseSelected(clase: Clase): void {
    // Actualiza la variable clase con la clase seleccionada
    this.clase = clase;

    // Actualiza la tarjeta con la nueva información
    this.updateCard();
  }

  editClase = (): void => {
    //Actualizamos la clase
    this.clase = this.claseModel;

    //Enviamos la clase a la API

    // Actualizamos los datos de la card
    this.updateCard()

    // Reseteamos y ocultamos el formulario
    this.resetClase();
    this.showEdit = false;

    // Mostramos el mensaje de edición
    this.showEdited = true;
  }

  //Actualizar los datos de la card
  updateCard = (): void => {
    this.claseData = this.getClaseData();
  }

  resetClase = (): void => {
    this.claseModel = this.clase
  }

}
