using AutoMapper;
using LibrariesFluentValidation.DTOs;
using LibrariesFluentValidation.Models;
namespace LibrariesFluentValidation.Mapping
{
    public class CustomerProfile:Profile//Dönüştürme işlemi gerçekleşmesi için profile üzerinden bir inheritance aldık. Fluent Validation için AbstractValidator ne ise AutoMapper için Profile odur. Program.cs içerisindeki services Profile class'ından inheritance alanları bulur.
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();//Bu metot generic olarak bir kaynak alır ve bir dönüştürecek. Kaynağın Customer bu kaynağı CustomerDto dönüştür.
            //CreateMap<CustomerDto, Customer>();=ReverseMap metodu bu görevi gerçekleştirebiliyor.
            CreateMap<Customer, CustomerDtoTurkce>()
                .ForMember(dest => dest.MusteriIsim, opt => opt.MapFrom(x => x.CustomerName))
                .ForMember(dest=>dest.Eposta,opt=>opt.MapFrom(x=>x.CustomerMail))
                .ForMember(dest=>dest.MusteriYas,opt=>opt.MapFrom(x=>x.CustomerAge));
        }
    }
}
