import { Injectable } from '@angular/core';
import { Http, Response, ResponseContentType } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class GenerateInvoicesService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    generateInvoices(generateInvoicesDto) {
      return this._http.post(this._apiUrl + 'GenerateInvoices', generateInvoicesDto)
        .map(res => res);
    }

    generateInvoicesLogs(logsDate) {
      return this._http.get(this._apiUrl + 'GenerateInvoicesLogs' + "/" + logsDate)
        .map(res => res.text());
    }
}
