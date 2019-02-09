
import {map} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';


@Injectable()
export class OneTimeServiceItemsService {
    private _apiUrl = "http://localhost:5000/api/";

    constructor(private _http: Http) { }

    private getOneTimeServiceItemUrl(oneTimeServiceItemId) {
        return this._apiUrl + 'OneTimeServiceItems' + "/" + oneTimeServiceItemId;
    }

    getOneTimeServiceItems() {
        return this._http.get(this._apiUrl + 'OneTimeServiceItems').pipe(
            map(res => res.json()));
    }

    //getOneTimeServiceItemsByServiceItemsSet(serviceItemsSetId) {
    //    return this._http.get(this._apiUrl + 'OneTimeServiceItemsByServiceItemsSet'+ "/" + serviceItemsSetId)
    //        .map(res => res.json());
    //}

    getOneTimeServiceItem(oneTimeServiceItemId) {
        return this._http.get(this.getOneTimeServiceItemUrl(oneTimeServiceItemId)).pipe(
            map(res => res.json()));
    }

    addOneTimeServiceItem(oneTimeServiceItem) {
        return this._http.post(this._apiUrl + 'OneTimeServiceItems', oneTimeServiceItem).pipe(
            map(res => res.json()));
    }

    updateOneTimeServiceItem(oneTimeServiceItem) {
        return this._http.put(this.getOneTimeServiceItemUrl(oneTimeServiceItem.id), oneTimeServiceItem).pipe(
            map(res => res.json()));
    }

    deleteOneTimeServiceItem(oneTimeServiceItemId) {
        return this._http.delete(this.getOneTimeServiceItemUrl(oneTimeServiceItemId)).pipe(
            map(res => res.json()));
    }
}
