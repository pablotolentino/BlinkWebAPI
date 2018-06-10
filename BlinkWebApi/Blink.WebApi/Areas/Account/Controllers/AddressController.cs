using AutoMapper;
using Blink.Core.Dtos;
using Blink.Core.Entities;
using Blink.Core.UnitsOfWork;
using Blink.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blink.WebApi.Areas.Account.Controllers
{
    [Produces("application/json")]
    [Route("api/Address")]
    public class AddressController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPersonUnit _personUnit;
        private readonly AppSettings _appSettings;


        public AddressController(IMapper mapper, IOptions<AppSettings> appSettings, IPersonUnit personUnit)
        {
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _personUnit = personUnit;
        }

        [HttpPut]
        public IActionResult Update([FromBody]AddressDto addressDto)
        {
            if (addressDto == null)
            {
                return BadRequest("Null address");
            }

            try
            {
                // map dto to entity
                Address address = _mapper.Map<Address>(addressDto);
                if (address == null)
                {
                    return BadRequest("Null address");
                }

                Address addressToUpdate = _personUnit.AddressRepository.Get(address.AddressId);
                 BinnacleAddress binnacleAddress = _mapper.Map<BinnacleAddress>(addressToUpdate);
                binnacleAddress.CreationDate = DatetimeBo.CurrentDateTime();
                _personUnit.BinnacleAddressRepository.Insert(binnacleAddress);
                 _personUnit.AddressRepository.Update(address);
                _personUnit.SaveChanges();
                return Ok(address);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }

        }
    }
}