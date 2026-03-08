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
            Name = $"{name.Trim()} {surname.Trim()}",
            Email = email,
            Role = role,
            PasswordHash = password
        };
    }
    

    private bool IsValidEmail(string email)
    {
        var mailAddress =  new EmailAddressAttribute();
        return mailAddress.IsValid(email);
    }
}