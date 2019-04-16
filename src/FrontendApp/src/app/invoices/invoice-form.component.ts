import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { DatePipe } from '@angular/common';

import { InvoicesService } from './invoices.service';
import { Invoice } from './invoice';
import { CustomerShort } from '../customers/customerShort';
import { CustomersService } from '../customers/customers.service';

@Component({
  selector: 'invoice-form',
  templateUrl: './invoice-form.component.html',
  styleUrls: ['./invoice-form.component.css'],
  providers: [InvoicesService, CustomersService, DatePipe]
})
export class InvoiceFormComponent implements OnInit {
    invoice = new Invoice();
    customersShort: CustomerShort[];
    invoiceForm: FormGroup;
    title: string = " ";
    isCustomerEditable = false;
    isBusy: boolean = false;

    constructor(private _fb: FormBuilder,
                private _invoicesService: InvoicesService,
                private _customersService: CustomersService,
                private _routerService: Router,
                private _route: ActivatedRoute,
                private _datePipe: DatePipe) {
        this.createForm();
    }

    createForm() {
        this.invoiceForm = this._fb.group({
            customerId: ['', Validators.required],
            description: [],
            invoiceDate: ['', Validators.required],
            isExported: ['', Validators.required],
            invoiceDelivery: ['', Validators.required],
            priceCalculation: ['', Validators.required],
            paymentMethod: ['', Validators.required],
            paymentPeriod: []
        });
    }

    ngOnInit() {
        this.isBusy = true;

        var id = this._route.params
            .subscribe(params => {
                var invoiceId = +params["invoiceId"];
                var customerId = +params["customerId"];

                this._customersService.getCustomer(customerId)
                    .subscribe(
                    customer => {
                        this.title = invoiceId ? "Edytuj fakturę kontrahenta " + customer.customerCode + " - " + customer.name : "Dodaj fakturę";
                    },
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['invoices']);
                        }
                    });

                this._customersService.getCustomersShort()
                    .subscribe(
                    customersShort => {
                        this.customersShort = customersShort;
                    },
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['invoices']);
                        }
                    });

                if (!invoiceId) {
                    this.isCustomerEditable = true;

                    var date = new Date();
                    this.invoice.description = "";
                    this.invoice.invoiceDate = this._datePipe.transform(date, 'yyyy-MM-dd');
                    this.invoice.isExported = false;
                    this.invoice.invoiceDelivery = 0;
                    this.invoice.priceCalculation = 0;
                    this.invoice.paymentMethod = 0;
                    this.invoice.paymentPeriod = 14;

                    this.isBusy = false;
                    return;
                }

                this._invoicesService.getInvoice(invoiceId)
                    .subscribe(
                    invoice => {
                        this.invoice = invoice;
                        this.isBusy = false;
                    }, 
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['invoices']);
                        }
                        this.isBusy = false;
                    });
            });
    }

    save() {
        this.isBusy = true;
        var result;

        if (this.invoice.id)
            result = this._invoicesService.updateInvoice(this.invoice);
        else
            result = this._invoicesService.addInvoice(this.invoice)

        result.subscribe(success => {
            this.invoiceForm.markAsPristine();
            this._routerService.navigate(['invoices']);
            this.isBusy = false;
        },
        error => {
            if (error.status === 401)
                this._routerService.navigate(['unauthorized']);

            if (error.status === 404) 
                this._routerService.navigate(['invoices']);

            if (error.status === 409)
                this._routerService.navigate(['invoices']);

            this.isBusy = false;
        });
    }

    canDeactivate() {
        if (this.invoiceForm.dirty)
            return confirm("Czy chcesz odrzucić wprowadzone zmiany?");

        return true;
    }
}
