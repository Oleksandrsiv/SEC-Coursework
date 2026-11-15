using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KPI_course_work.Models
{
    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice
    }

    public class Question
    {
        public int Id { get; set; }

        public int TestId { get; set; }
        
        [ForeignKey("TestId")]
        public Test? Test { get; set; }

        [Required(ErrorMessage = "The text of the question is required.")]
        public string Text { get; set; } = string.Empty;

        public QuestionType Type { get; set; } = QuestionType.SingleChoice;

        [Range(1, 100)]
        public int Points { get; set; } = 1;

        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}