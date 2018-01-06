import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class InvoiceItemsService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    private getInvoiceItemUrl(invoiceItemId) {
        return this._apiUrl + 'InvoiceItems' + "/" + invoiceItemId;
    }

    getInvoiceItem(invoiceItemId) {
        return this._http.get(this.getInvoiceItemUrl(invoiceItemId))
            .map(res => res.json());
    }

    addInvoice(invoiceItem) {
        return this._http.post(this._apiUrl + 'InvoiceItems', invoiceItem)
            .map(res => res.json());
    }

    updateInvoiceItem(invoiceItem) {
        return this._http.put(this.getInvoiceItemUrl(invoiceItem.id), invoiceItem)
            .map(res => res.json());
    }

    deleteInvoiceItem(invoiceItemId) {
        return this._http.delete(this.getInvoiceItemUrl(invoiceItemId))
            .map(res => res.json());
    }
}
