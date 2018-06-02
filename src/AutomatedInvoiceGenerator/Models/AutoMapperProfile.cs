using AutoMapper;
using AutomatedInvoiceGenerator.DTO;

namespace AutomatedInvoiceGenerator.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Group, GroupDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>()
                .ReverseMap()
                .ForMember(dest => dest.ServiceItemsSets, opt => opt.Ignore());

            CreateMap<Customer, CustomerShortDto>();

            CreateMap<ServiceItemsSet, ServiceItemsSetDto>()
                .ReverseMap()
                .ForMember(dest => dest.OneTimeServiceItems, opt => opt.Ignore())
                .ForMember(dest => dest.SubscriptionServiceItems, opt => opt.Ignore());

            CreateMap<OneTimeServiceItem, OneTimeServiceItemDto>()
                .ForMember(dest => dest.InvoiceItemShorts, opt => opt.ResolveUsing(s => s.InvoiceItems))
                .ReverseMap()
                .ForMember(dest => dest.InvoiceItems, opt => opt.Ignore());

            CreateMap<SubscriptionServiceItem, SubscriptionServiceItemDto>()
                .ForMember(dest => dest.InvoiceItemShorts, opt => opt.ResolveUsing(s => s.InvoiceItems))
                .ReverseMap()
                .ForMember(dest => dest.InvoiceItems, opt => opt.Ignore());

            CreateMap<Invoice, InvoiceDto>()
                .ReverseMap().ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.InvoiceItems, opt => opt.Ignore());

            CreateMap<InvoiceItem, InvoiceItemDto>()
                .ReverseMap();

            CreateMap<InvoiceItem, InvoiceItemShortDto>();
        }
    }
}
