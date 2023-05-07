import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatereservaComponent } from './createreserva.component';

describe('CreatereservaComponent', () => {
  let component: CreatereservaComponent;
  let fixture: ComponentFixture<CreatereservaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatereservaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatereservaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
