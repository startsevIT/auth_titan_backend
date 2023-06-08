using System.ComponentModel.DataAnnotations;

namespace auth_titan_backend.Models
{
	public class RegisterViewModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get;set; }
		public string ReturnUrl { get; set; }

	}
}
