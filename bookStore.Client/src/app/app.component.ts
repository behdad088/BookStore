import {Component, ViewEncapsulation} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES} from 'angular2/router';

import {AppState} from './app.service';

import {MainMenuService} from './components/main-menu/main-menu.service';
import {MainMenuComponent} from './components/main-menu/main-menu.component';

import {BooksComponent} from './components/books/books.component.ts';
import {OrderComponent} from './components/books/order.component.ts';

import {SpinnerComponent} from './shared/spinner/spinner.component';
import {SpinnerService} from './shared/spinner/spinner.service';
import {HomeComponent} from "./components/dashboard/home.component";
@Component({
  selector: 'app',
  template: `
    <div class="container-fluid">

      <nav class="row">

        <div class="logo pull-left">
          <a href="/"><img src="/assets/img/BOOK_logo_162x30.png" alt="IBookStore"></a>
        </div>

        <div class="user pull-right">
          <i class="fa fa-user"></i>
        </div>

        <main-menu></main-menu>

      </nav>

      <main class="row">
        <router-outlet></router-outlet>
      </main>

      <spinner></spinner>

    </div>
  `,
  styles: [
    require('bootstrap/dist/css/bootstrap.css'),
    require('ng2-toastr/ng2-toastr.css'),
    require('./app.scss')],
  encapsulation: ViewEncapsulation.None,
  providers: [MainMenuService, SpinnerService],
  directives: [ROUTER_DIRECTIVES, MainMenuComponent, SpinnerComponent]
})
@RouteConfig([
  { path: '/dashboard/home', component: HomeComponent, name: 'Home', useAsDefault: true },
  { path: '/books/books', component: BooksComponent, name: 'Library'},
  { path: '/books/order', component: OrderComponent, name: 'OrderList'}

])
export class App {
  constructor(public appState: AppState) {
  }
}
