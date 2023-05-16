import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductosodooComponent } from './productosodoo.component';

describe('ProductosodooComponent', () => {
  let component: ProductosodooComponent;
  let fixture: ComponentFixture<ProductosodooComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductosodooComponent]
    });
    fixture = TestBed.createComponent(ProductosodooComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
