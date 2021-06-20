using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Web.Models
{
    // poco class for contact dto, to simplify things for this project,
    // two views may have different requirements, but it seems our Edit/Create views ask for the same fields
    public class dtoContact
    {
        public int Id { get; set; }

        [Display(Name="Name")]
        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(50, ErrorMessage ="Contact name should be between 5 and 50 characters", MinimumLength = 5)]
        public string Name { get; set; }

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Title is a required field.")]
        [StringLength(50, ErrorMessage = "Contact title should be between 3 and 50 characters", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Company")]
        [Required(ErrorMessage = "Company is a required field.")]
        [StringLength(50, ErrorMessage ="Contact company should be less than 50 characters")]
        public string Company { get; set; }

        [Display(Name = "Phone")]
        [Phone]
        [StringLength(50)]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        [StringLength(100)]
        public string Address { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Display(Name = "Last Date Contacted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime LastDateContacted { get; set; }

        [Display(Name = "Comments")]
        [StringLength(512, ErrorMessage ="Comments should be less than 512 characters")]
        public string Comments { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }

        public dtoContact()
        {
            Id = 0;
            Name = string.Empty;
            Title = string.Empty;
            Company = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
            Email = string.Empty;
            LastDateContacted = DateTime.Today;
        }
    }
}