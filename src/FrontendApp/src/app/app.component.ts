import { Component, OnInit, LOCALE_ID } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [{ provide: LOCALE_ID, useValue: 'pl-PL' }]
})

export class AppComponent implements OnInit {
    isPathUnauthorized: boolean = false;

    constructor(private _routerService: Router) {  }

    ngOnInit() {
        this._routerService.events.subscribe(event => {
            if (event.url === '/unauthorized')
                this.isPathUnauthorized = true;
            else
                this.isPathUnauthorized = false;
            });
    }
}
