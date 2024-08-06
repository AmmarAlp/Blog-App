using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; } = "";


        public int? ParentCommentId { get; set; } 

        [JsonIgnore]
        [ForeignKey(nameof(ParentCommentId))]
        public Comment? ParentComment { get; set; }

        public int PostId { get; set; } 

        [JsonIgnore]
        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        public string? UserId { get; set; } 

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public BlogUser? BlogerUser { get; set; }

        public ICollection<Rating>? Ratings { get; set; }

        public ICollection<Comment>? Replies { get; set; }



    }
}
