import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceItemInvoiceHistoryComponent } from './service-item-invoice-history.component';

describe('ServiceItemInvoiceHistoryComponent', () => {
  let component: ServiceItemInvoiceHistoryComponent;
  let fixture: ComponentFixture<ServiceItemInvoiceHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ServiceItemInvoiceHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceItemInvoiceHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
