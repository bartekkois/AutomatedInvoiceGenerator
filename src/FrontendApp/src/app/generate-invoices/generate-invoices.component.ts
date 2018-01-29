import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import * as FileSaver from "file-saver";

import { GenerateInvoicesService } from './generate-invoices.service';
import { GenerateInvoicesDto } from './generate-invoices-dto';

@Component({
  selector: 'app-export',
  templateUrl: './generate-invoices.component.html',
  styleUrls: ['./generate-invoices.component.css'],
  providers: [GenerateInvoicesService]
})
export class GenerateInvoicesComponent implements OnInit {
    startDate: Date;
    invoiceDate: Date;
    generateInvoicesStatusMessage: string = " ";
    isBusy: boolean = false;
    isGeneratingInvoices: boolean = false;
    generateInvoicesLogs: string = "";

    constructor(private _generateInvoicesService: GenerateInvoicesService,
                private _routerService: Router,
                private _route: ActivatedRoute) {
    }

    ngOnInit() {
        this.isBusy = true;

        var date = new Date();
        this.startDate = new Date(date.getFullYear(), date.getMonth() - 3, 1);
        this.invoiceDate = new Date(date.getFullYear(), date.getMonth(), 1);

        this.isBusy = false;
    }

    private startDateChanged(newStartDate) {
      this.startDate = new Date(newStartDate);
    }

    private invoiceDateChanged(newInvoiceDate) {
      this.invoiceDate = new Date(newInvoiceDate);
    }

    private refreshGenerateInvoicesLogs() {
      this._generateInvoicesService.generateInvoicesLogs(new Date().toISOString())
        .subscribe(response => {
          this.generateInvoicesLogs = "";
          var lines: string[] = response.split("\n");

          for (var i = 0, l = lines.length; i < l; i++) {
            var formatting: string = "text-info";

            if (lines[i].indexOf("[Error]") >= 0)
              formatting = "text-danger";

            if (lines[i].indexOf("[Warning]") >= 0)
              formatting = "text-warning";
            
            this.generateInvoicesLogs = this.generateInvoicesLogs + "<span class=" + formatting + ">" + lines[i] + "</span>" + "\n";
          }
        },
        error => {
          this.generateInvoicesLogs = error;
        });
    }

    generateInvoices() {
      this.isBusy = true;
      this.isGeneratingInvoices = true;
      this.generateInvoicesStatusMessage = "Rozpoczęto generowanie faktur ..."

      this._generateInvoicesService.generateInvoices(new GenerateInvoicesDto(this.startDate, this.invoiceDate))
        .subscribe(response => {
          this.generateInvoicesStatusMessage = "Poprawnie zakończono generowanie faktur";
          this.refreshGenerateInvoicesLogs();
          this.isGeneratingInvoices = false;
          this.isBusy = false;
        },
        error => {
          this.generateInvoicesStatusMessage = "Wystapił błąd podczas generowania faktur"
          this.refreshGenerateInvoicesLogs();
          this.isGeneratingInvoices = false;
          this.isBusy = false;
        });
    }
}
