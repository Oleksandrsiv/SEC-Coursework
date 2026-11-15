using FluentAssertions;
using KPI_course_work.Models;
using KPI_course_work.Services;

namespace KPI_course_work.Tests.Services
{
    public class TestValidatorTests
    {
        private readonly ITestValidator _sut;

        public TestValidatorTests()
        {
            _sut = new TestValidator();
        }

        [Fact]
        public void IsTestValidForPublishing_NoQuestions_ReturnsFalse()
        {
            // Arrange
            var test = new Test { Questions = [] };

            // Act
            var isValid = _sut.IsTestValidForPublishing(test, out var error);

            // Assert
            isValid.Should().BeFalse();
            error.Should().Contain("at least one question");
        }

        [Fact]
        public void IsTestValidForPublishing_SingleChoiceWithMultipleCorrect_ReturnsFalse()
        {
            // Arrange
            var test = new Test
            {
                Questions =
                [
                    new Question
                    {
                        Type = QuestionType.SingleChoice,
                        Text = "Invalid Single Choice",
                        AnswerOptions =
                        [
                            new AnswerOption { Id = 1, IsCorrect = true },
                            new AnswerOption { Id = 2, IsCorrect = true } // Second correct answer breaks the rule
                        ]
                    }
                ]
            };

            // Act
            var isValid = _sut.IsTestValidForPublishing(test, out var error);

            // Assert
            isValid.Should().BeFalse();
            error.Should().Contain("multiple correct answers");
        }
    }
}