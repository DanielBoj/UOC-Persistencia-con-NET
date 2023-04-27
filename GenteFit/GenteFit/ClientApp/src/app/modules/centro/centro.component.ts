import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Centro } from 'src/app/models/interfaces/centro.model';
import { CentroService } from 'src/app/services/centro.service';
import { ReduxService } from 'src/app/services/redux.service';

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
  centro$: Subscription = new Subscription();
  centro: Centro = {} as Centro;
  subscripts: Array<Subscription> = [];

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

  constructor(private localStorage: ReduxService,
    private api: CentroService) {
    // Obtenemos los datos del centro de la API
    this.subscripts.push(
      this.centro$ = this.api.getCentro().subscribe((centro) => {
        // Comumimos la API => Obtenemos el centro
        this.centro = centro;

        // Actualizamos los datos de la card
        this.updateCard(centro);

        // Montamos el modelo para el formulario
        this.updateForm(centro);
      })
    );
  }

  ngOnInit(): void {
    // Obtenemos el estado
    this.getLocalStore();

    // Obtenemos la información del centro
    //this.getCentro();

    // Actualizamos los datos de la card
    //this.updateCard();

    // Montamos el modelo para el formulario
    //this.updateForm();

    this.tipoUsuario = "admin"
  }

  getLocalStore = (): void => {
    this.tipoUsuario = this.localStorage.getTipoUsuario();
    this.idUsuario = this.localStorage.getIdUsuario();
  }

  //getCentro = () => this.api.getCentro().pipe(centro => this.centro$ = centro);

  // Enviamos el formulario de edición
  editCentro = (): void => {
    // Actualizamos el centro
    let updatedCentro: Centro = this.centroModel;
    updatedCentro.id = '';

    // Enviar el centro a la API

    // Actualizamos los datos de la card
    //this.updateCard(this.centro);

    // Reseteamos y ocultamos el formulario
    this.resetCentro();
    this.showEdit = false;

    // Mostramos el mensaje de éxito
  }

  // Actualizar los datos de la card
  updateCard = (centro: Centro): void => {
    // Actualizamos los datos de la card
    this.centroData = [
      { name: 'Nombre', value: centro.nombre },
      { name: 'Descripción', value: centro.descripcion },
      { name: 'Dirección', value: centro.direccion.domicilio },
      { name: 'Población', value: centro.direccion.poblacion },
      { name: 'Código postal', value: centro.direccion.cp },
      { name: 'País', value: centro.direccion.pais },
      { name: 'Teléfono', value: centro.telefono },
      { name: 'Email', value: centro.email }
    ];
  }


  updateForm = (centro: Centro): void => {
    // Actualizamos los datos del formulario
    this.centroModel = centro;

  }

  // Reseteamos el formulario
  resetCentro = (): void => {
    this.updateForm(this.centro);
  }

  // Validador del email
  validateEmail = (): boolean => {
    const re = /\S+@\S+\.\S+/;
    return re.test(this.centroModel.email);
  }

  // Constructor del mensaje de error
  getErrorMessage = (): string => this.validateEmail() ? '' : 'Email no válido';
}

