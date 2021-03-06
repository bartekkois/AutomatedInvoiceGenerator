import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

import { InvoicesService } from './invoices.service';
import { Invoice } from './invoice';
import { InvoiceItem } from '../invoice-items/invoice-item';
import { InvoiceItemsService } from '../invoice-items/invoice-items.service';
import { CustomersService } from '../customers/customers.service';
import { Customer } from '../customers/customer';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html',
  styleUrls: ['./invoices.component.css'],
  providers: [InvoicesService, InvoiceItemsService, CustomersService, CookieService]
})
export class InvoicesComponent implements OnInit {
    invoices: Invoice[];
    filterTerm: string;
    filteredInvoices: Invoice[];
    startPeriodDate: Date;
    endPeriodDate: Date;
    isBusy: boolean = false;

    constructor(private _invoicesService: InvoicesService,
                private _invoiceItemsService: InvoiceItemsService,
                private _customersService: CustomersService,
                private _routerService: Router,
                private _route: ActivatedRoute,
                private _cookieService: CookieService) {
    }

    ngOnInit() {
        this.isBusy = true;

        var date = new Date();
        this._cookieService.check('startPeriodDate') ? this.startPeriodDate = new Date(this._cookieService.get('startPeriodDate')) : this.startPeriodDate = new Date(date.getFullYear(), date.getMonth(), 1);
        this._cookieService.check('endPeriodDate') ? this.endPeriodDate = new Date(this._cookieService.get('endPeriodDate')) : this.endPeriodDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        this._route.params
            .subscribe(params => {
                this.refreshInvoicesView(this.startPeriodDate.toISOString(), this.endPeriodDate.toISOString());
            });
    }

    filterInvoicesByDate(startPeriodDate, endPeriodDate) {
      this._cookieService.set('startPeriodDate', (new Date(startPeriodDate)).toString(), 1);
      this._cookieService.set('endPeriodDate', (new Date(endPeriodDate)).toString(), 1);
      this.refreshInvoicesView(startPeriodDate, endPeriodDate);
    }

    filterInvoicesByFilterTerm(filterTerm) {
        if (filterTerm) {
            var filterTermLowerCase = filterTerm.toLowerCase();
            this.filteredInvoices = this.invoices.filter(invoice =>
                (invoice.customer.customerCode.replace(/null/i, "\"\"").toLowerCase().indexOf(filterTermLowerCase) > -1)
                || (invoice.customer.shippingCustomerCode.replace(/null/i, "\"\"").toLowerCase().indexOf(filterTermLowerCase) > -1)
                || (invoice.customer.name.replace(/null/i, "\"\"").toLowerCase().indexOf(filterTermLowerCase) > -1)
                || (invoice.customer.brandName.replace(/null/i, "\"\"").toLowerCase().indexOf(filterTermLowerCase) > -1));
        }
        else {
            this.filteredInvoices = this.invoices;
        }
    }

    refreshInvoicesView(startPeriodDate, endPeriodDate) {
        this._invoicesService.getInvoicesByDate(startPeriodDate, endPeriodDate)
            .subscribe(
            invoices => {
                this.invoices = invoices;
                this.filteredInvoices = invoices;
                this.filterInvoicesByFilterTerm(this.filterTerm);
                this.isBusy = false;
            },
            error => {
                if (error.status === 401)
                    this._routerService.navigate(['unauthorized']);
                this.invoices = [];
                this.filteredInvoices = [];
                this.isBusy = false;
            }
        );
    }

    calculateNettoValueAdded(invoice: Invoice) {
        var total = 0;

        for (var invoiceItem of invoice.invoiceItems) {
            total = total + invoiceItem.netValueAdded;
        }

        return total;
    }

    calculateGrossValueAdded(invoice: Invoice) {
        var total = 0;

        for (var invoiceItem of invoice.invoiceItems) {
            total = total + invoiceItem.grossValueAdded;
        }

        return total;
    }

    deleteInvoice(invoice) {
        if (confirm("Czy na pewno chcesz usunąć fakturę kontrahenta " + invoice.customer.customerCode + " " + invoice.customer.name + "?")) {
            var index = this.invoices.indexOf(invoice);
            this.invoices.splice(index, 1);

            this._invoicesService.deleteInvoice(invoice.id)
                .subscribe(
                success => {
                },
                error => {
                    if (error.status === 401)
                        this._routerService.navigate(['unauthorized']);

                    alert("Usunięcie faktury nie powiodło się !!!");
                    this.invoices.splice(index, 0, invoice);
                });
        }
    }

    deleteInvoiceItem(invoiceId, invoiceItem) {
      if (confirm("Czy na pewno chcesz usunąć wybraną pozycję z faktury?")) {
        var index = this.invoices.find(i => i.id == invoiceId).invoiceItems.indexOf(invoiceItem);
        this.invoices.find(i => i.id == invoiceId).invoiceItems.splice(index, 1);

        this._invoiceItemsService.deleteInvoiceItem(invoiceItem.id)
          .subscribe(
          success => {
          },
          error => {
            if (error.status === 401)
              this._routerService.navigate(['unauthorized']);

            alert("Usunięcie pozycji z faktury nie powiodło się !!!");
            this.invoices.find(i => i.id == invoiceId).invoiceItems.splice(index, 0, invoiceItem);
          });
      }
    }
}
