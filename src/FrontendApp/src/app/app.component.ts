import { Component, OnInit } from '@angular/core';
import { Event, Router, NavigationEnd } from '@angular/router';
import { registerLocaleData } from '@angular/common';
import localePl from '@angular/common/locales/pl';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
    isPathUnauthorized: boolean = false;

    constructor(private _routerService: Router) {  }

    ngOnInit() {
        registerLocaleData(localePl);

        this._routerService.events
            .subscribe((event: Event) => {
                if (event instanceof NavigationEnd && event.url === '/unauthorized')
                    this.isPathUnauthorized = true;
                else
                    this.isPathUnauthorized = false;
            });
        }
}
