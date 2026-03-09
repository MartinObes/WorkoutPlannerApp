using System.ComponentModel.DataAnnotations;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Users;

public class UserLogic : IUserLogic
{
    private string _specialCharacters = "!@#$%^&*()_+[]{}|;:',.<>?/`~-=";
    
    public User CreateUser(string password, string name, string surname, string email, Enums.UserRole role)
    {
        //null validations
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be empty.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Surname cannot be empty.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
        
        //Domain Validations
        User.ValidateRole(role);
        if(!IsValidEmail(email)) throw new ArgumentException("Invalid email format.");
        if(name.Any(ch => _specialCharacters.Contains(ch))|| surname.Any(ch => _specialCharacters.Contains(ch)))
            throw new ArgumentException("Name and surname cannot contain special characters.");
        
        return new User
        {
            Id = Guid.NewGuid(),
            Name = $"{name.Trim()} {surname.Trim()}",
            Email = email,
            Role = role,
            PasswordHash = password
        };
    }
    
    public User UpdateUser(User user, string? password, string? name, string? surname, string? email, Enums.UserRole? role)
    {
        if (user == null) throw new ArgumentException("User cannot be null.");
        
        if (!string.IsNullOrWhiteSpace(password))
        {
            user.PasswordHash = password;
        }
        
        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
        {
            if(name.Any(ch => _specialCharacters.Contains(ch))|| surname.Any(ch => _specialCharacters.Contains(ch)))
                throw new ArgumentException("Name and surname cannot contain special characters.");
            user.Name = $"{name.Trim()} {surname.Trim()}";
        }
        
        if (!string.IsNullOrWhiteSpace(email))
        {
            if(!IsValidEmail(email)) throw new ArgumentException("Invalid email format.");
            user.Email = email;
        }
        
        if (role.HasValue)
        {
            User.ValidateRole(role.Value);
            user.Role = role.Value;
        }

        return user;
    }
    
    public void DeleteUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null.");
        // Logic to delete user from database or collection
    }

    private bool IsValidEmail(string email)
    {
        var mailAddress =  new EmailAddressAttribute();
        return mailAddress.IsValid(email);
    }
}