import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { Centro } from 'src/app/models/interfaces/centro.model';
import { Cache } from 'src/app/models/interfaces/cache.model';
import { CentroService } from 'src/app/services/centro.service';
import { ReduxService } from 'src/app/services/redux.service';

@Component({
  selector: 'app-centro',
  templateUrl: './centro.component.html',
  styleUrls: ['./centro.component.css']
})
export class CentroComponent implements OnInit, OnDestroy {

  // Estado obtenido del servicio redux
  cache$: Subscription = new Subscription();
  cache!: Cache;

  tipoUsuario!: string;
  idUsuario!: string;

  // Información para la card
  title: string = "Bienvenido a GenteFit";
  subtitle: string = "Información del centro";

  // Contenedor para la información del centro
  centro$: Subscription = new Subscription();
  centro: Centro = {} as Centro;
  subscripts: Array<Subscription> = [];

  // Información para la card
  centroData: any[] = [];

  // Flags para el display de elementos
  showEdit: boolean = false;

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
    private api: CentroService,
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    // Obtenemos el estado de la aplicación
    this.subscripts.push(this.cache$ = this.localStorage.getCache().subscribe((cache) => {
      // Actualizamos el estado de la aplicación
      this.cache = cache;

      // Actualizamos los datos del usuario
      this.tipoUsuario = cache.tipoUsuario;
      this.idUsuario = cache.idUsuario;
    }));

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

  ngOnDestroy(): void {
    // Desuscribimos todos los observables
    this.subscripts.forEach(sub => sub.unsubscribe());
  }

  // Enviamos el formulario de edición
  editCentro = (): void => {
    // Actualizamos el centro, nos aseguramos de que el ID sea el correcto
    this.centroModel.id = this.centro.id;
    const toSave: Centro = this.centroModel;

    // Nos aseguramos de que el ID sea un string
    let id: string = this.centro.id as string;

    // Enviar el centro a la API
    try {
      this.api.editCentro(id, toSave);

    } catch (error) {
      this.snackBar.open('Imposible modificar el centro, verifica los datos.', 'Cerrar', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
        panelClass: ['mat-snackbar-error']
      });

      // Salimos de la función
      return;
    }

    // Actualizamos los datos de la card
    this.updateCard(this.centro);

    // Reseteamos y ocultamos el formulario
    this.resetCentro();
    this.showEdit = false;

    // Mostramos el mensaje de éxito
    this.snackBar.open('Centro modificado con éxito.', 'Cerrar', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['mat-snackbar-success']
    });

  }

  // Actualizar los datos de la card
  updateCard = (centro: Centro): void => {
    // Actualizamos los datos de la card
    this.centroData = [
      { name: 'Nombre', value: centro.nombre },
      { name: 'Descripción', value: centro.descripcion },
      { name: 'Dirección', value: centro.direccion.domicilio },
      { name: 'Población', value: centro.direccion.poblacion },
      { name: 'Código postal', value: centro.direccion.cp as number },
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

