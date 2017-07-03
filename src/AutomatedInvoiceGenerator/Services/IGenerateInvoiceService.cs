using AutomatedInvoiceGenerator.Models;
using System;
using System.Threading.Tasks;

namespace AutomatedInvoiceGenerator.Services
{
    public interface IGenerateInvoiceService
    {
        Task GenerateInvoice(Customer customer, DateTime invoiceDate);
    }
}
