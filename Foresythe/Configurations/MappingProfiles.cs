using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Entities;
using Entities.DTOs;

namespace Foresythe.Configurations
{
    public class MappingProfiles : Profile 
    {
        public MappingProfiles()
        {
            CreateMap<BookInputDto, Book>().ReverseMap();

            CreateMap<Book, BookOutputDto>().AfterMap<UseCustomConverter>();

            CreateMap<AuthorInputDto, Author>()
                .ForMember(c => c.Name,
                    opt => opt.MapFrom(x => string.Join(' ', x.FirstName, x.LastName)))
                .ReverseMap();

            CreateMap<Author, AuthorOutputDto>();
        }
    }

    public class UseCustomConverter : IMappingAction<Book, BookOutputDto>
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
