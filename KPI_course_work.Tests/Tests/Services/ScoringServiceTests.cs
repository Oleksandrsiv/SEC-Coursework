using FluentAssertions;
using FsCheck.Xunit;
using KPI_course_work.Models;
using KPI_course_work.Services;
using KPI_course_work.Tests.Generators;

namespace KPI_course_work.Tests.Services
{
    public class ScoringServiceTests
    {
        private readonly IScoringService _sut;
        private readonly Dictionary<int, List<int>> _emptyAnswers;

        public ScoringServiceTests()
        {
            _sut = new ScoringService();
            _emptyAnswers = [];
        }

        // P2: Інваріант - Вихідна оцінка завжди має перебувати в межах допустимого математичного діапазону.
        [Property(Arbitrary = new[] { typeof(TestDataGen) })] // Reverted to classic array syntax for xUnit discovery
        public void CalculateScore_ScoreIsAlwaysWithinValidRange(Test test)
        {
            var result = _sut.CalculateScore(test, _emptyAnswers);

            result.Score.Should().BeInRange(0, result.MaxScore);
        }

        // P3: Ідемпотентність - Виконання розрахунку двічі дає абсолютно той самий результат.
        [Property(Arbitrary = new[] { typeof(TestDataGen) })]
        public void CalculateScore_CalledTwice_ReturnsIdenticalResults(Test test)
        {
            var firstResult = _sut.CalculateScore(test, _emptyAnswers);
            var secondResult = _sut.CalculateScore(test, _emptyAnswers);

            firstResult.Should().BeEquivalentTo(secondResult);
        }

        // P8: Без винятків - Коректні об'єкти доменної моделі в жодному разі не повинні призводити до збою в роботі сервісу.
        [Property(Arbitrary = new[] { typeof(TestDataGen) })]
        public void CalculateScore_ValidInput_NeverThrowsException(Test test)
        {
            Action act = () => _sut.CalculateScore(test, _emptyAnswers);

            act.Should().NotThrow();
        }

        // P6: Метаморфічна - Зміна вхідних даних у відомий спосіб призводить до передбачуваної зміни вихідних даних.
        [Property(Arbitrary = new[] { typeof(TestDataGen) })]
        public void CalculateScore_SkipOneQuestion_DecreasesScoreByQuestionPoints(Test test)
        {
            // Arrange
            var perfectAnswers = new Dictionary<int, List<int>>();
            foreach (var q in test.Questions!)
            {
                var correctIds = q.AnswerOptions.Where(a => a.IsCorrect).Select(a => a.Id).ToList();
                perfectAnswers[q.Id] = correctIds;
            }

            var perfectResult = _sut.CalculateScore(test, perfectAnswers);

            var firstQuestion = test.Questions!.First();

            // Act
            var imperfectAnswers = new Dictionary<int, List<int>>(perfectAnswers);
            imperfectAnswers.Remove(firstQuestion.Id);

            var imperfectResult = _sut.CalculateScore(test, imperfectAnswers);

            // Assert
            imperfectResult.Score.Should().BeApproximately(perfectResult.Score - firstQuestion.Points, 0.01);
        }
    }
}