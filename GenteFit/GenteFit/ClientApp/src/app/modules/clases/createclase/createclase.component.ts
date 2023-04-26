import { Component } from '@angular/core';
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
    private router: Router) { }

  createClase = async () => {
    const toSave: Clase = this.claseModel;
    toSave.id = "";

    try {
      this.api.createClase(toSave);
    } catch (error) {
      console.log(error);
    }

    this.router.navigate(['/clases']);
  }
}
