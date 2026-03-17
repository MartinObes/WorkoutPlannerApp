using System.ComponentModel.DataAnnotations;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Application.Services.HasherService;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Users;

public class UserLogic(IUserRepository userRepository, IHasherService hasherService) : IUserLogic
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IHasherService _hasherService = hasherService ?? throw new ArgumentNullException(nameof(hasherService));

    private string _specialCharacters = "!@#$%^&*()_+[]{}|;:',.<>?/`~-=";

    public async Task<User> CreateUser(string password, string name, string surname, string email, Enums.UserRole role)
    {
        //null validations
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(surname))
        {
            throw new ArgumentException("Surname cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.");
        }

        //Domain Validations
        User.ValidateRole(role);
        if (!IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email format.");
        }
        if (name.Any(ch => _specialCharacters.Contains(ch)) || surname.Any(ch => _specialCharacters.Contains(ch)))
        {
            throw new ArgumentException("Name and surname cannot contain special characters.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = $"{name.Trim()} {surname.Trim()}",
            Email = email,
            Role = role,
            PasswordHash = _hasherService.Hash(password)
        };

        await _userRepository.InsertAsync(user);
        await _userRepository.SaveAsync();
        return user;
    }

    public async Task<User?> LoginUser(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var normalizedEmail = email.Trim();
        var user = await _userRepository.GetAsync(u => u.Email == normalizedEmail);

        if (user == null)
        {
            return null;
        }

        var isValidPassword = _hasherService.Verify(user.PasswordHash, password);
        if (isValidPassword)
        {
            return user;
        }

        // Backward compatibility for legacy seeded users that stored plain text passwords.
        if (user.PasswordHash == password)
        {
            user.PasswordHash = _hasherService.Hash(password);
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
            return user;
        }

        return null;
    }

    public async Task<User> UpdateUser(string username, string? password, string? name, string? surname, string? email, Enums.UserRole? role)
    {
        var user = await GetUserByName(username);
        if (user == null) throw new ArgumentException("User cannot be null.");

        if (!string.IsNullOrWhiteSpace(password))
        {
            user.PasswordHash = _hasherService.Hash(password);
        }

        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
        {
            if (name.Any(ch => _specialCharacters.Contains(ch)) || surname.Any(ch => _specialCharacters.Contains(ch)))
            {
                throw new ArgumentException("Name and surname cannot contain special characters.");
            }
            user.Name = $"{name.Trim()} {surname.Trim()}";
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            if (!IsValidEmail(email)) throw new ArgumentException("Invalid email format.");
            user.Email = email;
        }

        if (role.HasValue)
        {
            User.ValidateRole(role.Value);
            user.Role = role.Value;
        }

        _userRepository.Update(user);
        await _userRepository.SaveAsync();
        return user;
    }

    public async Task DeleteUser(string username)
    {
        var user = await GetUserByName(username);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }
        _userRepository.Delete(user);
        await _userRepository.SaveAsync();
    }

    public async Task<bool> UserExists(string name)
    {
        var normalizedName = NormalizeName(name);
        return await _userRepository.ExistsAsync(u => u.Name == normalizedName);
    }


    public async Task<User> GetUserByName(string name)
    {
        var normalizedName = NormalizeName(name);
        var user = await _userRepository.GetAsync(u => u.Name == normalizedName);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        return user;
    }

    public async Task<IList<User>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return users.OrderBy(u => u.Name).ToList();
    }

    private bool IsValidEmail(string email)
    {
        var mailAddress = new EmailAddressAttribute();
        return mailAddress.IsValid(email);
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        }
        return name.Trim();
    }
}