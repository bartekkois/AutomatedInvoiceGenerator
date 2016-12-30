using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public enum ServiceCategoryType
    {
        [Display(Name = "Internet")]
        Internet,
        [Display(Name = "Telefon")]
        Phone,
        [Display(Name = "Dzierżawa")]
        InfrastractureLease,
        [Display(Name = "Transmisja danych")]
        EthernetLease,
        [Display(Name = "Telewizja")]
        Television,
        [Display(Name = "Inne")]
        Other,
    }

    public enum PaymentMethodType
    {
        [Display(Name = "przelew")]
        BankTransfer,
        [Display(Name = "gotówka")]
        Cash,
        [Display(Name = "inny")]
        Other
    }

    public enum PriceCalculationType
    {
        [Display(Name = "netto")]
        Netto,
        [Display(Name = "brutto")]
        Brutto
    }

    public enum InvoiceDeliveryType
    {
        [Display(Name = "elektroniczna")]
        Email,
        [Display(Name = "elektroniczna (wielokrotna)")]
        MultipleEmail,
        [Display(Name = "pocztowa")]
        PostMail,
        [Display(Name = "bezpośrednia")]
        Direct,
        [Display(Name = "inna")]
        Other,
    }
}
