import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MaterialModule } from './material/material.module';
import { HomeComponent } from './modules/home/home.component';
import { MenuComponent } from './modules/menu/menu.component';
import { CentroComponent } from './modules/centro/centro.component';
import { ClientesComponent } from './modules/clientes/clientes.component';
import { ClasesComponent } from './modules/clases/clases.component';
import { HorariosComponent } from './modules/horarios/horarios.component';
import { NavComponent } from './modules/partials/nav/nav.component';
import { FooterComponent } from './modules/partials/footer/footer.component';
import { MatToolbarModule } from '@angular/material/toolbar';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    MenuComponent,
    CentroComponent,
    ClientesComponent,
    ClasesComponent,
    HorariosComponent,
    NavComponent,
    FooterComponent,
  ],
  imports: [
    BrowserModule/*.withServerTransition({ appId: 'ng-cli-universal' })*/,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    NgbModule,
    MatToolbarModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
