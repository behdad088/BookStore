import {Inject, Injectable} from "angular2/core";
import {Http, Headers, URLSearchParams, RequestOptions} from "angular2/http";
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import {SpinnerService} from '../../shared/spinner/spinner.service';
import {Book} from './../interfaces.ts';


@Injectable()

export class AddBookService {
    
    private baseUrl: string;
    private options:RequestOptions;

    constructor(private _http: Http , private spinnerService: SpinnerService, @Inject('API_BASE_PATH') apiBasePath){
        this.baseUrl =  apiBasePath + 'Bookstore/';
        let headers = new Headers({'Content-Type': 'application/json'});
        this.options = new RequestOptions({headers: headers});
    }

    private httpPost(url: string, data: any): Observable<any> {

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

    createBook(data: Book){
        return this.httpPost(this.baseUrl + "addbook", data);
    }
}