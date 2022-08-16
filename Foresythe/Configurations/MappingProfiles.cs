using System.Collections.Generic;
using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace Foresythe.Configurations
{
    public class MappingProfiles : Profile 
    {
        public MappingProfiles()
        {
            CreateMap<BookInputDto, Book>().ReverseMap();

            CreateMap<Book, BookOutputDto>().AfterMap<AuthorListFormatter>();

            CreateMap<AuthorInputDto, Author>()
                .ForMember(c => c.Name,
                    opt => opt.MapFrom(x => string.Join(' ', x.FirstName, x.LastName)))
                .ReverseMap();

            CreateMap<Author, AuthorOutputDto>();

            CreateMap<UserForRegistrationDto, User>();
        }
    }

    public class AuthorListFormatter : IMappingAction<Book, BookOutputDto>
    {
        public void Process(Book source, BookOutputDto destination, ResolutionContext context)
        {
            destination.AuthorsNames = new List<string>();
            foreach (var author in source.Authors)
            {
                string name = author.Name;
                destination.AuthorsNames.Add(name);
            }
        }
    }

}
