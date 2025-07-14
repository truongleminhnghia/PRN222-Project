using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Mappers
{
    public class SupplierMapper : Profile
    {
        public SupplierMapper()
        {
            CreateMap<Supplier, SupplierResponse>().ReverseMap();
        }
    }
}