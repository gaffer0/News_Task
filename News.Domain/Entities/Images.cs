using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace News.Domain.Entities
{
    public class Images
    {
        [Key]
        public int ImageId { get; set; }
        [ForeignKey("New")]
        public int NewsId { get; set; }
        public string ImagePath { get; set; }
        [JsonIgnore]
        public New New { get; set; }
    }
}
