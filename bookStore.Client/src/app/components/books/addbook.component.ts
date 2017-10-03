import {Component, ElementRef, OnInit, Output, EventEmitter} from 'angular2/core';
import {Control} from 'angular2/common';
import {ToastsManager} from 'ng2-toastr/ng2-toastr';
import {Book} from './../interfaces.ts';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/do';
import * as _ from 'lodash';
import {AddBookService} from './addbook.service';

@Component({
  selector: 'addbooks',
  providers: [ToastsManager, AddBookService],

  template: `
              
                <div class="addbook">
                    <div *ngIf="!addToggle" class="addToggle">
                        <button  type="button" class="btn-addbook btn btn-secondary fa fa-plus fa-5x" (click)="OnAdd()"></button>
                    </div>
                    <form  #CreateForm="ngForm" class="form-horizontal" (ngSubmit)="OnCreate()">
                            <div class="form-group">
                                <label for="to" class="col-sm-3 control-label">Author</label>
                                <div class="col-sm-8">
                                    <input type="text" ngControl="Author" #author="ngForm" class="form-control" id="Author" placeholder="Author" required [(ngModel)]="Author">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="to" class="col-sm-3 control-label">Title</label>
                                <div class="col-sm-8">
                                    <input type="text" ngControl="Title" #title="ngForm" class="form-control" id="Title" placeholder="Title" required [(ngModel)]="Title">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="to" class="col-sm-3 control-label">Price</label>
                                <div class="col-sm-8">
                                    <input type="number" ngControl="Price" #price="ngForm" class="form-control" id="Price" placeholder="Price" required [(ngModel)]="Price">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="to" class="col-sm-3 control-label">Available</label>
                                <div class="col-sm-8">
                                    <input type="number" ngControl="Available" #available="ngForm" class="form-control" id="Available" placeholder="Available" required [(ngModel)]="Available">
                                </div>
                            </div>
                             <div class="form-group">
                                <div class="col-sm-offset-3 col-sm-10">
                                    <button type="button" class="btn btn-default" (click)="OnCancle()">Cancel</button>
                                    <button type="submit" [disabled]="!CreateForm.form.valid" class="btn btn-default">Create</button>
                                </div>
                            </div>

                        </form>
                </div>
`,
styles: [`

      .form-horizontal .control-label {
            padding-top: 7px;
            margin-bottom: 0;
            text-align: right;
      }
        .col-sm-3 {
            width: 25%;
        }
        .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12 {
            float: left;
        }
        .col-sm-8 {
            width: 66.66666667%;
        }
        .col-sm-10 {
            width: 83.33333333%;
        }
        .col-sm-offset-3 {
            margin-left: 25%;
        }
      .addbook{
          position: relative;
          height: 325px!important;
          width: 230px!important;
          margin:5px;
          background-color: red;
          border: dashed 2px #efefef;
      }
      .form-horizontal{
          padding-left:5px;
          padding-top:30px;
          height:100%;
          position:absolute;
          background-color:#FFFFFF;
          top:0; 
      }
      .addToggle{
          height:100%;
          width:100%;
          background-color: #FFFFFF;
          position:absolute;
          z-index:99;
          display: flex;
          align-items: center;
      }
      .btn-addbook{
          margin:  auto;
          height: 70px;
          width: 70px;
          font-size: 35pt;
          text-align: center;
      }
`]

})
export class AddBooksComponent implements OnInit {
    @Output() passResultOfBookCreation = new EventEmitter<any>(); 

    private addToggle: boolean= false;
    private Author: string;
    private Title: string;
    private Price: number;
    private Available: number;
    
    constructor(private _addBookService: AddBookService, private toastr: ToastsManager) { 
    
    }
    OnAdd(){
        this.addToggle = (this.addToggle == false)? true: false;
    }
    OnCreate(bookId){
        let AddBookItem: Book={
            Author: this.Author,
            Title: this.Title,
            Price: this.Price,
            inStock: this.Available
        }
        this._addBookService.createBook(AddBookItem)
        .subscribe(
            data => this.updateBookForUi(data),
            error => this.toastr.error("Something went wrong!"),
            () => console.log("Done")
        )
    }
    updateBookForUi(data){
        this.toastr.success("Email was sent");
        this.passResultOfBookCreation.emit(data);
        this.OnCancle();

     }
    OnCancle(){
         this.addToggle = false;
         this.Author = "";
         this.Title = "";
         this.Available = null;
         this.Price = null;
    }
    
    ngOnInit(): any {
    }
}

