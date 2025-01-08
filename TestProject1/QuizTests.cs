using GenericFanSite.Models;

namespace FanSiteTests
{
    public class QuizTests
    {
        Quiz quiz = new Quiz();
        public QuizTests()
        {
            //Arrange
            Question q1 = new Question() { Q = "What did Thomas eat for breakfast?", A = "Cereal", UserA = "Cereal" };
            Question q2 = new Question() { Q = "What did Thomas eat for breakfast?", A = "Cereal", UserA = "Toast" };
            quiz.Questions.Add(q1);
            quiz.Questions.Add(q2);
        }
        [Fact]
        public void CheckCorrectAnswer()
        {
            //Act
            //Assert
            Assert.True(quiz.CheckAnswer(quiz.Questions[2]));
        }
        [Fact]
        public void CheckIncorrectAnswer()
        {
            Assert.False(quiz.CheckAnswer(quiz.Questions[3]));
        }
        [Fact]
        public void CheckNumberQuestions()
        {
            Assert.Equal(4, quiz.Questions.Count);
        }
    }
}