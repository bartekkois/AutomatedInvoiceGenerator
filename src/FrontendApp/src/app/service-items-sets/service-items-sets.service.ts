
import {map} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';


@Injectable()
export class ServiceItemsSetsService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    private getServiceItemsSetUrl(serviceItemsSetId) {
        return this._apiUrl + 'ServiceItemsSets' + "/" + serviceItemsSetId;
    }

    getServiceItemsSets() {
        return this._http.get(this._apiUrl + 'ServiceItemsSets').pipe(
            map(res => res.json()));
    }

    getServiceItemsSetsByCustomer(customerId) {
        return this._http.get(this._apiUrl + 'ServiceItemsSetsByCustomer'+ "/" + customerId).pipe(
            map(res => res.json()));
    }

    getServiceItemsSet(serviceItemsSetId) {
        return this._http.get(this.getServiceItemsSetUrl(serviceItemsSetId)).pipe(
            map(res => res.json()));
    }

    addServiceItemsSet(serviceItemsSet) {
        return this._http.post(this._apiUrl + 'ServiceItemsSets', serviceItemsSet).pipe(
            map(res => res.json()));
    }

    updateServiceItemsSet(serviceItemsSet) {
        return this._http.put(this.getServiceItemsSetUrl(serviceItemsSet.id), serviceItemsSet).pipe(
            map(res => res.json()));
    }

    deleteServiceItemsSet(serviceItemsSetId) {
        return this._http.delete(this.getServiceItemsSetUrl(serviceItemsSetId)).pipe(
            map(res => res.json()));
    }
}
