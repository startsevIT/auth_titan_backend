using System.ComponentModel.DataAnnotations;

namespace auth_titan_backend.Models
{
	public class LoginViewModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public EmailAddressAttribute Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
