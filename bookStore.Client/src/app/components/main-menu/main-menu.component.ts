import { Component, OnInit } from 'angular2/core';
import { ROUTER_DIRECTIVES } from 'angular2/router';

import { MainMenuItem, MainMenuService } from './main-menu.service';
import { CategoryComponent } from './category.component';

@Component({
  selector: 'main-menu',
  template: require('./main-menu.component.html'),
  directives: [ROUTER_DIRECTIVES, CategoryComponent]
})
export class MainMenuComponent implements OnInit {

  private menuItems: MainMenuItem[];
  private paddingLeft = 0;

  constructor(private mainMenuService: MainMenuService) { }

  ngOnInit() {
    this.menuItems = this.mainMenuService.get();
  }

  private showCategories(element: HTMLElement, item: MainMenuItem) {

    if (element.className === 'menu-item-link') {
      this.paddingLeft = Math.round(element.getBoundingClientRect().left);
    }

    this.hideCategories();
    item.active = true;

  }

  private hideCategories() {
    this.menuItems.forEach(item => item.active = false);
  }

}
