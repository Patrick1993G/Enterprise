using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class OrderViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime DatePlaced { get; set; }
        [Required]
        public String UserEmail { get; set; }
    }
}
