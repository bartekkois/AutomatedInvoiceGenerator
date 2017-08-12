import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class InvoicesService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    private getInvoiceUrl(invoiceId) {
        return this._apiUrl + 'Invoices' + "/" + invoiceId;
    }

    getInvoice(invoiceId) {
        return this._http.get(this.getInvoiceUrl(invoiceId))
            .map(res => res.json());
    }

    getInvoicesByDate(startDate, endDate) {
        return this._http.get(this._apiUrl + 'InvoicesByDateAndCustomer' + "/" + startDate + "/" + endDate)
            .map(res => res.json());
    }

    getInvoicesByDateAndCustomer(startDate, endDate, customerId) {
        return this._http.get(this._apiUrl + 'InvoicesByDateAndCustomer' + "/" + startDate + "/" + endDate + "/" + customerId)
            .map(res => res.json());
    }

    addInvoice(invoice) {
        return this._http.post(this._apiUrl + 'Invoices', invoice)
            .map(res => res.json());
    }

    updateInvoice(invoice) {
        return this._http.put(this.getInvoiceUrl(invoice.id), invoice)
            .map(res => res.json());
    }

    deleteInvoice(invoiceId) {
        return this._http.delete(this.getInvoiceUrl(invoiceId))
            .map(res => res.json());
    }
}
