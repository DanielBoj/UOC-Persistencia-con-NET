import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatehorarioComponent } from './createhorario.component';

describe('CreatehorarioComponent', () => {
  let component: CreatehorarioComponent;
  let fixture: ComponentFixture<CreatehorarioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatehorarioComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatehorarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
