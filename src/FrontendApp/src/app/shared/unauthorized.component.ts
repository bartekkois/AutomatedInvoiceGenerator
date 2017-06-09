import { Component } from '@angular/core';
;

@Component({
    selector: 'unauthorized',
    template: `
    <h2>Brak uprawnień</h2>
    <hr />
    <span>Zaloguj się aby kontynuować</span>
    `
})
export class UnauthotizedComponent {

}