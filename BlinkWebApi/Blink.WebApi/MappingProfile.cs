using AutoMapper;
using Blink.Core.Dtos;
using Blink.Core.Entities;

namespace Blink.WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Person, PersonDto>();
            CreateMap<PersonDto, Person>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<Address, BinnacleAddress>().ForSourceMember(address => address.Person, opt => opt.Ignore()).ForSourceMember(address => address.BinnacleAddress, opt => opt.Ignore());
        }
    }
}
