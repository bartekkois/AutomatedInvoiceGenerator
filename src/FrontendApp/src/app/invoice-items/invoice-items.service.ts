
import {map} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';


@Injectable()
export class InvoiceItemsService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    private getInvoiceItemUrl(invoiceItemId) {
        return this._apiUrl + 'InvoiceItems' + "/" + invoiceItemId;
    }

    getInvoiceItem(invoiceItemId) {
        return this._http.get(this.getInvoiceItemUrl(invoiceItemId)).pipe(
            map(res => res.json()));
    }

    addInvoice(invoiceItem) {
        return this._http.post(this._apiUrl + 'InvoiceItems', invoiceItem).pipe(
            map(res => res.json()));
    }

    updateInvoiceItem(invoiceItem) {
        return this._http.put(this.getInvoiceItemUrl(invoiceItem.id), invoiceItem).pipe(
            map(res => res.json()));
    }

    deleteInvoiceItem(invoiceItemId) {
        return this._http.delete(this.getInvoiceItemUrl(invoiceItemId)).pipe(
            map(res => res.json()));
    }
}
