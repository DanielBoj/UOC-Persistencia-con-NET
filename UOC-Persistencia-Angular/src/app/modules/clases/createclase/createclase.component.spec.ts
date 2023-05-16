import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateclaseComponent } from './createclase.component';

describe('CreateclaseComponent', () => {
  let component: CreateclaseComponent;
  let fixture: ComponentFixture<CreateclaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateclaseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateclaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
