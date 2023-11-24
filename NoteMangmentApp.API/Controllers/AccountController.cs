using AutoMapper.Execution;
using AutoMapper;
using Core.Models;
using Data.UnitWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteMangmentApp.API.Errors;
using NoteMangmentApp.API.Services;
using Microsoft.EntityFrameworkCore;
using NoteMangmentApp.API.ViewModels;

namespace NoteMangmentApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
       
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
           
           _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<MemberVM>> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<MemberVM>> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
           
        }

        
    }
}
