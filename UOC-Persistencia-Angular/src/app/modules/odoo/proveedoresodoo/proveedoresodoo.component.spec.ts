import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProveedoresodooComponent } from './proveedoresodoo.component';

describe('ProveedoresodooComponent', () => {
  let component: ProveedoresodooComponent;
  let fixture: ComponentFixture<ProveedoresodooComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProveedoresodooComponent]
    });
    fixture = TestBed.createComponent(ProveedoresodooComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
