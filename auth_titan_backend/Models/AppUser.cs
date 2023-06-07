using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace auth_titan_backend.Models
{
	public class AppUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string SurName { get; set; }
		public DateOnly BirthDate { get; set; }
		public string Division { get; set; }
		public string Post { get; set; }
		public EmailAddressAttribute Email { get; set; }
		public PhoneAttribute Phone { get; set; }
		
	}
}
