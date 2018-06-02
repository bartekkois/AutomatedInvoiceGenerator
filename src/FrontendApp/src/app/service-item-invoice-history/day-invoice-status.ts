export class DayInvoiceStatus {
  isInvoiced: boolean;
  isMonthBeginning: boolean;
  date: string;

  constructor(isInvoiced, isMonthBeginning, date) {
    this.isInvoiced = isInvoiced;
    this.isMonthBeginning = isMonthBeginning;
    this.date = date;
  }
}
