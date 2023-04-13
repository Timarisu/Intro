using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Intro.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }
    }
}