import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateesperaComponent } from './createespera.component';

describe('CreateesperaComponent', () => {
  let component: CreateesperaComponent;
  let fixture: ComponentFixture<CreateesperaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateesperaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateesperaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
