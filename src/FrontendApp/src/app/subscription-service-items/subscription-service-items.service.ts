import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class SubscriptionServiceItemsService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    private getSubscriptionServiceItemUrl(subscriptionServiceItemId) {
        return this._apiUrl + 'SubscriptionServiceItems' + "/" + subscriptionServiceItemId;
    }

    getSubscriptionServiceItems() {
        return this._http.get(this._apiUrl + 'SubscriptionServiceItems')
            .map(res => res.json());
    }

    //getSubscriptionServiceItemsByServiceItemsSet(serviceItemsSetId) {
    //    return this._http.get(this._apiUrl + 'SubscriptionServiceItemsByServiceItemsSet'+ "/" + serviceItemsSetId)
    //        .map(res => res.json());
    //}

    getSubscriptionServiceItem(subscriptionServiceItemId) {
        return this._http.get(this.getSubscriptionServiceItemUrl(subscriptionServiceItemId))
            .map(res => res.json());
    }

    addSubscriptionServiceItem(subscriptionServiceItem) {
        return this._http.post(this._apiUrl + 'SubscriptionServiceItems', subscriptionServiceItem)
            .map(res => res.json());
    }

    updateSubscriptionServiceItem(subscriptionServiceItem) {
        return this._http.put(this.getSubscriptionServiceItemUrl(subscriptionServiceItem.id), subscriptionServiceItem)
            .map(res => res.json());
    }

    deleteSubscriptionServiceItem(subscriptionServiceItemId) {
        return this._http.delete(this.getSubscriptionServiceItemUrl(subscriptionServiceItemId))
            .map(res => res.json());
    }
}
