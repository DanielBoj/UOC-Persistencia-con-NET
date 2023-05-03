import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EsperasComponent } from './esperas.component';

describe('EsperasComponent', () => {
  let component: EsperasComponent;
  let fixture: ComponentFixture<EsperasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EsperasComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EsperasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
