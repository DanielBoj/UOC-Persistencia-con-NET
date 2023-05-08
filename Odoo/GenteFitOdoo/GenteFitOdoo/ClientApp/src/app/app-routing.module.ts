import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './modules/home/home.component';
import { MenuComponent } from './modules/menu/menu.component';
import { CentroComponent } from './modules/centro/centro.component';
import { ClientesComponent } from './modules/clientes/clientes.component';
import { ClasesComponent } from './modules/clases/clases.component';
import { HorariosComponent } from './modules/horarios/horarios.component';
import { EditclasesComponent } from './modules/clases/editclases/editclases.component';
import { DetalleclaseComponent } from './modules/clases/detalleclase/detalleclase.component';
import { CreateclaseComponent } from './modules/clases/createclase/createclase.component';
import { CreatehorarioComponent } from './modules/horarios/createhorario/createhorario.component';
import { CreatereservaComponent } from './modules/horarios/reservas/createreserva/createreserva.component';
import { CreateesperaComponent } from './modules/horarios/esperas/createespera/createespera.component';
import { EditclienteComponent } from './modules/clientes/editcliente/editcliente.component';
import { DetailclienteComponent } from './modules/clientes/detailcliente/detailcliente.component';
import { ReservasComponent } from './modules/horarios/reservas/reservas.component';
import { EsperasComponent } from './modules/horarios/esperas/esperas.component';
import { OdooComponent } from './modules/odoo/odoo.component';

const routes: Routes = [
  {
    path: '', redirectTo: 'home', pathMatch: 'full'
  },
  {
    path: 'home', component: HomeComponent, title: 'Home', pathMatch: 'full'
  },
  {
    path: 'menu', component: MenuComponent, title: 'Menu', pathMatch: 'full'
  },
  {
    path: 'centro', component: CentroComponent, title: 'Centro', pathMatch: 'full'
  },
  {
    path: 'clientes', component: ClientesComponent, title: 'Clientes', runGuardsAndResolvers: 'always', pathMatch: 'full',
  },
  {
    path: 'clienteedit/:id', component: EditclienteComponent, title: 'Editar Cliente', pathMatch: 'full'
  },
  {
    path: 'clientedetalle/:id', component: DetailclienteComponent, title: 'Detalle Cliente', pathMatch: 'full'
  },
  {
    path: 'clases', component: ClasesComponent, title: 'Clases', pathMatch: 'full', runGuardsAndResolvers: 'always'
  },
  {
    path: 'clasedetalle/:id', component: DetalleclaseComponent, title: 'Detalle Clase', pathMatch: 'full'
  },
  {
    path: 'clases/edit/:id', component: EditclasesComponent, title: 'Editar Clases', pathMatch: 'full'
  },
  {
    path: 'clasescreate', component: CreateclaseComponent, title: 'Crear Clases', pathMatch: 'full'
  },
  {
    path: 'horarios', component: HorariosComponent, title: 'Horario', pathMatch: 'full', runGuardsAndResolvers: 'always'
  },
  {
    path: 'horarioscreate', component: CreatehorarioComponent, title: 'Nuevo Horario', pathMatch: 'prefix'
  },
  {
    path: 'reservacreate', component: CreatereservaComponent
  },
  {
    path: 'esperacreate', component: CreateesperaComponent
  },
  {
    path: 'reservas', component: ReservasComponent, pathMatch: 'full'
  },
  {
    path: 'esperas', component: EsperasComponent, pathMatch: 'full'
  },
  {
    path: 'odoo', component: OdooComponent, pathMatch: 'full'
  },
  {
    path: '**', redirectTo: 'home', pathMatch: 'full'
  }
];



@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
