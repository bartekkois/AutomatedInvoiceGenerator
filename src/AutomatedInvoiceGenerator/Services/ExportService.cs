using System;
using System.Threading.Tasks;

namespace AutomatedInvoiceGenerator.Services
{
    public class ExportService : IExportService
    {
        public Task ExportToOptimaXML(DateTime exportStartDate, DateTime exportEndDate)
        {
            // Add export code here
            return Task.FromResult(0);
        }
    }
}
