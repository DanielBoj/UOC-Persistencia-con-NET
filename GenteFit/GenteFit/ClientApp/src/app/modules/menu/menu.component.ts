import { Component, ElementRef, EventEmitter, OnInit, Output, Renderer2 } from '@angular/core';
import { Subscription } from 'rxjs';
import { ReduxService } from 'src/app/services/redux.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  showHeader: boolean = true;

  cache$: Subscription = new Subscription();
  cache!: Cache;

  constructor(private redux: ReduxService) { }

  ngOnInit(): void {
    this.cache$ = this.redux.getCache().subscribe((data: any) => {
      this.cache = data;
    });
  }
}
