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

namespace Blink.WebApi.Areas.Account.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private IUserUnit _userUnit;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AccountController(IUserUnit userUnit, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userUnit = userUnit;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        // GET api/values   
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize]
        [HttpGet("GetAuth")]
        public IEnumerable<string> GetAuth()
        {
            return new string[] { "value1", "value2" };
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                _userUnit.UserRepository.Create(user, userDto.Password);
                _userUnit.SaveChanges();
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            User user = _userUnit.UserRepository.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return Unauthorized();

            userDto = _mapper.Map<UserDto>(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userDto.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Name = userDto.Name,
                MaternalSurname = userDto.MaternalSurname,
                PaternalSurname = userDto.PaternalSurname,
                Token = tokenString,
                TypeUser = userDto.TypeUser
            });
        }
    }


}