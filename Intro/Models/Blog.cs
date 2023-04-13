using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Intro.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Описание")]
        [Required]
        public string Description { get; set; }
        [DisplayName("Изображение")]
        public string Image { get; set; }
        [DisplayName("Создатель")]
        public string Author { get; set; }
        [DisplayName ("Категория")]

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
