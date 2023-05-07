import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditclasesComponent } from './editclases.component';

describe('EditclasesComponent', () => {
  let component: EditclasesComponent;
  let fixture: ComponentFixture<EditclasesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditclasesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditclasesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
