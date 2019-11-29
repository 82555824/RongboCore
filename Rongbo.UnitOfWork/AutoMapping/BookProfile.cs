using AutoMapper;
using Rongbo.Common.AutoMapper;
using Rongbo.Entity;
using Rongbo.Model.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.UnitOfWork.AutoMapping
{
    public class BookProfile : Profile, IProfile
    {
        public BookProfile()
        {
            CreateMap<BookRequest, Book>();
        }
    }
}
