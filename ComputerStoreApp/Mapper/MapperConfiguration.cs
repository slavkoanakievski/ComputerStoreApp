using AutoMapper;
using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Resources;

namespace ComputerStoreApp.Mapper
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Category, CategoryResource>().ReverseMap();
        }
    }
}
