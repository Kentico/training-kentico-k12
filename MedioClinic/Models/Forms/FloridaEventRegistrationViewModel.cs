using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Business.Repository.Forms;

namespace MedioClinic.Models.Forms
{
    public class FloridaEventRegistrationViewModel : BaseFormViewModel, IFormViewModel, IViewModel
    {
        protected const string RequiredFieldErrorMessage = "This field is required.";
        protected const string TextFieldLengthErrorMessage = "The first name mustn't exceed 200 characters.";

        public IDictionary<string, object> Fields =>
            GetFields(
                new KeyValuePair<string, object>(nameof(FirstName), FirstName),
                new KeyValuePair<string, object>(nameof(LastName), LastName),
                new KeyValuePair<string, object>(nameof(EmailInput), EmailInput)
                );

        [Display(Name = "First name", Prompt = "Enter your given name here")]
        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        [MaxLength(200, ErrorMessage = TextFieldLengthErrorMessage)]
        public string FirstName { get; set; }

        [Display(Name = "Last name", Prompt = "Enter your surname here")]
        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        [MaxLength(200, ErrorMessage = TextFieldLengthErrorMessage)]
        public string LastName { get; set; }

        [Display(Name = "Email", Prompt = "Enter your email address here")]
        [EmailAddress(ErrorMessage = "The value isn't a valid email address.")]
        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        [MaxLength(200, ErrorMessage = TextFieldLengthErrorMessage)]
        public string EmailInput { get; set; }
    }
}