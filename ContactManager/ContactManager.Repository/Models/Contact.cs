using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Repository.Models
{
    public class Contact : ICloneable
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Comments { get; set; }
        // required
        public string Company { get; set; }
        public string Email { get; set; }
        public DateTime LastDateContacted { get; set; }
        // required
        public string Name { get; set; }
        public string Phone { get; set; }
        // required
        public string Title { get; set; }

        public Contact() { }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// try to **simulate** server side validation on the Contact's fields. 
        /// With a DB, these validation rules can be shift to a SP.
        /// The error messages should be in a resource file
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;
            var sbError = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 50)
            {
                // required
                sbError.AppendLine($"Contact name cannot be empty or longer than 50 characters");
            }
            if (!string.IsNullOrWhiteSpace(Address) && Address.Length > 100)
            {
                sbError.AppendLine($"Contact address cannot be longer than 100 characters");
            }
            if (!string.IsNullOrWhiteSpace(Comments) && Comments.Length > 512)
            {
                sbError.AppendLine($"Contact address cannot be longer than 512 characters");
            }
            if (string.IsNullOrWhiteSpace(Company) || Company.Length > 50)
            {
                sbError.AppendLine($"Contact company cannot be longer than 50 characters");
            }
            if (!string.IsNullOrWhiteSpace(Comments) && Comments.Length > 255)
            {
                sbError.AppendLine($"Contact email cannot be longer than 255 characters");
            }
            // we don't have a client side phone # validator, so pretend here
            if (!string.IsNullOrWhiteSpace(Phone) && Phone.Length > 50)
            {
                sbError.AppendLine($"Contact phone cannot be longer than 50 characters");
            }
            if (string.IsNullOrWhiteSpace(Title) || Title.Length > 50)
            {
                // required
                sbError.AppendLine($"Contact title cannot be empty or longer than 50 characters");
            }

            errorMessage = sbError.ToString();
            return string.IsNullOrWhiteSpace(errorMessage);
        }
        // Other model related methods
    }
}
