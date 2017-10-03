import { Component, Input , Output,EventEmitter} from 'angular2/core';
import { ROUTER_DIRECTIVES } from 'angular2/router';

import { Category } from './main-menu.service';

@Component({
  selector: 'category',
  template: require('./category.component.html'),
  directives: [ROUTER_DIRECTIVES]
})
export class CategoryComponent {
  @Input() category: Category;
  @Output() publisherSelected = new EventEmitter();

  itemClicked() {
    this.publisherSelected.emit('Clicked');
  }
}
