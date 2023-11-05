using AutoMapper;
using LibrariesFluentValidation.DTOs;
using LibrariesFluentValidation.Models;
namespace LibrariesFluentValidation.Mapping
{
    public class EventDateProfile :Profile
    {
        public EventDateProfile()
        {
            CreateMap<EventDateDto, EventDate>()
            .ForMember(x => x.Date, opt => opt.MapFrom(x => new DateTime(x.Year, x.Month, x.Day)));//ReverseMap tarih bazlı işlemde çalışmayacaktır.
            CreateMap<EventDate, EventDateDto>()
                .ForMember(x => x.Year, opt => opt.MapFrom(x => x.Date.Year))
                .ForMember(x => x.Month, opt => opt.MapFrom(x => x.Date.Month))
                .ForMember(x => x.Year, opt => opt.MapFrom(x => x.Date.Day));
        }
    }
}