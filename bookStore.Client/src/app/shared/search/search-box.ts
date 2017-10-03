import {Component, Output, EventEmitter} from "angular2/core";

@Component({
  selector:'search-box',
  template:`
    <div class="row">
      <div>
          <input #input type="text" class="form-control" placeholder="Search" (input)="update.emit(input.value)">
      </div>
    </div>`
})
export class SearchBox{
  @Output() update = new EventEmitter();

  ngOnInit(){
    this.update.emit('');
  }
}
