import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class GroupsService {
    private _url = "http://localhost:5000/api/Groups";

    constructor(private _http: Http) { }

    getGroups() {
        return this._http.get(this._url)
            .map(res => res.json());
    }

    getGroup(groupId) {
        return this._http.get(this.getGroupUrl(groupId))
            .map(res => res.json());
    }

    addGroup(group) {
        return this._http.post(this._url, group)
            .map(res => res.json());
    }

    updateGroup(group) {
        return this._http.put(this.getGroupUrl(group.id), group)
            .map(res => res.json());
    }

    deleteGroup(groupId) {
        return this._http.delete(this.getGroupUrl(groupId))
            .map(res => res.json());
    }

    private getGroupUrl(groupId) {
        return this._url + "/" + groupId;
    }
}
