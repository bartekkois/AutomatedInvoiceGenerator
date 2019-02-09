
import {map} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http, Response, ResponseContentType } from '@angular/http';


@Injectable()
export class GenerateInvoicesService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    generateInvoices(generateInvoicesDto) {
      return this._http.post(this._apiUrl + 'GenerateInvoices', generateInvoicesDto).pipe(
        map(res => res));
    }

    generateInvoicesLogs(logsDate) {
      return this._http.get(this._apiUrl + 'GenerateInvoicesLogs' + "/" + logsDate).pipe(
        map(res => res.text()));
    }
}
