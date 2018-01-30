import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import * as FileSaver from "file-saver";

import { ExportService } from './export.service';
import { Export } from './export';

@Component({
  selector: 'app-export',
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.css'],
  providers: [ExportService]
})
export class ExportComponent implements OnInit {
    exportStartDate: Date;
    exportEndDate: Date;
    exportStatusMessage: string = " ";
    isBusy: boolean = false;
    isExportingInvoices: boolean = false;
    exportInvoicesToComarchOptimaXMLFormatArchiveLogs: string = "";

    constructor(private _exportService: ExportService,
                private _routerService: Router,
                private _route: ActivatedRoute) {
    }

    ngOnInit() {
        this.isBusy = true;

        var date = new Date();
        this.exportStartDate = new Date(date.getFullYear(), date.getMonth(), 1);
        this.exportEndDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        this.isBusy = false;
    }

    private exportStartDateChanged(newExportStartDate) {
      this.exportStartDate = new Date(newExportStartDate);
    }

    private exportEndDateChanged(newExportEndDate) {
      this.exportEndDate = new Date(newExportEndDate);
    }

    private refreshExportInvoicesToComarchOptimaXMLFormatArchiveLogs() {
      this._exportService.exportInvoicesToComarchOptimaXMLFormatArchiveLogs(new Date().toISOString())
        .subscribe(response => {
          this.exportInvoicesToComarchOptimaXMLFormatArchiveLogs = "";
          var lines: string[] = response.split("\n");

          for (var i = 0, l = lines.length; i < l; i++) {
            var formatting: string = "text-info";

            if (lines[i].indexOf("[Error]") >= 0)
              formatting = "text-danger";

            if (lines[i].indexOf("[Warning]") >= 0)
              formatting = "text-warning";

            this.exportInvoicesToComarchOptimaXMLFormatArchiveLogs = this.exportInvoicesToComarchOptimaXMLFormatArchiveLogs + "<span class=" + formatting + ">" + lines[i] + "</span>" + "\n";
          }
        },
        error => {
          this.exportInvoicesToComarchOptimaXMLFormatArchiveLogs = error;
        });
    }

    exportInvoicesToComarchOptimaXMLFormatArchive() {
      this.isBusy = true;
      this.isExportingInvoices = true;
      this.exportStatusMessage = "Rozpoczęto eksport ..."

      this._exportService.exportInvoicesToComarchOptimaXMLFormatArchive(this.exportStartDate.toISOString(), this.exportEndDate.toISOString())
        .subscribe(response => {
          const contentDispositionHeader: string = response.headers.get('Content-Disposition');
          const parts: string[] = contentDispositionHeader.split(';');
          const filenamePart = parts[1].split('=')[1];
          const filename = filenamePart.substring(1, filenamePart.length - 1);
          const blob = new Blob([response.arrayBuffer()], { type: "octet/stream" });
          FileSaver.saveAs(blob, filename);

          this.exportStatusMessage = "Poprawnie zakończono eksport"
          this.refreshExportInvoicesToComarchOptimaXMLFormatArchiveLogs();
          this.isExportingInvoices = false;
          this.isBusy = false;
        },
        error => {
          this.exportStatusMessage = "Wystapił błąd podczas eksportu"
          this.refreshExportInvoicesToComarchOptimaXMLFormatArchiveLogs();
          this.isExportingInvoices = false;
          this.isBusy = false;
        });
    }
}
