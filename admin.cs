
using System.Security.Cryptography;
using System.Text;

namespace HealthCareSystem;

public static class Password
{
    public static (string Hash, string Salt) HarshPassword(string password)
    {
        string saltString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        string passwordAndSalt = password + saltString;
        byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(passwordAndSalt);

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] hashBytes = sha256Hash.ComputeHash(passwordAndSaltBytes);
            return (hashString, saltString);
        }
    }

    public static bool VerifyPassword(string password, stirng storeHash, string storeSalt)
    {
        string passwordAndSalt = password + storeSalt;
        byte[] passwordandSaltBytes = Encoding.UTF8.GetBytes(passwordAndSalt);

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] hashBytes = sha256Hash.ComputeHash(passwordandSaltBytes);
            string ComputeHash = Convert.ToBase64String(hashBytes);

            return ComputeHash == storeHash;
        }
    }
}
