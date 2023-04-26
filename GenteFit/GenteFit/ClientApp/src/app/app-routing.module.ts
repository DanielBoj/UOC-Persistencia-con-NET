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
    path: 'clientes', component: ClientesComponent, title: 'Clientes', pathMatch: 'full'
  },
  {
    path: 'clases', component: ClasesComponent, title: 'Clases', pathMatch: 'full'
  },
  {
    path: 'clases/:id', component: DetalleclaseComponent, title: 'Detalle Clase', pathMatch: 'full'
  },
  {
    path: 'clases/edit/:id', component: EditclasesComponent, title: 'Editar Clases', pathMatch: 'full'
  },
  {
    path: 'clasescreate', component: CreateclaseComponent, title: 'Crear Clases', pathMatch: 'full'
  },
  {
    path: 'horarios', component: HorariosComponent, title: 'Horario', pathMatch: 'full'
  },
  {
    path: '**', redirectTo: 'home', pathMatch: 'full'
  }
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
