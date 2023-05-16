import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PedidosodooComponent } from './pedidosodoo.component';

describe('PedidosodooComponent', () => {
  let component: PedidosodooComponent;
  let fixture: ComponentFixture<PedidosodooComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PedidosodooComponent]
    });
    fixture = TestBed.createComponent(PedidosodooComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
