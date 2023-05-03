import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailclienteComponent } from './detailcliente.component';

describe('DetailclienteComponent', () => {
  let component: DetailclienteComponent;
  let fixture: ComponentFixture<DetailclienteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetailclienteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailclienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
