using Microsoft.AspNetCore.Mvc;

namespace Zencareservice.Controllers
{
	public class Calendarscheduler : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Calendar()
		{

			return View();
		}
	}
}
