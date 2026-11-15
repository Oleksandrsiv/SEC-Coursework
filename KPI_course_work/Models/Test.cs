using System.ComponentModel.DataAnnotations;

namespace KPI_course_work.Models
{
    public class Test
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the test is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        // Modifier: true – show all questions and the result at the end; 
        // false – show the result immediately after each question
        public bool ShowAnswersAtEnd { get; set; } = true;

        public Guid UniqueLink { get; set; } = Guid.NewGuid();


        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}