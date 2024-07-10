using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zencareservice.Models
{
	public class AppUser : IdentityUser
	{
		public int MyProperty { get; set; }

	}
}
