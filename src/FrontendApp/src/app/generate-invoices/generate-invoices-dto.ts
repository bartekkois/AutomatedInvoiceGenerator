export class GenerateInvoicesDto {
    constructor(private _startDate: Date,
                private _invoiceDate: Date) {

      this.startDate = _startDate;
      this.invoiceDate = _invoiceDate;
    }

    startDate: Date;
    invoiceDate: Date;
}
