import { Component, OnInit, OnDestroy, ViewChild, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { User } from '../../models/interfaces/user.model';
import { Cache } from '../../models/interfaces/cache.model';
import { ReduxService } from '../../services/redux.service';
import { Login } from 'src/app/models/interfaces/login.model';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DisplayService } from 'src/app/services/display.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  // Información para la card
  title: string = "Gente Fit";
  subtitle: string = "Identificate como un Gente Fit";
  description: string = "Gente Fit es la plataforma de gestión de los centros de fitness Gente Fit. ¡Accede a tu nuevo gimnasio!";

  // Modelo form
  log: Login = {
    email: '',
    pass: ''
  };

  // Contenedor del usuario solicitado a la API
  users$: Subscription = new Subscription();
  users: User[] = [];
  user?: User;

  // Contenedor para manejar el cache
  cache$: Subscription = new Subscription();
  cache: Cache = {
    tipoUsuario: '',
    idUsuario: ''
  };

  subscripts: Subscription[] = [];

  // Flag para el control de la visibilidad de la contraseña
  hide: boolean;

  // Flag para controlar si existe el usuario
  userFound: boolean = false;

  // Flag para controlar la barra de navegación
  showHeader: boolean = false;

  // Flag para el control de carga de datos
  isLoading: boolean = false;

  // Controles del formulario
  @ViewChild('loginForm') loginForm: NgForm = new NgForm([], []);

  // El constructor se ejecuta antes que el ngOnInit, además, lo usamos para inyectar servicios
  constructor(private router: Router,
    private redux: ReduxService,
    private api: UserService,
    private snackBar: MatSnackBar,
    private displayService: DisplayService) {
    // Seteamos el pass como oculto
    this.hide = true;
  }

  ngOnInit(): void {
    this.hide = true;
    // Ocultamos la navegación
    this.displayService.setShowHeader(false);

    // Mostramos un spinner mientras cargan los datos
    this.isLoading = true;

    // Obtenemos los usuarios de la API
    this.subscripts.push(this.users$ = this.api.getUsers().subscribe(
      (users) => {
        // Cargamos los datos desde el observable recibido de la API
        this.users = users;

        // Ocultamos el spinner
        this.isLoading = false;
      }
    ));

  }

  ngOnDestroy(): void {
    // Nos desuscribimos de todos los observables
    this.subscripts.forEach(sub => sub.unsubscribe());

    // Volvemos a mostrar la navegación
    this.displayService.setShowHeader(true);
  }

  checkUser = (): void => {

    // Comprobamos que exista un usuario con el email y la contraseñá introducidos
    this.user = this.users.find(user => user.email === this.log.email && user.pass === this.log.pass);

    // Comprobamos si el login es correcto y mostramos un aviso por pantalla con Snackbar
    if (!this.user) {
      this.snackBar.open('Usuario no encontrado, verifica las credenciales.', 'Cerrar', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
        panelClass: ['mat-snackbar-error']
      });

      // Salimos de la función
      return;
    }

    // Cargamos el id y el tipo de usuario en el Redux
    this.cache.idUsuario = this.user.id as string;
    this.cache.tipoUsuario = this.user.tipo;
    this.sendCache();

    // Mostrar un aviso por pantalla con Snackbar y redireccionar al usuario
    this.openValidSnackBar();
  }

  sendCache = (): void => this.redux.setCache(this.cache);

  // Creamos un método para mostrar el snackbar de éxito
  openValidSnackBar = (): void => {
    // Creamos un modelo de referencia para nuestro snackbar
    const snackBarRef = this.snackBar.open('Bienvenido a GentFit', 'Entrar', {
      // Orientamos el modelo en la pantalla
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['mat-snackbar-success'],
    });

    // Generamos las acciones del snackbar
    snackBarRef.onAction().subscribe(
      () => {
        // Redireccionamos al usuario
        this.router.navigate(['/menu']);
      }
    );
  }
}
