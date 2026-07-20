using System.Security.Claims;
using AutoMapper;
using ECommerce.Application.common;
using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs.Identity;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers;

[Route("api/account")]
public class AccountsController : BaseController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountsController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return HandleResult(Result<UserDto>.Fail(Error.Unauthorized("Account.Unauthorized", "Invalid email or password")));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            return HandleResult(Result<UserDto>.Fail(Error.Unauthorized("Account.Unauthorized", "Invalid email or password")));
        }

        var userDto = new UserDto
        {
            Email = user.Email!,
            DisplayName = user.DisplayName,
            Token = await _tokenService.CreateTokenAsync(user)
        };

        return HandleResult(Result<UserDto>.Ok(userDto));
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return HandleResult(Result<UserDto>.Fail(Error.Conflict("Account.EmailExists", "Email address is already in use")));
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => Error.ValidationError(e.Code, e.Description))
                .ToList();
            return HandleResult(Result<UserDto>.Fail(errors));
        }

        var userDto = new UserDto
        {
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = await _tokenService.CreateTokenAsync(user)
        };

        return HandleResult(Result<UserDto>.Ok(userDto));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return HandleResult(Result<UserDto>.Fail(Error.Unauthorized()));
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return HandleResult(Result<UserDto>.Fail(Error.NotFound("Account.NotFound", "User not found")));
        }

        var userDto = new UserDto
        {
            Email = user.Email!,
            DisplayName = user.DisplayName,
            Token = await _tokenService.CreateTokenAsync(user)
        };

        return HandleResult(Result<UserDto>.Ok(userDto));
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult> GetUserAddress()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return HandleResult(Result<AddressDto>.Fail(Error.Unauthorized()));
        }

        var user = await _userManager.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return HandleResult(Result<AddressDto>.Fail(Error.NotFound("Account.NotFound", "User not found")));
        }

        if (user.Address == null)
        {
            return HandleResult(Result<AddressDto>.Fail(Error.NotFound("Address.NotFound", "User address not set")));
        }

        var addressDto = _mapper.Map<Address, AddressDto>(user.Address);
        return HandleResult(Result<AddressDto>.Ok(addressDto));
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult> UpdateUserAddress(AddressDto addressDto)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return HandleResult(Result<AddressDto>.Fail(Error.Unauthorized()));
        }

        var user = await _userManager.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return HandleResult(Result<AddressDto>.Fail(Error.NotFound("Account.NotFound", "User not found")));
        }

        if (user.Address == null)
        {
            user.Address = _mapper.Map<AddressDto, Address>(addressDto);
        }
        else
        {
            _mapper.Map(addressDto, user.Address);
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => Error.ValidationError(e.Code, e.Description))
                .ToList();
            return HandleResult(Result<AddressDto>.Fail(errors));
        }

        var updatedAddressDto = _mapper.Map<Address, AddressDto>(user.Address);
        return HandleResult(Result<AddressDto>.Ok(updatedAddressDto));
    }
}
