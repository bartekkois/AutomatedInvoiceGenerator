
import {map} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http, Response, ResponseContentType } from '@angular/http';


@Injectable()
export class ExportService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    exportInvoicesToComarchOptimaXMLFormatArchive(exportStartDate, exportEndDate) {
      return this._http.get(this._apiUrl + 'ExportInvoicesToComarchOptimaXMLFormatArchive' + "/" + exportStartDate + "/" + exportEndDate, { responseType: ResponseContentType.ArrayBuffer }).pipe(
        map(res => res));
    }

    exportInvoicesToComarchOptimaXMLFormatArchiveLogs(logsDate) {
      return this._http.get(this._apiUrl + 'ExportInvoicesToComarchOptimaXMLFormatArchiveLogs' + "/" + logsDate).pipe(
        map(res => res.text()));
    }
}
