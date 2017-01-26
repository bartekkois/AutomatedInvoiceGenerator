import { AutomatedInvoiceGeneratorPage } from './app.po';

describe('automated-invoice-generator App', function() {
  let page: AutomatedInvoiceGeneratorPage;

  beforeEach(() => {
    page = new AutomatedInvoiceGeneratorPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
