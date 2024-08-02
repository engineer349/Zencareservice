namespace Zencareservice.Models
{
	public class Bedreg
	{
		public int BedregId { get; set; }

		public string? PatientFirstName { get; set; }

		public string? PatientLastName { get; set; }

		public string? BedType { get; set; }

		public string? RoomType { get; set; }

		public DateOnly? BedRequiredDate { get; set; }
		public TimeOnly? BedRequiredTime { get; set; }

		public string? ReasonType { get; set; }

		public int RequiredDays { get; set; }


	}
}
