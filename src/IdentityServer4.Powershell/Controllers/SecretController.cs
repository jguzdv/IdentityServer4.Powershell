using System.Security.Cryptography;
using System.Text;

namespace IdentityServer4.Powershell.Controllers
{
    static class SecretController
    {
        internal static string GenerateKey(int size)
        {
            if (size <= 0 || size > 512)
                throw new System.ArgumentOutOfRangeException(nameof(size), "Size should be 0 < x <= 512");

            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!/()[]\\+*~#,;.-_".ToCharArray();

            byte[] data = new byte[size];
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
            }
            var result = new StringBuilder(size);

            foreach (byte b in data)
                result.Append(chars[b % (chars.Length)]);

            return result.ToString();
        }
    }
}
