﻿namespace EA.Iws.Api.Client.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ApplicantRegistrationData
    {
        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [Required]
        [RegularExpression("^[0-9+\\(\\)#\\.\\s\\/ext-]+$", ErrorMessage = "The entered phone number is invalid")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Guid? AddressId { get; set; }
    }
}