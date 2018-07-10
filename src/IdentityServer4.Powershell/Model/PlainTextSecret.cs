using IdentityServer4.Models;
using IdentityServer4.Powershell.Controllers;

namespace IdentityServer4.Powershell.Model
{
    public class PlainTextSecret : Secret
    {
        private string _plainText;

        public PlainTextSecret(int length = 48)
        {
            PlainText = SecretController.GenerateKey(length);
        }

        public string PlainText {
            get => _plainText;
            set
            {
                _plainText = value;
                Value = value.Sha256();
            }
        }
    }
}
