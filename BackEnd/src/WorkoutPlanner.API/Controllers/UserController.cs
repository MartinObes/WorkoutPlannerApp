using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Users;

namespace WorkoutPlanner.API.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserLogic userLogic) : ControllerBase
{
    private readonly IUserLogic _userLogic = userLogic;
     [HttpGet]
     public async Task<UsersResponseDto> GetAll()
     {
         var users = await _userLogic.GetAllUsers();
         return new UsersResponseDto(users);
     }

     [HttpGet("{name}")]
     public async Task<UserResponseDto> GetByName(string name)
     {
         var result = await _userLogic.GetUserByName(name);
         return new UserResponseDto(result);
     }

     [HttpPost]
     public async Task<UserResponseDto> Create([FromBody] CreateUserRequestDto request)
     {
         var result = await _userLogic.CreateUser(request.Password, request.Name, request.Surname, request.Email,
             request.Role);
         return new UserResponseDto(result);
     }

     [HttpPut("{name}")]
     public async Task<UserResponseDto> Update(string name, [FromBody] UpdateUserRequestDto request)
     {
         var result = await _userLogic.UpdateUser(name, request.Password, request.Name, request.Surname,
             request.Email, request.Role);
         return new UserResponseDto(result);
     }

     [HttpDelete]
     public async Task<IActionResult> Delete([FromBody] DeleteUserRequestDto request)
     {
         await _userLogic.DeleteUser(request.Name);
         return NoContent();
     }
     
     
}