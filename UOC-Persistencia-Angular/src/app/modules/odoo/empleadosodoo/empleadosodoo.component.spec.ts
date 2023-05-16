import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpleadosodooComponent } from './empleadosodoo.component';

describe('EmpleadosodooComponent', () => {
  let component: EmpleadosodooComponent;
  let fixture: ComponentFixture<EmpleadosodooComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EmpleadosodooComponent]
    });
    fixture = TestBed.createComponent(EmpleadosodooComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
