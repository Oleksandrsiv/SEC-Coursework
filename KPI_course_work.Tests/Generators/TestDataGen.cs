using FsCheck;
using FsCheck.Fluent;
using KPI_course_work.Models;

namespace KPI_course_work.Tests.Generators
{
    public static class TestDataGen
    {
        // Generates a valid Test object
        public static Arbitrary<Test> ArbTest()
        {
            var gen = from questionsCount in Gen.Choose(1, 10)
                      from questions in Gen.ArrayOf(ArbQuestion().Generator, questionsCount)
                      select new Test
                      {
                          Id = 1,
                          Title = "Auto-Generated Test",
                          ShowAnswersAtEnd = true,
                          Questions = questions.ToList()
                      };
            
            return Arb.From(gen);
        }

        // Generates a valid Question object with options
        public static Arbitrary<Question> ArbQuestion()
        {
            var gen = from type in Gen.Elements(QuestionType.SingleChoice, QuestionType.MultipleChoice)
                      from optionsCount in Gen.Choose(2, 5) // At least 2 answer options
                      select GenerateValidQuestion(type, optionsCount);
            
            return Arb.From(gen);
        }

        // Helper to assemble answer options
        private static Question GenerateValidQuestion(QuestionType type, int optionsCount)
        {
            // Ensure at least one correct option exists
            var options = new List<AnswerOption>
            {
                new AnswerOption { Id = 1, Text = "Correct Option", IsCorrect = true }
            };

            for (int i = 1; i < optionsCount; i++)
            {
                // MultipleChoice gets additional correct options, SingleChoice gets only false
                bool isCorrect = type == QuestionType.MultipleChoice && i % 2 == 0;
                options.Add(new AnswerOption { Id = i + 1, Text = $"Option {i}", IsCorrect = isCorrect });
            }

            return new Question
            {
                Id = Guid.NewGuid().GetHashCode(), 
                Text = "Generated Question?",
                Type = type,
                Points = type == QuestionType.SingleChoice ? 1 : 2,
                AnswerOptions = options
            };
        }
    }
}