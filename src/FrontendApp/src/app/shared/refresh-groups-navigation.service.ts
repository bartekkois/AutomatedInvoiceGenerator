import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class RefreshGroupsNavigationService {
    refreshGroupsNavigation = new EventEmitter();

    constructor() { }

    sendRefreshEvent() {
        this.refreshGroupsNavigation.emit();
    }
}