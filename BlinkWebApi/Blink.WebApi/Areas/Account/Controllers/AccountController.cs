using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Blink.Core.Dtos;
using Blink.Core.Entities;
using Blink.Core.UnitsOfWork;
using Blink.Data.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Blink.WebApi.Areas.Account.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private IPersonUnit _personUnit;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AccountController(IPersonUnit personUnit, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _personUnit = personUnit;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]PersonDto personDto)
        {
            // map dto to entity
            var person = _mapper.Map<Person>(personDto);

            try
            {
                if (person.TypeUserId == _appSettings.TypeUserAffiate)
                {
                    string code = "234234235";
                    person.Affiliate = new Affiliate()
                    {
                        CodeAffiliate = code,//Genera codigo
                        PersonId = person.PersonId,
                        CreationDate = DatetimeBo.CurrentDateTime(),
                        Deleted = false
                    };
                }
                if (person.Address != null)
                {
                    person.Address.ToList().ForEach(x =>
                    {
                        x.Deleted = false;
                    });
                }
                // save 
                _personUnit.PersonRepository.Create(person, personDto.Password);
                _personUnit.SaveChanges();
                return Ok(person);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]PersonDto personDto)
        {
            Person person = _personUnit.PersonRepository.Authenticate(personDto.Email, personDto.MobilePhone, personDto.Password);

            if (person == null)
                return Unauthorized();

            personDto = _mapper.Map<PersonDto>(person);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, personDto.PersonId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = personDto.PersonId,
                Email = personDto.Email,
                MobilePhone = personDto.MobilePhone,
                Name = personDto.Name,
                MaternalSurname = personDto.MaternalSurname,
                PaternalSurname = personDto.PaternalSurname,
                Token = tokenString,
                TypeUser = personDto.TypeUserId
            });
        }

        [HttpPut]
        public IActionResult Update([FromBody]PersonDto personDto)
        {
            // map dto to entity
            var person = _mapper.Map<Person>(personDto);
            try
            {

                if (person.Address != null && person.Address.Count() > 0)
                {
                    person.Address.ToList().ForEach(x =>
                    {
                        x.Deleted = false;
                    });

                    BinnacleAddress binnacleAddress = new BinnacleAddress();
                    Address addressToUpdate = new Address();
                    foreach (Address item in person.Address)
                    {
                        addressToUpdate = _personUnit.AddressRepository.Get(item.AddressId);
                        binnacleAddress = new BinnacleAddress();
                        binnacleAddress = _mapper.Map<BinnacleAddress>(addressToUpdate);
                        binnacleAddress.CreationDate = DatetimeBo.CurrentDateTime();
                        _personUnit.BinnacleAddressRepository.Insert(binnacleAddress);
                        _personUnit.AddressRepository.Update(item);
                    }

                }

                // save 
                _personUnit.PersonRepository.Update(person, personDto.Password);
                _personUnit.SaveChanges();
                return Ok(person);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchByEmail")]
        public IActionResult SearchByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is empty");
                }

                Person person = _personUnit.PersonRepository.GetByEmail(email);
                if(person == null)
                {
                    return Ok();
                }
                return Ok(person.Email);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    return BadRequest(ex.Message);
                }

                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("SearchByRFC")]
        public IActionResult SearchByRFC(string rfc)
        {
            try
            {
                if (string.IsNullOrEmpty(rfc))
                {
                    return BadRequest("RFC is empty");
                }

                Person person = _personUnit.PersonRepository.GetByRfc(rfc);
                if (person == null)
                {
                    return Ok();
                }
                return Ok(person.Rfc);              
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    return BadRequest(ex.Message);
                }

                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("SearchByMobilePhone")]
        public IActionResult SearchByMobilePhone(string mobilePhone)
        {
            try
            {
                if (string.IsNullOrEmpty(mobilePhone))
                {
                    return BadRequest("Mobile Phone is empty");
                }

                Person person = _personUnit.PersonRepository.GetByMobilePhone(mobilePhone);
                if (person == null)
                {
                    return Ok();
                }
                return Ok(person.MobilePhone);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    return BadRequest(ex.Message);
                }

                return BadRequest(ex.InnerException.Message);
            }
        }
    }


}