using System.Security.Cryptography;
using System.Text;

namespace FaturaTakip.Utils;

public class HashingHelper
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmc = new HMACSHA512())
        {
            passwordSalt = hmc.Key;
            passwordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public static bool VerifyPassowrd(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmc = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                {
                    return false;
                }
            }
        }

        return true;
    }
}