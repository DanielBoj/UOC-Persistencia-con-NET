import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {

  // Texto del footer
  licencia: string = 'CC Zero';
  autor: string = 'Hyperion Development';
  fecha: string = 'since 2023';
}
