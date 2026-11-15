using KPI_course_work.Models;

namespace KPI_course_work.Services
{
    public interface ITestValidator
    {
        bool IsTestValidForPublishing(Test test, out string errorMessage);
    }

    public class TestValidator : ITestValidator
    {
        public bool IsTestValidForPublishing(Test test, out string errorMessage)
        {
            if (test.Questions == null || !test.Questions.Any())
            {
                errorMessage = "The test must contain at least one question.";
                return false;
            }

            foreach (var question in test.Questions)
            {
                if (question.AnswerOptions == null || question.AnswerOptions.Count < 2)
                {
                    errorMessage = $"Question '{question.Text}' must have at least 2 answer options.";
                    return false;
                }

                if (!question.AnswerOptions.Any(a => a.IsCorrect))
                {
                    errorMessage = $"Question '{question.Text}' does not have a correct answer.";
                    return false;
                }

                if (question.Type == QuestionType.SingleChoice && question.AnswerOptions.Count(a => a.IsCorrect) > 1)
                {
                    errorMessage = $"Question '{question.Text}' is SingleChoice but has multiple correct answers.";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}