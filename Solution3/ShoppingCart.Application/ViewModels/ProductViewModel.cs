using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="The name is required !")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The price is required !")]
        [Range(typeof(Double),"0","99999",ErrorMessage ="{0} must be a digit between {1} and {2} !")]
        public double Price { get; set; }
        [Required(ErrorMessage = "The description is required !")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The category is required !")]
        public CategoryViewModel Category { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "The stock size is required !")]
        public int Stock { get; set; }
        public bool Disable { get; set; }
    }
}
