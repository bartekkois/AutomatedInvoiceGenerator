using System;
using System.Threading.Tasks;

namespace AutomatedInvoiceGenerator.Services
{
    public interface IExportService
    {
        Task ExportToOptimaXML(DateTime exportStartDate, DateTime exportEndDate);
    }
}
