using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Model
{
	public class Post
	{
		public int  Id { get; set; }

		public string Header { get; set; } = "";

		public string Topic { get; set; } = "";

		public string Text { get; set; } = "";

		public DateTime PostDate { get; set; }

		public string UserId { get; set; } = "";

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public BlogUser? BlogerUser { get; set; }

        public ICollection<Comment>? Comments { get; set; }

		public virtual ICollection<Rating>? Ratings { get; set; } = new List<Rating>();


		
    }
}


