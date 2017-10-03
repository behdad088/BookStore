import {Component, ElementRef, OnInit, Output, EventEmitter} from 'angular2/core';
import {Control} from 'angular2/common';
import {ToastsManager} from 'ng2-toastr/ng2-toastr';
import {Book} from './../interfaces.ts';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/do';
import * as _ from 'lodash';
import {orderService} from './order.service';

@Component({
  selector: 'order',
  providers: [ToastsManager, orderService],

  template: `
              
              <div class="orderInfo-box">
                    <table class="table table-bordered">
                        <tr>
                            <th></th>
                            <th>Author</th>
                            <th>Title</th>
                            <th>Count</th>
                            <th>Total Price</th>
                        </tr>
                        <tr *ngFor="#data of AllOrderList; #indexOf=index">
                            <td>{{indexOf + 1}}</td>
                            <td>{{data.author}}</td>
                            <td>{{data.title}}</td>
                            <td>{{data.number}}</td>
                            <td>{{data.totalPrice}}</td>
                        </tr>
                    </table>
              </div>
               
`,
styles: [`
        .orderInfo-box{
            max-width:1200px;
            margin: 20px auto;
        }
      }
`]

})
export class OrderComponent implements OnInit {
    private AllOrderList: Array<any>;
    constructor(private _orderService: orderService, private toastr: ToastsManager) { 
    
    }

    getAllOrder(){
        this._orderService.getOrder()
        .subscribe(
            data => this.AllOrderList = data.result,
            error => this.toastr.error("Something went wrong!"),
            () => console.log()
        )
    }
    ngOnInit(): any {
        this.getAllOrder();
    }
}

