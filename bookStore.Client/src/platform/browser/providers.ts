/*
 * These are globally available services in any component or any other service
 */

import {provide} from 'angular2/core';

// Angular 2
import {FORM_PROVIDERS} from 'angular2/common';

// Angular 2 Http
import {HTTP_PROVIDERS} from 'angular2/http';
// Angular 2 Router
import {ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy} from 'angular2/router';

// Toastr
import {ToastOptions} from 'ng2-toastr/ng2-toastr';

let toastOptions = {
  positionClass: 'toast-top-center'
};

/*
* Application Providers/Directives/Pipes
* providers/directives/pipes that only live in our browser environment
*/
export const APPLICATION_PROVIDERS = [
  ...FORM_PROVIDERS,
  ...HTTP_PROVIDERS,
  ...ROUTER_PROVIDERS,
  provide(LocationStrategy, { useClass: HashLocationStrategy }),
  provide(ToastOptions, { useValue: new ToastOptions(toastOptions) }),
  provide('API_BASE_PATH', { useValue: 'http://localhost:15106/api/' })
];

export const PROVIDERS = [
  ...APPLICATION_PROVIDERS
];
