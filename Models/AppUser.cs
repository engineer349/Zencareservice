using Microsoft.AspNetCore.Identity;

namespace Zencareservice.Models
{
	public class AppUser : IdentityUser
	{
		public int MyProperty { get; set; }

	}
}
