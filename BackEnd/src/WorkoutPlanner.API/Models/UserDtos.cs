using WorkoutPlanner.Domain;

namespace WorkoutPlanner.API.Models;

public class CreateUserRequestDto
{
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Enums.UserRole Role { get; set; }
}

public class LoginUserRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateUserRequestDto
{
    public Guid UserId { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public Enums.UserRole? Role { get; set; }
}

public class DeleteUserRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class UserExistsRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class GetUserByNameRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Enums.UserRole Role { get; set; }

    public UserResponseDto()
    {
    }

    public UserResponseDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Role = user.Role;
    }
}

public class UsersResponseDto
{
    public IList<UserResponseDto> Users { get; set; } = new List<UserResponseDto>();

    public UsersResponseDto()
    {
    }

    public UsersResponseDto(IList<User> users)
    {
        Users = users.Select(user => new UserResponseDto(user)).ToList();
    }
}
