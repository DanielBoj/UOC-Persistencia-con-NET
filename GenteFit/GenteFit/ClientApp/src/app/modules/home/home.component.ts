import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { User, UserResponse } from '../../models/interfaces/user.model';
import { ReduxService } from '../../services/redux.service';
import { Login } from 'src/app/models/interfaces/login.model';
import { Observable, map, of } from 'rxjs';
import { UserService } from 'src/app/services/user.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  title: string = "Gente Fit";
  subtitle: string = "Identificate como un Gente Fit";
  description: string = "Gente Fit es una plataforma que te permite encontrar personas que comparten tus mismos intereses en el mundo del deporte y la actividad física. ¡Registrate y encuentra a tu gente fit!";

  // Modelo form
  log: Login = {
    email: '',
    pass: ''
  };

  // Contenedor del usuario solicitado a la API
  user$: Observable<User> = new Observable<User>();
  users$: Observable<User[]> = new Observable<User[]>();

  // Validación de email
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  passFormControl = new FormControl('', [Validators.required, Validators.minLength(6)]);

  id?: string;

  constructor(private router: Router,
    private redux: ReduxService,
    private api: UserService) { }

  ngOnInit(): void {
    this.users$ = this.getUsers();
  }

  getErrorMessage = (): string => this.emailFormControl.hasError('required') ? 'Debes ingresar un valor' : this.emailFormControl.hasError('email') ? 'No es un email válido' : this.passFormControl.hasError('required') ? 'Debes ingresar un valor' : this.passFormControl.hasError('minLenght') ? 'No es un password válido' : '';

  // Conseguimos el usuario
  getUsers = (): Observable<User[]> => this.api.getUsers().pipe(map(users => users));


  // Conseguimos el usuario

  checkUser = (): void => {
    // Comprobamos que exista un usuario con el email y la contraseña introducidos
    this.user$ = this.users$.pipe(map(users => users.find(user => user.email === this.log.email && user.pass === this.log.pass))) as Observable<User>;

    if (this.user$ == null) {
      alert('Usuario no encontrado');
      return;
    };

    // Casteamos de Observable a User
    let userFound: User;
    this.user$.subscribe(user => {
      if (user) {
        this.redux.setIdUsuario(user.id || '');
        this.redux.setTipoUsuario(user.tipo);
      }
    });

    this.id = this.redux.getIdUsuario() || '';
  }


  // Redireccionamos al usuario
  redireccionar = (): void => {
    this.router.navigate(['/menu']);
  }
}
