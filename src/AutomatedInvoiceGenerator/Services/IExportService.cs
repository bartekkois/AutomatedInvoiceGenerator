using System;

namespace AutomatedInvoiceGenerator.Services
{
    public interface IExportService
    {
        void FlushOrCreateDirectory(string path);
        void ExportInvoicesToComarchOptimaXMLFormat(DateTime exportStartDate, DateTime exportEndDate, string temporaryXMLDirectory);
        void CreateZipArchive(string sourceDirectory, string destinationFile);
    }
}
