import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleclaseComponent } from './detalleclase.component';

describe('DetalleclaseComponent', () => {
  let component: DetalleclaseComponent;
  let fixture: ComponentFixture<DetalleclaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetalleclaseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetalleclaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
