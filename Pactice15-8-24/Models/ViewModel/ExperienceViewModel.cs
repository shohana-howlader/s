using System.ComponentModel.DataAnnotations;

namespace Pactice15_8_24.Models.ViewModel
{
    public class ExperienceViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int Duration { get; set; }
    }
}