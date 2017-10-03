import {Component, ElementRef, OnInit} from 'angular2/core';
import {Control} from 'angular2/common';
import {ToastsManager} from 'ng2-toastr/ng2-toastr';
import {Edit} from './../interfaces.ts';
import {Product} from './../interfaces.ts';
import {Book} from './../interfaces.ts'
import {AddBooksComponent} from './addbook.component';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/do';
import * as _ from 'lodash';
import {SearchBox} from "../../shared/search/search-box";
import {BookService} from './books.service';

@Component({
  selector: 'view-books',
  providers: [ToastsManager, BookService],
  directives: [SearchBox, AddBooksComponent],
  template: `
            <div *ngIf="AllBooksList" class="searchBox">
                  <search-box class="seachText" (update)="SearchBook($event)"></search-box>
                  <select (change)="onSearchType($event.target.value)">
                    <option *ngFor="#product of products" [value]="product.id">{{product.name}}</option>
                  </select>
            </div>
            <div *ngIf="AllBooksList" class="main-box">
                <div class="library">
                    <div class="book-box" *ngFor="#data of AllBooksList; #indexOf=index">
                            <button  type="button" class="btn-edit btn btn-success btn-sm fa fa-pencil-square-o fa-lg" (click)="OnEdit(data.id)"></button>
                            <button  type="button" class="btn-delete btn btn-danger fa fa-trash-o fa-5x" (click)="OnDelete(data.id)"></button>
                            <img src="/assets/img/book.jpg" class="img-responsive img-rounded book-img" alt="Responsive image">
                            <div class="book-info">
                                <span >Author: {{data.author}}</span>
                                <span>Title: {{data.title}}</span>
                                <span>Price: {{data.price}}</span>
                                <div class="book-action">
                                <div class="book-availabity-info"> 
                                        <span>Available:{{data.inStock}}</span> 
                                        <span class="book-availabiity" [ngClass]="{'book-notAvailable': data.inStock < 1, 'book-isAvailable': data.inStock > 0}" ></span>
                                </div>
                                    <button  type="button" class="btn btn-primary btn-sm btn-cart" (click)="onAddToCart(data.id)" [disabled]="data.inStock < 1">Add To Cart</button>
                                </div>
                            </div>

                            <form *ngIf="data.id === ItemSelectedForEdit"  #EditForm="ngForm" class="form-horizontal" (ngSubmit)="OnSend(data.id)">
                                <div class="form-group">
                                    <label for="to" class="col-sm-3 control-label">Author</label>
                                    <div class="col-sm-7">
                                        <input type="text" ngControl="Author" #author="ngForm" class="form-control" id="Author" placeholder="Author" required [(ngModel)]="Author">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="to" class="col-sm-3 control-label">Title</label>
                                    <div class="col-sm-7">
                                        <input type="text" ngControl="Title" #title="ngForm" class="form-control" id="Title" placeholder="Title" required [(ngModel)]="Title">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="to" class="col-sm-3 control-label">Price</label>
                                    <div class="col-sm-7">
                                        <input type="number" step="any" ngControl="Price" #price="ngForm" class="form-control" id="Price" placeholder="Price" required [(ngModel)]="Price">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="to" class="col-sm-3 control-label">Available</label>
                                    <div class="col-sm-7">
                                        <input type="number" ngControl="Available" #available="ngForm" class="form-control" id="Available" placeholder="Available" required [(ngModel)]="Available">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-3 col-sm-10">
                                        <button type="submit" [disabled]="!EditForm.form.valid" class="btn btn-default">Edit</button>
                                    </div>
                                </div>

                            </form>
                    </div>
                    <addbooks (passResultOfBookCreation)="updateLibrary($event)"></addbooks>
                </div>
                <div class="cart-box">
                    <div class="cart-header"> 
                        <span class="fa fa-cart-plus">&nbsp;Cart</span>
                    </div>
                    <table *ngIf="Cart?.length > 0" class="cart-item table table-striped">
                        <tr>
                            <th></th>
                            <th>Author</th>
                            <th>Title</th>
                            <th>Price</th>
                            <th>Count</th>
                            <th></th>
                        </tr>
                        <tr *ngFor="#data of Cart; #indexOf=index">
                            <td>{{indexOf + 1}}</td>
                            <td>{{data.Author}}</td>
                            <td>{{data.Title}}</td>
                            <td>{{data.Price}}</td>
                            <td><input #input type="number" [ngModel]="defaultCartItemCount" (input)="OnBookItemCount(input.value, data.BookId )"></td>
                            <td><button type="button" class="btn btn-danger btn-xs fa fa-trash" aria-hidden="true" (click)="OnCartItemDelete(data.BookId)"></button></td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right;">
                                <span>Total</span>
                            </td>
                            <td colspan="2" style="text-align: right;">
                                {{totalCost}}
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: right;">
                                <button [disabled]="!IsCartItemCountValid || !isCartItemCountMatchStock" type="button" (click)="OnOrder()" class="btn btn-primary">Order</button>
                            </td>
                        </tr>
                        <tr *ngIf="!IsCartItemCountValid || !isCartItemCountMatchStock">
                            <td colspan="6" style="text-align: left;">
                                    <span class="invalidCount" *ngIf="!IsCartItemCountValid">Item Count Is Not Valid</span>
                                    <span class="invalidStock" *ngIf="!isCartItemCountMatchStock">Item Count Is More Than Our Stock</span>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
`,
styles: [` 
     .invalidStock, .invalidCount{
         color:red;
         font-size:11pt;
         display:block;

     }
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
        .col-sm-7 {
            width: 58.33333333%;
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
      .form-horizontal input{
          font-size: 12px;
      }
      .btn-edit{
          position:absolute;
          right:0;
          z-index:100;
          font-size:15px;
      }
      .btn-delete{
          position:absolute;
          left:0;
          z-index:100;
          font-size:15px;
      }
      .book-info{
          display: block;
          margin: 0px 5px 5px 0;
      }
      .book-availabity-info{
          display: flex;
          flex-direction: row;
      }
      .book-info span{
          display: block;
          margin-left: 5px;
      }
      .book-img{
          height: 230px!important;
          width: 230px!important;
      }  
      .book-availabiity{
          height:10px;
          width: 10px;
          border-radius: 50%;
      }
      .book-isAvailable{
          background-color:green;
      }
      .book-notAvailable{
          background-color: red!important;
      }
      .book-box{
          diplay: flex!important;
          flex-direction: column;
          position:relative;
          background-color: white;
          width: 230px;
          margin: 5px;
      }
      .book-action{
          display: flex;
          flex-direction: row;
          justify-content: space-between;
          width: 100%;
          line-height: 10px;
          align-items: center;
      }
      .library{
          display: flex;
          flex-direction: row;
          max-width:960px;
          min-height: 300px;
          flex-wrap: wrap; 
          margin:0 auto;
          justify-content: center;
      }
      .searchBox{
          display: flex;
          flex-direction: row;
          max-width:1200px;
          margin:20px auto;
          padding-left: 15px;
      }
      .main-box{
          max-width:1300px;
          margin:0 auto;
          display: flex;
          flex-direction: row;
      }
      .cart-box{
          display: flex;
          flex-direction: column;
          min-width: 330px;
          min-height: 320px;
          margin-top:5px;
          border-radius: 5px;

      }
      .cart-header{
          display: block;
          height: 40px;
          width: 100%;
          background-color: #eee;
          font-size: 14pt;
          line-height: 40px;
          text-align: center;
          border: solid 2px #413bf7;
      }
      .cart-item{
          width: 330px;
          border: solid 2px #413bf7;
          border-top:none;
          font-size:8pt;
      }
      .cart-item input{
          width:40px;
      }
      @media (min-width: 1200px) { 
            .library{
                justify-content: start!important;
            }
      }
`]

})
export class BooksComponent implements OnInit {
    private AllBooksList: Array<any>;
    private Author: string;
    private Title: string;
    private Price: number;
    private Available: string;
    private ItemSelectedForEdit: string = null;
    private AvailableBook: Array<any>;
    public searchType: Product;
    private Cart: Array<Book>;
    private defaultCartItemCount: number;
    private IsCartItemCountValid:boolean =  true;
    private isCartItemCountMatchStock:boolean = true;
    private totalCost: number = 0;
    constructor(private _bookService: BookService, private toastr: ToastsManager) { 
    
    }
    public products: Product[] = [
      { "id": 1, "name": "Title" },
      { "id": 2, "name": "Author" }
    ];
    onSearchType(productId) { 
        this.searchType = null;
        for (var i = 0; i < this.products.length; i++)
        {
          if (this.products[i].id == productId) {
            this.searchType = this.products[i];
          }
        }
    }

    updateLibrary(data){
        this.getAllBooks();
    }

    onAddToCart(bookId){
        debugger;
        if(this.Cart != undefined){
            if(this.Cart.length == 0){
            this.IsCartItemCountValid = true;
            this.isCartItemCountMatchStock = true;
            }
        }

        var book = _.filter(this.AllBooksList, d => d.id == bookId)[0]
        let data: { BookId: number, Author: string, Title: string, Price: number, Number: number  }[] = [
                                { "BookId": book.id, "Author": book.author, "Title": book.title, "Price": book.price, "Number": 1}
                            ];  
        if(this.Cart == undefined){
            this.Cart = data;
        }
        else
        {
            var IsItemAlreadyExists = _.filter(this.Cart, d => d.BookId == bookId)
            if(IsItemAlreadyExists.length == 0){
                this.Cart.push(data[0])   
            }
        }
        this.CalculateTotalCost();
    }

    CalculateTotalCost(){
        this.totalCost = 0;
        _.forEach(this.Cart, item => {
            this.totalCost += (item.Price * item.Number) 
        });
    }
    OnOrder(){
        this._bookService.submitOrder(this.Cart)
            .subscribe(
                data => this.updateUIForOrder(),
                error => this.toastr.error("Something went wrong!"),
                () => console.log()
            )
    }
     updateUIForOrder(){
        this.toastr.success("Order was successfully submitted");
        this.Cart = null;
        this.getAllBooks();
    }
    OnDelete(bookId){
        this._bookService.deleteBook(bookId)
            .subscribe(
                data => this.getAllBooks(),
                error => this.toastr.error("Something went wrong!"),
                () => console.log()
            )
    }
    
    SearchBook(query){
        let SearchType = (query == '')? 0: this.searchType.id;
        this._bookService.getAllBooks(query, SearchType)
        .subscribe(
            data => this.AllBooksList = data.result,
            error => this.toastr.error("Something went wrong!"),
            () => console.log()
        )
    }
    OnBookItemCount(count, bookId, index){
        let data = _.filter(this.Cart, d => d.BookId == bookId)[0];
        data.Number = count;
        this.CalculateTotalCost();
        this.CartItemCountValidator();
        this.CartItemCountMatchStockValidator(bookId);
    }
    OnCartItemDelete(bookId){
        this.Cart = _.filter(this.Cart, d => d.BookId != bookId );
    }
    CartItemCountValidator(){
        var data = _.filter(this.Cart, d => d.Number <= 0 );
        if(data.length > 0){
            this.IsCartItemCountValid = false;
        } else{
            this.IsCartItemCountValid = true;
        }
    }
    
    CartItemCountMatchStockValidator(bookId){
        _.forEach(this.Cart, item => {
            let stockData = _.filter(this.AllBooksList, d => d.id == item.BookId)[0];

            if(stockData.inStock < item.Number){
                this.isCartItemCountMatchStock = false;
                return false;
            } else {
                this.isCartItemCountMatchStock = true;
            }
        });
    }


    OnEdit(id){
        if(this.ItemSelectedForEdit == null || this.ItemSelectedForEdit != id){
            this.ItemSelectedForEdit = id;
        } else {
            this.ItemSelectedForEdit = null;
        }
        this.getSelectedBook(id);
    }
    getSelectedBook(bookId){
        this.AvailableBook = _.filter(this.AllBooksList, d => d.id == bookId)[0];
        this.Author = this.AvailableBook.author;
        this.Title = this.AvailableBook.title;
        this.Price = this.AvailableBook.price;
        this.Available = this.AvailableBook.inStock;
    }
    OnSend(bookId){
        let Editdata: Edit={
            Id: bookId,
            Author: this.Author,
            Title: this.Title,
            Price: this.Price,
            inStock: this.Available
        }

        this._bookService.editBook(Editdata)
            .subscribe(
                data => this.updateUI(bookId, Editdata),
                error => this.toastr.error("Something went wrong!"),
                () => console.log()
            )
    }
    updateUI(bookId, data){
        this.toastr.success("Item has successfully updated ");
        this.AvailableBook.author = data.Author;
        this.AvailableBook.price = data.Price;
        this.AvailableBook.title = data.Title;
        this.AvailableBook.inStock = data.inStock;
        this.ItemSelectedForEdit = null;
    }
    getAllBooks(){
        this._bookService.getAllBooks('',0)
        .subscribe(
            data => this.AllBooksList = data.result,
            error => this.toastr.error("Something went wrong!"),
            () => console.log()
        )
    }
    ngOnInit(): any {
        this.getAllBooks();
        this.searchType = this.products[0];
        this.defaultCartItemCount = 1;
    }
}

