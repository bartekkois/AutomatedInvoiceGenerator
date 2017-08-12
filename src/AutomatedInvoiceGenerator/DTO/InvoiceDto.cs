using AutomatedInvoiceGenerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class InvoiceDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public bool IsExported { get; set; }

        [EnumDataType(typeof(InvoiceDeliveryType))]
        [Required]
        public InvoiceDeliveryType InvoiceDelivery { get; set; }

        [Required]
        public PriceCalculationType PriceCalculation { get; set; }

        [EnumDataType(typeof(PaymentMethodType))]
        [Required]
        public PaymentMethodType PaymentMethod { get; set; }

        public int PaymentPeriod { get; set; }

        // Relationships
        public int CustomerId { get; set; }
        public virtual CustomerShortDto Customer { get; set; }

        public int? ServiceItemSetId { get; set; }

        public virtual ICollection<InvoiceItemDto> InvoiceItems { get; set; }
    }
}
