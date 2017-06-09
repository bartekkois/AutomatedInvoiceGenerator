import { Component, OnInit, LOCALE_ID } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [{ provide: LOCALE_ID, useValue: 'pl-PL' }]
})

export class AppComponent implements OnInit {
    isPathUnauthorized: boolean = false;

    constructor(private _routerService: Router,
        private _route: ActivatedRoute) {
    }

    ngOnInit() {
        if (this._routerService.url === '/unauthorized')
            this.isPathUnauthorized = true;
        else
            this.isPathUnauthorized = false;
    }
}
