import { Component, Input, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import * as moment from 'moment';

import { InvoiceItemShort } from "../invoice-items/invoice-item-short";
import { DayInvoiceStatus } from './day-invoice-status';

@Component({
  selector: 'app-service-item-invoice-history',
  templateUrl: './service-item-invoice-history.component.html',
  styleUrls: ['./service-item-invoice-history.component.css'],
  providers: [DatePipe]
})
export class ServiceItemInvoiceHistoryComponent implements OnInit  {
  @Input() invoiceItemsForLastYearShorts: InvoiceItemShort[];
  yearTimeline: DayInvoiceStatus[];

  constructor(private _datePipe: DatePipe) {
  }

  ngOnInit() {
    var todaysMonthBeginningDate = moment(new Date()).startOf('month').startOf('day').toDate();
    var numberOfDaysYearAgo = moment(new Date()).add(-1, 'year').add(1, 'month').startOf('month').startOf('day').diff(todaysMonthBeginningDate, 'days', false);
    var numberOfDaysMonthAhead = moment(new Date()).endOf('month').endOf('day').diff(todaysMonthBeginningDate, 'days', false);

    // Initilize array of days
    this.yearTimeline = new Array(Math.abs(numberOfDaysYearAgo) + Math.abs(numberOfDaysMonthAhead) + 1);

    // Add dates and markers to indicate months beginnings
    for (var arrayIndex = 0; arrayIndex < (Math.abs(numberOfDaysYearAgo) + Math.abs(numberOfDaysMonthAhead) + 1); arrayIndex++) {
      var currentIndexMoment = moment(new Date(this.calculateDateFromArrayIndex(numberOfDaysYearAgo, arrayIndex)));
      var currentArrayIndexMonthBeginningMoment = moment(new Date(currentIndexMoment.toDate())).startOf('month');

      var isMonthBeginning = false;
      if (currentIndexMoment.isSame(currentArrayIndexMonthBeginningMoment) && (arrayIndex != 0)) {
        isMonthBeginning = true;
      }

      this.yearTimeline[arrayIndex] = new DayInvoiceStatus(false, isMonthBeginning, this._datePipe.transform(currentIndexMoment.toDate(), 'yyyy-MM-dd'));
    }

    // Fill yearTimeline with data from invoiceItemsForLastYearShorts
    if (this.invoiceItemsForLastYearShorts != null) {
      for (var period = 0, len = this.invoiceItemsForLastYearShorts.length; period < len; period++) {

        var invoicePeriodStartTime = new Date(this.invoiceItemsForLastYearShorts[period].invoicePeriodStartTime);
        var invoicePeriodEndTime = new Date(this.invoiceItemsForLastYearShorts[period].invoicePeriodEndTime);

        var numberOfDaysForInvoiceStartTime = moment(invoicePeriodStartTime).startOf('day').diff(todaysMonthBeginningDate, 'days', false);
        var numberOfDaysForInvoiceEndTime = moment(invoicePeriodEndTime).startOf('day').diff(todaysMonthBeginningDate, 'days', false);

        var arrayBeginning = this.calculateArrayIndexFromDay(numberOfDaysYearAgo, numberOfDaysForInvoiceStartTime);
        var arrayEnd = this.calculateArrayIndexFromDay(numberOfDaysYearAgo, numberOfDaysForInvoiceEndTime);

        for (var arrayIndex = arrayBeginning; arrayIndex <= arrayEnd; arrayIndex++) {
          if (arrayIndex > this.calculateArrayIndexFromDay(numberOfDaysYearAgo, numberOfDaysMonthAhead))
            continue;

          if (arrayIndex < this.calculateArrayIndexFromDay(numberOfDaysYearAgo, numberOfDaysYearAgo))
            continue;

          this.yearTimeline[arrayIndex].isInvoiced = true;
        }
      }
    }
  }

  calculateArrayIndexFromDay(numberOfDaysYearAgo, relativeDay) {
    return Math.abs(numberOfDaysYearAgo - relativeDay);
  }

  calculateDayFromArrayIndex(numberOfDaysYearAgo, arrayIndex) {
    return numberOfDaysYearAgo + arrayIndex;
  }

  calculateDateFromArrayIndex(numberOfDaysYearAgo, arrayIndex) {
    return moment(new Date()).startOf('month').startOf('day').add(numberOfDaysYearAgo + arrayIndex, 'day').startOf('day').toDate();
  }
}
