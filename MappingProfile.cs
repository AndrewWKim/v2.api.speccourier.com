using AutoMapper;
using SpeccourierApiV2.Core.Entities;
using SpeccourierApiV2.Models;

namespace SpeccourierApiV2
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Package, PackageDate>();
        }
    }
}
