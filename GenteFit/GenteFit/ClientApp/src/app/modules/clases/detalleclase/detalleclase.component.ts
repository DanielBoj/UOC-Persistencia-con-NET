import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs';
import { Clase } from 'src/app/models/interfaces/clase.model';
import { ClasesService } from 'src/app/services/clases.service';

@Component({
  selector: 'app-detalleclase',
  templateUrl: './detalleclase.component.html',
  styleUrls: ['./detalleclase.component.css']
})
export class DetalleclaseComponent implements OnInit {

  // Id de la clase
  idClase!: string;

  // Contenedor para la clase
  clase$: Observable<Clase> = new Observable<Clase>();

  // Modelo para la card
  claseCard: Clase = {
    id: '',
    nombre: '',
    descripcion: '',
    profesor: '',
    duracion: 0,
    plazas: 0,
  };

  constructor(private route: ActivatedRoute,
    private api: ClasesService) { }

  ngOnInit(): void {
    // Obtenemos la id de la ruta
    this.getIdClase();

    // Obtenemos la clase
    this.getClase();

    // Actualizamos la card
    this.updateCard();

  }

  getClase = async () => {
    this.api.getClase(this.idClase).pipe(data => this.clase$ = data);
  }

  getIdClase = () => {
    this.route.params.subscribe(params => {
      this.idClase = params['id'];
    });
  }

  updateCard = () => {
    this.clase$.subscribe(data => this.claseCard = data);
  }
}
