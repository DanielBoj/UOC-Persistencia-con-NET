import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { User } from '../../models/interfaces/user.model';
import { ReduxService } from '../../services/redux.service';
import { Login } from 'src/app/models/interfaces/login.model';

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

  // Contenedor del usuario
  user: User | undefined;

  // Validación de email
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  passFormControl = new FormControl('', [Validators.required, Validators.minLength(6)]);

  constructor(private router: Router,
    private redux: ReduxService) { }

  ngOnInit(): void { }

  getErrorMessage = (): string => this.emailFormControl.hasError('required') ? 'Debes ingresar un valor' : this.emailFormControl.hasError('email') ? 'No es un email válido' : this.passFormControl.hasError('required') ? 'Debes ingresar un valor' : this.passFormControl.hasError('minLenght') ? 'No es un password válido' : '';

  // Conseguimos el usuario
  getUsuario = (): User | undefined => {
    // Llamada a la API

    // Devolvemos un usuario de prueba
    return {
      id: '1',
      email: 'johndoe@algo.com',
      pass: '123456',
      tipo: 'admin'
    }
  }

  checkUser = (): void => {
    // Conseguimos el usuario
    if (this.user = this.getUsuario()) {

      // Guardamos el tipo de usuario
      this.redux.setTipoUsuario(this.user.tipo);
      // Guardamos el id de usuario
      if (this.user.id) this.redux.setIdUsuario(this.user.id);

      // Redireccionamos al usuario a la página de inicio
      this.redireccionar();
    }
  }

  // Redireccionamos al usuario
  redireccionar = (): void => {
    this.router.navigate(['/menu']);
  }
}
