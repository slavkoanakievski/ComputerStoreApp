using AutoMapper;
using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;

namespace ComputerStoreApp.Mapper
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Category, CategoryResource>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductResource>().ReverseMap();

        }
    }
}
