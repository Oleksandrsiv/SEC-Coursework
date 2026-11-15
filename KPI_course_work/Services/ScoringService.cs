using KPI_course_work.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KPI_course_work.Services
{
    public class TestResult
    {
        public double Score { get; set; }
        public double MaxScore { get; set; }
        public double Percentage { get; set; }
        public bool IsPassed { get; set; }
    }

    public interface IScoringService
    {
        TestResult CalculateScore(Test test, Dictionary<int, List<int>> userAnswers, int passingPercentage = 60);
    }

    public class ScoringService : IScoringService
    {
        public TestResult CalculateScore(Test test, Dictionary<int, List<int>> userAnswers, int passingPercentage = 60)
        {
            double totalScore = 0;
            // Max score is now the sum of all question points
            double maxScore = test.Questions?.Sum(q => q.Points) ?? 0;

            if (maxScore == 0)
            {
                return new TestResult { Score = 0, MaxScore = 0, Percentage = 0, IsPassed = false };
            }

            foreach (var question in test.Questions!)
            {
                var correctOptionIds = question.AnswerOptions
                    .Where(a => a.IsCorrect)
                    .Select(a => a.Id)
                    .ToList();

                if (!userAnswers.TryGetValue(question.Id, out var selectedIds) || !selectedIds.Any())
                {
                    continue; // No answers provided for this question
                }

                if (question.Type == QuestionType.SingleChoice)
                {
                    // Strict scoring: exact match required
                    if (selectedIds.Count == 1 && correctOptionIds.Contains(selectedIds.First()))
                    {
                        totalScore += question.Points;
                    }
                }
                else if (question.Type == QuestionType.MultipleChoice && correctOptionIds.Any())
                {
                    // Partial scoring with penalty for incorrect guesses
                    double weightPerOption = (double)question.Points / correctOptionIds.Count;
                    
                    int correctSelectedCount = selectedIds.Count(id => correctOptionIds.Contains(id));
                    int incorrectSelectedCount = selectedIds.Count(id => !correctOptionIds.Contains(id));

                    double questionEarned = (correctSelectedCount * weightPerOption) - (incorrectSelectedCount * weightPerOption);
                    
                    // Prevent negative scores for a single question
                    totalScore += Math.Max(0, questionEarned);
                }
            }

            double percentage = Math.Round(totalScore / maxScore * 100, 2);
            
            return new TestResult
            {
                Score = Math.Round(totalScore, 2), // Rounding to 2 decimals for clean UI
                MaxScore = maxScore,
                Percentage = percentage,
                IsPassed = percentage >= passingPercentage
            };
        }
    }
}