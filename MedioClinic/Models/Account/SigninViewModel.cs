using System.ComponentModel;

namespace MedioClinic.Models.Account
{
    public class SignInViewModel : IViewModel
    {
        public EmailViewModel EmailViewModel { get; set; }

        public PasswordViewModel PasswordViewModel { get; set; }

        [DisplayName("Models.Account.StaySignedIn")]
        public bool StaySignedIn { get; set; }
    }
}