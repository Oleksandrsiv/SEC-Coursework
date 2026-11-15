using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KPI_course_work.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }

        [Required(ErrorMessage = "The text of the variant is required.")]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; } = false;
    }
}