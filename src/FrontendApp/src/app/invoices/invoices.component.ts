import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';

import { InvoicesService } from './invoices.service';
import { Invoice } from './invoice';
import { InvoiceItem } from './invoice-item';
import { CustomersService } from '../customers/customers.service';
import { Customer } from '../customers/customer';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html',
  styleUrls: ['./invoices.component.css'],
  providers: [InvoicesService, CustomersService]
})
export class InvoicesComponent implements OnInit {
    invoices: Invoice[];
    filterTerm: string;
    filteredInvoices: Invoice[];
    startPeriodDate: Date;
    endPeriodDate: Date;
    isBusy: boolean = false;

    constructor(private _invoicesService: InvoicesService,
                private _customersService: CustomersService,
                private _routerService: Router,
                private _route: ActivatedRoute) {
    }

    ngOnInit() {
        this.isBusy = true;

        var date = new Date();
        this.startPeriodDate = new Date(date.getFullYear(), date.getMonth(), 1);
        this.endPeriodDate = new Date(date.getFullYear(), date.getMonth() +1, 0);

        this._route.params
            .subscribe(params => {
                this.refreshInvoicesView(this.startPeriodDate.toISOString(), this.endPeriodDate.toISOString());
            });
    }

    filterInvoicesByDate(startPeriodDate, endPeriodDate) {
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
            var index = this.invoices.indexOf(invoice)
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
}
