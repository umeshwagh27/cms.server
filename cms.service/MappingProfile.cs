using AutoMapper;
using cms.data.Model;
using cms.service.ViewModel;

namespace cms.service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCarVM, Car>().ReverseMap();
        }
    }
}
