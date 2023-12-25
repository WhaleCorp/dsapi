using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Tables
{
	[Table("Ads")]
	public class Ads
	{
		public Ads(string orientation,string photo)
		{
			this.Orientation = orientation;
			this.Photo = photo;
		}

		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		public string Orientation { get; set; }
		public string Photo { get; set; }
	}
}

