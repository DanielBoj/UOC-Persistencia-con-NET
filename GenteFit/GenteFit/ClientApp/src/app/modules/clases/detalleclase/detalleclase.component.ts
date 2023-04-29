import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Clase } from 'src/app/models/interfaces/clase.model';
import { ClasesService } from 'src/app/services/clases.service';

@Component({
  selector: 'app-detalleclase',
  templateUrl: './detalleclase.component.html',
  styleUrls: ['./detalleclase.component.css']
})
export class DetalleclaseComponent implements OnInit, OnDestroy {

  // Id de la clase
  idClase!: string;

  // Contenedor para la clase
  clase$: Subscription = new Subscription();
  clase!: Clase;

  subscrips: Array<Subscription> = [];

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

    // Obtenemos la clase de la API
    this.subscrips.push(this.clase$ = this.api.getClase(this.idClase).subscribe(
      (clase) => {
        // Cargamos los datos de la clase
        this.clase = clase;

        // Actualizamos la card
        this.claseCard = clase;
      }
    ));
  }

  ngOnDestroy(): void {
    // Nos desuscribimos de las subscripciones
    this.subscrips.forEach(sub => sub.unsubscribe());
  }

  getIdClase = () => {
    this.route.params.subscribe(params => {
      this.idClase = params['id'];
    });
  }
}
