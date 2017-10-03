import {Inject, Injectable} from "angular2/core";
import {Http, Headers, URLSearchParams, RequestOptions} from "angular2/http";
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import {SpinnerService} from '../../shared/spinner/spinner.service';
import {Book} from './../interfaces.ts';


@Injectable()

export class orderService {
    
    private baseUrl: string;

    constructor(private _http: Http , private spinnerService: SpinnerService, @Inject('API_BASE_PATH') apiBasePath){
        this.baseUrl =  apiBasePath + 'Bookstore/';
    }

    private httpGet(url: string){

        let searchParams = new URLSearchParams();
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

    getOrder(){
       return this.httpGet(this.baseUrl + "order" )
    }
}