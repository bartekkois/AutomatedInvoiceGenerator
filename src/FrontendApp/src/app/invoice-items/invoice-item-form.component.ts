
import {debounceTime} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';



import { InvoicesService } from '../invoices/invoices.service';
import { Invoice } from '../invoices/invoice';
import { InvoiceItemsService } from './invoice-items.service';
import { InvoiceItem } from './invoice-item';
import { CustomerShort } from '../customers/customerShort';
import { CustomersService } from '../customers/customers.service';
import { ServiceItemsSet } from 'app/service-items-sets/service-items-set';

@Component({
  selector: 'invoice-item-form',
  templateUrl: './invoice-item-form.component.html',
  styleUrls: ['./invoice-item-form.component.css'],
  providers: [InvoiceItemsService, CustomersService, DatePipe]
})
export class InvoiceItemFormComponent implements OnInit {
    invoiceId: number;
    invoiceItem = new InvoiceItem();
    invoiceItemForm: FormGroup;
    invoiceItemTemplateTypeIsSet = false;
    currentCustomerServiceItemsSets: ServiceItemsSet[];
    title: string = " ";
    isBusy: boolean = false;

    constructor(private _fb: FormBuilder,
                private _invoicesService: InvoicesService,
                private _invoiceItemsService: InvoiceItemsService,
                private _customersService: CustomersService,
                private _routerService: Router,
                private _route: ActivatedRoute,
                private _datePipe: DatePipe) {
        this.createForm();
    }

    createForm() {
      this.invoiceItemForm = this._fb.group({
        invoiceItemTemplate: [],
        remoteSystemServiceCode: ['', Validators.required],
        description: ['', Validators.required],
        quantity: ['', Validators.required],
        units: ['', Validators.required],
        netUnitPrice: ['', Validators.required],
        netValueAdded: ['', Validators.required],
        vatRate: ['', Validators.required],
        grossValueAdded: ['', Validators.required],
        invoicePeriodStartTime: ['', Validators.required],
        invoicePeriodEndTime: ['', Validators.required],
        invoiceId: ['', Validators.required],
        serviceItemId: []
      });
    }

    ngOnInit() {
        this.isBusy = true;

        var id = this._route.params
          .subscribe(params => {
                this.invoiceId = +params["invoiceId"];
                var invoiceItemId = +params["invoiceItemId"];

                this._invoicesService.getInvoice(this.invoiceId)
                    .subscribe(
                    invoice => {
                      this.title = invoiceItemId ? "Edytuj" : "Dodaj" + " pozycję faktury o numerze wewnętrznym FID" + invoice.id + " kontrahenta " + invoice.customer.name;
                    },
                    error => {
                        if (error.status === 401)
                            this._routerService.navigate(['unauthorized']);

                        if (error.status === 404) {
                            this._routerService.navigate(['invoices']);
                        }
                    });

                this._invoicesService.getInvoice(this.invoiceId).toPromise()
                  .then(result =>
                    this._customersService.getCustomer(result.customerId)
                      .subscribe(
                      customer => {
                        this.currentCustomerServiceItemsSets = customer.serviceItemsSets;
                      })
                  )
                  .catch
                    (
                    error => {
                      if (error.status === 401)
                        this._routerService.navigate(['unauthorized']);

                      if (error.status === 404) {
                        this._routerService.navigate(['invoices']);
                      }
                      this.isBusy = false;
                    }
                  );

                if (!invoiceItemId) {
                    var date = new Date();
                    this.invoiceItem.remoteSystemServiceCode = "";
                    this.invoiceItem.description = "";
                    this.invoiceItem.quantity = 1;
                    this.invoiceItem.units = "usł.";
                    this.invoiceItem.netUnitPrice = 0;
                    this.invoiceItem.netValueAdded = 0;
                    this.invoiceItem.vatRate = 23;
                    this.invoiceItem.grossValueAdded = 0;
                    this.invoiceItem.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-dd');
                    this.invoiceItem.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-dd');
                    this.invoiceItem.invoiceId = this.invoiceId;

                    this.isBusy = false;
                    return;
                }

                this._invoiceItemsService.getInvoiceItem(invoiceItemId)
                    .subscribe(
                    invoiceItem => {
                        this.invoiceItem = invoiceItem;
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

        // Realtime form recalculations
        this.invoiceItemForm.controls["netUnitPrice"].valueChanges.pipe(
          debounceTime(100))
          .subscribe((ngModelChange) => {
            this.invoiceItem.netValueAdded = this.invoiceItem.netUnitPrice * this.invoiceItem.quantity;
            this.invoiceItem.grossValueAdded = (this.invoiceItem.netUnitPrice * this.invoiceItem.quantity) * (1 + (this.invoiceItem.vatRate / 100));
          });

        this.invoiceItemForm.controls["quantity"].valueChanges.pipe(
          debounceTime(100))
          .subscribe((ngModelChange) => {
            if (Number.isInteger(this.invoiceItem.quantity))
              this.invoiceItem.netValueAdded = this.invoiceItem.netUnitPrice * this.invoiceItem.quantity;
              this.invoiceItem.grossValueAdded = (this.invoiceItem.netUnitPrice * this.invoiceItem.quantity) * (1 + (this.invoiceItem.vatRate / 100));
          });

        this.invoiceItemForm.controls["vatRate"].valueChanges.pipe(
          debounceTime(100))
          .subscribe((ngModelChange) => {
            this.invoiceItem.netValueAdded = this.invoiceItem.netUnitPrice * this.invoiceItem.quantity;
            this.invoiceItem.grossValueAdded = (this.invoiceItem.netUnitPrice * this.invoiceItem.quantity) * (1 + (this.invoiceItem.vatRate / 100));
          });

        this.invoiceItemForm.controls["grossValueAdded"].valueChanges.pipe(
          debounceTime(100))
          .subscribe((ngModelChange) => {
            this.invoiceItem.netValueAdded = this.invoiceItem.grossValueAdded / (1 + (this.invoiceItem.vatRate / 100));
            this.invoiceItem.netUnitPrice = this.invoiceItem.grossValueAdded / (1 + (this.invoiceItem.vatRate / 100)) / this.invoiceItem.quantity;
          });
    }

    save() {
      this.isBusy = true;
      var result;

      if (this.invoiceItem.id)
        result = this._invoiceItemsService.updateInvoiceItem(this.invoiceItem);
      else
        result = this._invoiceItemsService.addInvoice(this.invoiceItem)

      result.subscribe(success => {
        this.invoiceItemForm.markAsPristine();
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

    setItemDefaultValues(invoiceItemTemplateType) {
      if (this.invoiceItemTemplateTypeIsSet == true) {
        if (!confirm("Czy chcesz odrzucić wybrany szablon usługi oraz wprowadzone dane?"))
          return;
      }

      this.invoiceItemTemplateTypeIsSet = true;
      var date = new Date();
      var invoiceItemTemplate = new InvoiceItem();

      switch (invoiceItemTemplateType) {
        case "0":
          invoiceItemTemplate.remoteSystemServiceCode = "1200";
          invoiceItemTemplate.description = "Internet fiberPORT 100/20 Mb/s - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 120.00;
          invoiceItemTemplate.netValueAdded = 120.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 147.60;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "1":
          invoiceItemTemplate.remoteSystemServiceCode = "1209";
          invoiceItemTemplate.description = "Dzierżawa modemu z funkcją routera Wi-Fi DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 6.00;
          invoiceItemTemplate.netValueAdded = 6.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 7.38;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "2":
          invoiceItemTemplate.remoteSystemServiceCode = "1209";
          invoiceItemTemplate.description = "Dzierżawa modemu z funkcją routera Wi-Fi DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 4.87;
          invoiceItemTemplate.netValueAdded = 4.87;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 6.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "3":
          invoiceItemTemplate.remoteSystemServiceCode = "1275";
          invoiceItemTemplate.description = "Opłata za dodatkowy stały publiczny adres IP DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 10.00;
          invoiceItemTemplate.netValueAdded = 10.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 12.30;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "4":
          invoiceItemTemplate.remoteSystemServiceCode = "1210";
          invoiceItemTemplate.description = "Telefon DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 19.00;
          invoiceItemTemplate.netValueAdded = 19.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 23.37;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "5":
          invoiceItemTemplate.remoteSystemServiceCode = "1210";
          invoiceItemTemplate.description = "Telefon DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 15.44;
          invoiceItemTemplate.netValueAdded = 15.44;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 19.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "6":
          invoiceItemTemplate.remoteSystemServiceCode = "1211";
          invoiceItemTemplate.description = "Telefon DETALE - połączenia telefoniczne OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 0.00;
          invoiceItemTemplate.netValueAdded = 0.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 0.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "7":
          invoiceItemTemplate.remoteSystemServiceCode = "1230";
          invoiceItemTemplate.description = "Dzierżawa pary miedzianej DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 80.00;
          invoiceItemTemplate.netValueAdded = 80.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 98.40;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "8":
          invoiceItemTemplate.remoteSystemServiceCode = "1263";
          invoiceItemTemplate.description = "Dzierżawa ciemnych włókien światłowodowych DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 80.00;
          invoiceItemTemplate.netValueAdded = 80.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 98.40;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "9":
          invoiceItemTemplate.remoteSystemServiceCode = "1243";
          invoiceItemTemplate.description = "Transmisja danych DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 250.00;
          invoiceItemTemplate.netValueAdded = 250.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 307.50;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "10":
          invoiceItemTemplate.remoteSystemServiceCode = "1310";
          invoiceItemTemplate.description = "Telewizja DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 23.05;
          invoiceItemTemplate.netValueAdded = 23.05;
          invoiceItemTemplate.vatRate = 8;
          invoiceItemTemplate.grossValueAdded = 24.90;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "11":
          invoiceItemTemplate.remoteSystemServiceCode = "1270";
          invoiceItemTemplate.description = "Opłata za fakturę tradycyjną";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 2.00;
          invoiceItemTemplate.netValueAdded = 2.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 2.46;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "12":
          invoiceItemTemplate.remoteSystemServiceCode = "";
          invoiceItemTemplate.description = "Inny DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 0.00;
          invoiceItemTemplate.netValueAdded = 0.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 0.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;

        case "20":
          invoiceItemTemplate.remoteSystemServiceCode = "1201";
          invoiceItemTemplate.description = "Internet fiberPORT DETALE Mb/s - instalacja";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 49.00;
          invoiceItemTemplate.netValueAdded = 49.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 60.27;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "21":
          invoiceItemTemplate.remoteSystemServiceCode = "1211";
          invoiceItemTemplate.description = "Telefon DETALE - instalacja (firma)";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 19.00;
          invoiceItemTemplate.netValueAdded = 19.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 23.37;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "22":
          invoiceItemTemplate.remoteSystemServiceCode = "1211";
          invoiceItemTemplate.description = "Telefon DETALE - instalacja (prywatny)";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 15.45;
          invoiceItemTemplate.netValueAdded = 15.45;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 19.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "23":
          invoiceItemTemplate.remoteSystemServiceCode = "1275";
          invoiceItemTemplate.description = "Dzierżawa pary miedzianej DETALE - instalacja";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 99.00;
          invoiceItemTemplate.netValueAdded = 99.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 121.77;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "24":
          invoiceItemTemplate.remoteSystemServiceCode = "1210";
          invoiceItemTemplate.description = "Dzierżawa ciemnych włókien światłowodowych DETALE - instalacja";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 99.00;
          invoiceItemTemplate.netValueAdded = 99.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 121.77;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "25":
          invoiceItemTemplate.remoteSystemServiceCode = "1241";
          invoiceItemTemplate.description = "Transmisja danych DETALE - instalacja";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 99.00;
          invoiceItemTemplate.netValueAdded = 99.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 121.77;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "26":
          invoiceItemTemplate.remoteSystemServiceCode = "1300";
          invoiceItemTemplate.description = "Telewizja DETALE - instalacja";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 80.49;
          invoiceItemTemplate.netValueAdded = 80.49;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 99.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        case "27":
          invoiceItemTemplate.remoteSystemServiceCode = "";
          invoiceItemTemplate.description = "Inny DETALE - instalacja";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 0.00;
          invoiceItemTemplate.netValueAdded = 0.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 0.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          break;
        default:
          invoiceItemTemplate.remoteSystemServiceCode = "";
          invoiceItemTemplate.description = "Inny DETALE - abonament OKRES";
          invoiceItemTemplate.quantity = 1;
          invoiceItemTemplate.units = "usł.";
          invoiceItemTemplate.netUnitPrice = 0.00;
          invoiceItemTemplate.netValueAdded = 0.00;
          invoiceItemTemplate.vatRate = 23;
          invoiceItemTemplate.grossValueAdded = 0.00;
          invoiceItemTemplate.invoicePeriodStartTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
          invoiceItemTemplate.invoicePeriodEndTime = this._datePipe.transform(date, 'yyyy-MM-ddThh:mm');
      }

      invoiceItemTemplate.invoiceId = this.invoiceId;

      this.invoiceItem = invoiceItemTemplate;
    }

    canDeactivate() {
        if (this.invoiceItemForm.dirty)
            return confirm("Czy chcesz odrzucić wprowadzone zmiany?");

        return true;
    }
}
