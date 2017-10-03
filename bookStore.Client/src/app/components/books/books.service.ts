import {Inject, Injectable} from "angular2/core";
import {Http, Headers, URLSearchParams, RequestOptions} from "angular2/http";
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import {SpinnerService} from '../../shared/spinner/spinner.service';


@Injectable()

export class BookService {
    
    private baseUrl: string;
    private options:RequestOptions;

    constructor(private _http: Http , private spinnerService: SpinnerService, @Inject('API_BASE_PATH') apiBasePath){
        this.baseUrl =  apiBasePath + 'Bookstore/';
        let headers = new Headers({'Content-Type': 'application/json'});
        this.options = new RequestOptions({headers: headers});
    }

    private put(data:any, endPoint:string):Observable<any> {

        let body = JSON.stringify(data);
        this.spinnerService.show();
        return this._http
        .put(this.baseUrl + '/' + endPoint + '/', body, this.options)
        .map(result => {
            if (result.headers.has('Content-Type') && result.headers.get('Content-Type').indexOf('application/json') >= 0) {
            return result.json();
            }
            return result;
        })
        .finally(() => this.spinnerService.hide());

    }

    private post(url: string, data: any):Observable<any> {

        let body = JSON.stringify(data);
        this.spinnerService.show();
        return this._http
        .post(url, body, this.options)
        .map(result => {
            if (result.headers.has('Content-Type') && result.headers.get('Content-Type').indexOf('application/json') >= 0) {
            return result.json();
            }
            return result;
        })
        .finally(() => this.spinnerService.hide());

    }

    private httpGet(url: string, query: string, searchType: number){

        let searchParams = new URLSearchParams();
        searchParams.append('query',query);
        searchParams.append('searchType', searchType.toString());
        this.spinnerService.show();
        return this._http
            .get(url , { search : searchParams})
            .map(result => {
                if (result.headers.has('content-type') && result.headers.get('content-type').indexOf('application/json') !==null) {
                return result.json();
                }
                return result;
            })
            .finally(() => this.spinnerService.hide());
    }
    private httpDelete(url: string, query: string){

        let searchParams = new URLSearchParams();
        searchParams.append('id',query);
        this.spinnerService.show();
        return this._http
            .delete(url , { search : searchParams})
            .map(result => {
                if (result.headers.has('content-type') && result.headers.get('content-type').indexOf('application/json') !==null) {
                return result.json();
                }
                return result;
            })
            .finally(() => this.spinnerService.hide());
    }
    getAllBooks(query, searchType){
        return this.httpGet(this.baseUrl + "search", query, searchType );
    }

    deleteBook(bookId){
        return this.httpDelete(this.baseUrl + "deletebook", bookId );
    }

    editBook(data){
        return this.put(data,'editbook')
    }

    submitOrder(data){
        return this.post(this.baseUrl + "orderbook", data);
    }
}