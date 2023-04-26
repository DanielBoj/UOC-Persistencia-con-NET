import { Component, OnInit } from '@angular/core';
import { Horario } from 'src/app/models/interfaces/horario.model';
import { HorariosService } from 'src/app/services/horarios.service';
import { DiasMap } from 'src/app/models/interfaces/dias.map';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-horarios',
  templateUrl: './horarios.component.html',
  styleUrls: ['./horarios.component.css']
})
export class HorariosComponent implements OnInit {

  // Contenedor para los horarios
  horarios$: Observable<Horario[]> = new Observable((observer) => {
    observer.next(this.getHorarios());
  });

  constructor(private apiHorario: HorariosService) { }

  ngOnInit(): void {

    // Nos subscribimos al observable para obtener los datos
    this.horarios$.subscribe((val) => {
      console.log(val);
    });
  }

  getHorarios = (): Horario[] => {
    let horarios: Horario[] = [];

    this.apiHorario.getHorarios().subscribe((data) => {
      horarios = data;
    });

    return horarios;
  }
}
