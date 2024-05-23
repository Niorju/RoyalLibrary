using CrossCutting.DTO;
using AutoMapper;
using RoyalLibrary.Domain.Entities;

namespace Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();
        }
    }
}
