using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

using Business.Identity.Models;
using MedioClinic.Attributes;
using MedioClinic.Models.Account;

namespace MedioClinic.Models.Profile
{
    public class CommonUserViewModel
    {
        [Display(Name = "Models.Profile.CommonUserViewModel.Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Models.Profile.CommonUserViewModel.FirstName")]
        [MaxLength(100, ErrorMessage = "Models.MaxLength")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Models.Profile.CommonUserViewModel.LastName")]
        [MaxLength(100, ErrorMessage = "Models.MaxLength")]
        public string LastName { get; set; }

        [Display(Name = "Models.Profile.CommonUserViewModel.FullName")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Models.Profile.CommonUserViewModel.DateOfBirth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "Models.Profile.CommonUserViewModel.City")]
        public string City { get; set; }

        [Display(Name = "Models.Profile.CommonUserViewModel.Street")]
        public string Street { get; set; }

        public EmailViewModel EmailViewModel { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Models.PhoneFormat")]
        public string Phone { get; set; }

        [Display(Name = "Models.Profile.CommonUserViewModel.Nationality")]
        public string Nationality { get; set; }

        [HiddenInput]
        public string AvatarContentPath { get; set; }

        [Display(Name = "Models.Profile.CommonUserViewModel.AvatarFile")]
        [DataType(DataType.Upload)]
        [HttpPostedFileExtensions(ErrorMessage = "Models.AllowedExtensions")]
        public HttpPostedFileBase AvatarFile { get; set; }
    }
}