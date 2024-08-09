using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegApi.Domain.Entities;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Web.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class IdentityUserRegistration : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public IdentityUserRegistration(IUserService userRepo, IMapper mapper)
        {
            _userService = userRepo;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationModel userRegistrationModel)
        {
            if (userRegistrationModel is null)
                return BadRequest();

            var result = await _userService.RegistrateAsync(userRegistrationModel);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseModel { Errors = errors });
            }

            return StatusCode(201);
        }

    }
}
