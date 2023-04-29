import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Clase } from 'src/app/models/interfaces/clase.model';
import { ClasesService } from 'src/app/services/clases.service';

@Component({
  selector: 'app-createclase',
  templateUrl: './createclase.component.html',
  styleUrls: ['./createclase.component.css']
})
export class CreateclaseComponent {

  // Modelo de clase para el formulario
  claseModel: Clase = {
    nombre: '',
    descripcion: '',
    profesor: '',
    duracion: 0,
    plazas: 0,
  };

  constructor(private api: ClasesService,
    private router: Router,
    private snackBar: MatSnackBar) { }

  createClase = async () => {
    const toSave: Clase = this.claseModel;
    toSave.id = "";

    try {
      // Creamos la clase
      this.api.createClase(toSave);
    } catch (error) {

      // Mostramos un mensaje de error
      this.snackBar.open('Error al crear la clase', 'Cerrar', {
        duration: 5000
      });

      // Salimos del método
      return;
    }

    // Mostramos un mensaje de éxito
    this.snackBar.open('Clase creada correctamente', 'Cerrar', {
      duration: 5000
    });

    // Redirigimos al listado de clases
    this.router.navigate(['/clases']);
  }
}
