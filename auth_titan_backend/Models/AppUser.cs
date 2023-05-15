using Microsoft.AspNetCore.Identity;

namespace auth_titan_backend.Models
{
	public class AppUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
