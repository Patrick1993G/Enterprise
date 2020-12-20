using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class MemberViewModel
    {
        [Required(ErrorMessage = "Please input an email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please input a first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please input a last name")]
        public string LastName { get; set; }
    }
}
