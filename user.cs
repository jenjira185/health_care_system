
namespace HealthCareSystem;

class User
{
    public string Email;
    public string Password;
    

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public bool TryLogin(string email, string password)
    {
        return email == Email && password == Password;
    }

    public string ToSaveString()
    {
        return $"{Email}, {Password}";
    }
}
