using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Model
{
	public class Rating
	{
		public int Id { get; set; }

        public bool Like { get; set; }

        public bool DisLike { get; set; }

       
      
        public int? CommentId { get; set; }

        public int PostId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CommentId))]
        public Comment? Comment { get; set; }

        public string UserId { get; set; } = "";

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public BlogUser? BlogerUser { get; set; }
    }
}

