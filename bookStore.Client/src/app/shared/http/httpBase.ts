import {Inject, Injectable} from 'angular2/core';
import {Http, URLSearchParams, RequestOptions, Headers} from 'angular2/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import {SpinnerService} from "../spinner/spinner.service";

export class httpBase{

  private baseUrl: string;
  private options: RequestOptions;

  constructor(private http: Http, private spinnerService:SpinnerService){

    let headers = new Headers({ 'Content-Type': 'application/json' });
    this.options = new RequestOptions({ headers: headers });

  }


  post(url:string,data) {

    let body = JSON.stringify(data);

    this.spinnerService.show();

    return this.http
      .post(url, body, this.options)
      .map(result => {
        if (result.headers.has('Content-Type') && result.headers.get('Content-Type').indexOf('application/json') >= 0) {
          return result.json();
        }
        return result;
      })
      .finally(() => this.spinnerService.hide());

  }
    
  put(){
  }




}
