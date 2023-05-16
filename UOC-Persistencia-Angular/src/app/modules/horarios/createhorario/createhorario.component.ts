import { Component } from '@angular/core';
import { Horario } from 'src/app/models/interfaces/horario.model';
import { Clase } from 'src/app/models/interfaces/clase.model';
import { HorariosService } from 'src/app/services/horarios.service';
import { ClasesService } from 'src/app/services/clases.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-createhorario',
  templateUrl: './createhorario.component.html',
  styleUrls: ['./createhorario.component.css']
})
export class CreatehorarioComponent {

  // Creamos los contenedores para trabajar con la clase
  clases$: Subscription = new Subscription();
  clases: Clase[] = [];
  clase?: Clase;

  // Insertaremos los nombres de las clases para crear un array de opciones para encontrar la clase
  nombresClases: string[] = [];

  subscripts: Array<Subscription> = [];

  // Creamos los días para el select y el día seleccionado
  dias: Array<string> = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];
  selectedDia!: number;

  // Modelo de horario para el formulario
  horarioModel: Horario = {
    dia: 0,
    hora: '',
    clase: {
      nombre: '',
      descripcion: '',
      profesor: '',
      duracion: 0,
      plazas: 0
    },
  }

  // Valor de retorno para la función de creación
  isCreated: boolean = false;
  isError: boolean = false;

  constructor(private apiHorario: HorariosService,
    private apiClase: ClasesService) {
    // Obtenemos todas las clases
    this.subscripts.push(
      this.clases$ = this.apiClase.getClases().subscribe(
        (clases) => {
          // Guardamos las clases obtenidas
          this.clases = clases;

          // Generamos un array con los nombres de las clases
          this.nombresClases = this.clases.map(clase => clase.nombre);
        }
      )
    );
  }

  createHorario = async () => {
    // Generamos el elemento a crear.
    // Pasamos el dia de string a number => Como estamos trabajando con un select, el valor es un string
    this.horarioModel.dia = parseInt(this.horarioModel.dia.toString());
    const toSave: Horario = this.horarioModel;
    toSave.id = "";

    // Obtenemos el id de la clase
    let claseId = toSave.clase.id as string;

    // Llamamos a la función de creación del servicio
    // Hay que enviarle el id de la clase
    try {
      this.apiHorario.createHorario(claseId, toSave);
      this.isCreated = true;
    } catch (error) {
      console.log(error);
      this.isError = true;
    }
  }

  // Buscamos la clase por su nombre
  onSearch = (event: Event) => {

    // Obtenemos el valor del input
    let nombre = (event.target as HTMLInputElement).value;

    // Filtramos el array de clases por el nombre y lo asignamos a clase
    this.clases.filter(clase => clase.nombre.toLowerCase().match(nombre.toLowerCase())).map((clase) => {
      this.clase = clase;
    });

    // Asignamos la clase al modelo
    if (this.clase) {
      // Si la clase existe, la asignamos al modelo
      this.horarioModel.clase = this.clase;
    }
  }

  // Repetimos la lógica para una busqueda al seleccionar una opción de la lista
  onSelect = (nombre: string) => {
    // Filtramos el array de clases por el nombre y lo asignamos a clase
    this.clases.filter(clase => clase.nombre.toLowerCase().match(nombre.toLowerCase())).map((clase) => {
      this.clase = clase;
    });

    // Asignamos la clase al modelo
    if (this.clase) {
      // Si la clase existe, la asignamos al modelo
      this.horarioModel.clase = this.clase;
    }
  }

  // Acción para la selección del día
  onDiaSelect = (dia: number) => {
    // Asignamos el día al modelo
    this.horarioModel.dia = dia;
  }

  // Reseteamos el formulario
  resetForm = () => {
    this.horarioModel = {
      dia: 0,
      hora: '',
      clase: {
        nombre: '',
        descripcion: '',
        profesor: '',
        duracion: 0,
        plazas: 0
      },
    }
  }
}
