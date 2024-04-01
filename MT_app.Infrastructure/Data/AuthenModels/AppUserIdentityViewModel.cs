﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MT_app.Infrastructure.Data.AuthenModels
{
    public class AppUserIdentityViewModel 
    {
        [BindProperty] public RegisterInputModel RegisterInput { get; set; }
        [BindProperty] public LoginInputModel LoginInput { get; set; }
        public string ReturnUrl { get; set; }


        public class RegisterInputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress] 
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
        }
    }
}