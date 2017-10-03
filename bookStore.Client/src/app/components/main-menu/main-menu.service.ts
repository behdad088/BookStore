import {Injectable} from 'angular2/core';

export interface MainMenuItem {
  name:string;
  routerLink?:string[];
  categories?:Category[];
  active?:boolean;
}

export interface Category {
  name:string;
  items:RouterLinkItem[];
}

export interface RouterLinkItem {
  name:string;
  routerLink:string[];
}

@Injectable()
export class MainMenuService {

  constructor() {}

  get():MainMenuItem[] {
    return [
      {
        name: 'BookStore',
        categories: [
          {
            name: 'Dashboard',
            items: [
              {
                name: 'Home',
                routerLink: ['/Home']
              }
            ]
          },
          {
            name: 'Book',
            items: [
              {
                name: 'All Books',
                routerLink: ['/Library']
              },
              {
                name: 'Ordered Book',
                routerLink: ['/OrderList']
              }
            ]
          }
        ]
      }
    ];
  }

}
