import { Component, OnInit } from '@angular/core';
import { Horario } from 'src/app/models/interfaces/horario.model';

@Component({
  selector: 'app-horarios',
  templateUrl: './horarios.component.html',
  styleUrls: ['./horarios.component.css']
})
export class HorariosComponent implements OnInit {
  // Estado obtenido del servicio
  tipoUsuario: string | null = '';
  idUsuario: string | null = '';

  // Información para la card
  title: string = "Horarios";
  subtitle: string = "";

  // Contenedor para la información del centro
  horario: Horario | any;;

  // Información para la card
  horarioData: any[] = [];

  // Flags para el display de elementos
  showEdit: boolean = false;
  showEdited: boolean = false;

  //Contenedor para el horario
  horarioModel: Horario = {
    dia: 0,
    hora: '00:00:00',
    clase: {
      nombre: '',
      descripcion: '',
      profesor: '',
      duracion: 0,
      plazas: 0
    },
    reservas: [],
    esperas: []
  }

  //Constructor
  constructor() { }

  // Método de ciclo de vida OnInit
  ngOnInit(): void {
    this.tipoUsuario = 'admin'
    // Obtenemos el estado
    this.getLocalStore();

    // Montamos el modelo para la formulario
    this.horarioData = this.getHorarioData();
   
    // Actualizamos los datos de la card
    this.updateCard();
    
  }

  getLocalStore = (): void => {
    console.log(this.tipoUsuario);
    console.log(this.idUsuario);
  }

  getHorarioData = (): any[] => {
    return [
      { name: 'Día', value: this.horario.dia },
      { name: 'Hora', value: this.horario.hora },
      { name: 'Nombre', value: this.horario.clase.nombre },
      { name: 'Descripción', value: this.horario.clase.descripcion },
      { name: 'Profesor', value: this.horario.clase.profesor },
      { name: 'Duración', value: this.horario.clase.duracion },
      { name: 'Plazas', value: this.horario.clase.plazas },
    ];
  }
  horarios: Horario[] = [
    {
      dia: 1,
      hora: '08:00:00',
      clase: {
        nombre: 'Yoga',
        descripcion: 'Vuelvete mas elastico que el yogurt',
        profesor: 'danone',
        duracion: 1,
        plazas: 15
      },
      reservas: [],
      esperas: []
    },
    {
      dia: 1,
      hora: '09:00:00',
      clase: {
        nombre: 'Pilates',
        descripcion: 'Soporta el mundo a tus espaldas',
        profesor: 'Atlas',
        duracion: 1,
        plazas: 10
      },
      reservas: [],
      esperas: []
    },
  ];

  onHorarioSelected(horario: Horario): void {
    // Actualiza la variable horario con el horario seleccionado
    this.horario = horario;

    // Actualiza la tarjeta con la nueva información
    this.updateCard();
  }

  editHorario = (): void => {
    //Actualizamos el horario
    this.horario = this.horarioModel;

    //Enviamos el horario a la API
    // Actualizamos los datos de la card
    this.updateCard()

    // Reseteamos y ocultamos el formulario
    this.resetHorario();
    this.showEdit = false;

    // Mostramos el mensaje de edición
    this.showEdited = true;
  }
  updateCard = (): void => {
    this.horarioData = this.getHorarioData();
  }

  resetHorario = (): void => {
    this.horarioModel = this.horario
  }
}
