import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class ServiceItemsSetsService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    private getServiceItemsSetUrl(serviceItemsSetId) {
        return this._apiUrl + 'ServiceItemsSets' + "/" + serviceItemsSetId;
    }

    getServiceItemsSets() {
        return this._http.get(this._apiUrl + 'ServiceItemsSets')
            .map(res => res.json());
    }

    getServiceItemsSetsByCustomer(customerId) {
        return this._http.get(this._apiUrl + 'ServiceItemsSetsByCustomer'+ "/" + customerId)
            .map(res => res.json());
    }

    getServiceItemsSet(serviceItemsSetId) {
        return this._http.get(this.getServiceItemsSetUrl(serviceItemsSetId))
            .map(res => res.json());
    }

    addServiceItemsSet(serviceItemsSet) {
        return this._http.post(this._apiUrl + 'ServiceItemsSets', serviceItemsSet)
            .map(res => res.json());
    }

    updateServiceItemsSet(serviceItemsSet) {
        return this._http.put(this.getServiceItemsSetUrl(serviceItemsSet.id), serviceItemsSet)
            .map(res => res.json());
    }

    deleteServiceItemsSet(serviceItemsSetId) {
        return this._http.delete(this.getServiceItemsSetUrl(serviceItemsSetId))
            .map(res => res.json());
    }
}
