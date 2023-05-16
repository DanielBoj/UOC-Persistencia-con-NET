import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientesodooComponent } from './clientesodoo.component';

describe('ClientesodooComponent', () => {
  let component: ClientesodooComponent;
  let fixture: ComponentFixture<ClientesodooComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ClientesodooComponent]
    });
    fixture = TestBed.createComponent(ClientesodooComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
