import { Injectable }    from '@angular/core';
import { Http }          from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class CustomersService {
    private _url = "http://localhost:55098/api/Customers";

    constructor(private _http: Http) { }

    getCustomers() {
        return this._http.get(this._url)
            .map(res => res.json());
    }

    getcustomer(customerId) {
        return this._http.get(this.getCustomerUrl(customerId))
            .map(res => res.json());
    }

    addcustomer(customer) {
        return this._http.post(this._url, JSON.stringify(customer))
            .map(res => res.json());
    }

    updatecustomer(customer) {
        return this._http.put(this.getCustomerUrl(customer.id), JSON.stringify(customer))
            .map(res => res.json());
    }

    deletecustomer(customerId) {
        return this._http.delete(this.getCustomerUrl(customerId))
            .map(res => res.json());
    }

    private getCustomerUrl(customerId) {
        return this._url + "/" + customerId;
    }
}
