import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class CustomersService {
    private _apiUrl = "http://localhost:55098/api/";

    constructor(private _http: Http) { }

    private getCustomerUrl(customerId) {
        return this._apiUrl + 'Customers' + "/" + customerId;
    }

    getCustomers() {
        return this._http.get(this._apiUrl + 'Customers')
            .map(res => res.json());
    }

    getCustomersByGroup(groupId) {
        return this._http.get(this._apiUrl + 'CustomersByGroup'+ "/" + groupId)
            .map(res => res.json());
    }

    getCustomer(customerId) {
        return this._http.get(this.getCustomerUrl(customerId))
            .map(res => res.json());
    }

    addCustomer(customer) {
        return this._http.post(this._apiUrl + 'Customers', customer)
            .map(res => res.json());
    }

    updateCustomer(customer) {
        return this._http.put(this.getCustomerUrl(customer.id), customer)
            .map(res => res.json());
    }

    deleteCustomer(customerId) {
        return this._http.delete(this.getCustomerUrl(customerId))
            .map(res => res.json());
    }
}
