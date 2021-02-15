using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using QAForum.Application.Answers.Commands.CreateAnswer;
using QAForum.Application.Questions.Commands.CreateQuestion;
using QAForum.Application.Questions.Queries.GetQuestion;
using Xunit;

namespace WebApi.IntegrationTests
{
    public class QuestionTests : IntegrationTest
    {
        private const string Question = "Question";
        private const string Answer = "Answer";

        [Fact]
        public async Task CreateQuestion_ShouldReturnQuestionId_WhenDataValid()
        {
            // Arrange
            await AuthenticateTestUser(nameof(CreateQuestion_ShouldReturnQuestionId_WhenDataValid));
            var question = new CreateQuestionCommand
            {
                Title = "What is the difference between a reference type and value type in C#?",
                Content = "Can you explain it to me in a professional way?"
            };

            // Act
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);
            var createQuestionResponseModel =
                await serverResponse.Content.ReadFromJsonAsync<CreateQuestionCommandResponse>();

            // Assert
            createQuestionResponseModel.Should().NotBeNull();
            createQuestionResponseModel!.Id.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("", "qwerty")]
        [InlineData("qwerty", "")]
        [InlineData("", "")]
        public async Task CreateQuestion_ShouldReturnBadRequest_WhenBodyIsEmpty(string title, string content)
        {
            // Arrange
            await AuthenticateTestUser($"{nameof(CreateQuestion_ShouldReturnBadRequest_WhenBodyIsEmpty)}-{title}-{content}");
            var question = new CreateQuestionCommand
            {
                Title = string.Empty,
                Content = string.Empty
            };

            // Act
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);

            // Assert
            serverResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateQuestion_ShouldReturnUnauthorized_WhenNotAuthenticated()
        {
            // Arrange
            var question = new CreateQuestionCommand
            {
                Title = "Is there a difference between authentication and authorization?",
                Content = "I see these two terms bandied about quite a bit"
            };

            // Act
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);

            // Assert
            serverResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetQuestion_ShouldReturnAuthorId_WhenQuestionExist()
        {
            // Arrange
            var userId = await AuthenticateTestUser(nameof(GetQuestion_ShouldReturnAuthorId_WhenQuestionExist));
            var question = new CreateQuestionCommand
            {
                Title = "What is the difference between constants and read-only?",
                Content = "When would you use one over the other?"
            };
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);
            var createQuestionResponseModel =
                await serverResponse.Content.ReadFromJsonAsync<CreateQuestionCommandResponse>();

            // Act
            serverResponse = await HttpClient.GetAsync($"{Question}/{createQuestionResponseModel!.Id}");
            var getQuestionResponseModel =
                await serverResponse.Content.ReadFromJsonAsync<GetQuestionQueryResponse>();

            // Assert
            getQuestionResponseModel.Should().NotBeNull();
            getQuestionResponseModel!.CreatedBy.Id.Should().Be(userId);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        public async Task GetQuestion_ShouldReturnValidAnswersCount_WhenAnswersCreated(int count)
        {
            // Arrange
            await AuthenticateTestUser(
                $"{nameof(GetQuestion_ShouldReturnValidAnswersCount_WhenAnswersCreated)}-{count}");
            var question = new CreateQuestionCommand
            {
                Title = "What is the difference between constants and read-only?",
                Content = "When would you use one over the other?"
            };
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);
            var createQuestionResponseModel =
                await serverResponse.Content.ReadFromJsonAsync<CreateQuestionCommandResponse>();

            for (var i = 0; i < count; i++)
            {
                var answer = new CreateAnswerCommand
                {
                    QuestionId = createQuestionResponseModel!.Id,
                    Content = "No difference for real"
                };
                await HttpClient.PostAsJsonAsync(Answer, answer);
            }

            // Act
            serverResponse = await HttpClient.GetAsync($"{Question}/{createQuestionResponseModel!.Id}");
            var getQuestionResponseModel =
                await serverResponse.Content.ReadFromJsonAsync<GetQuestionQueryResponse>();

            // Assert
            getQuestionResponseModel.Should().NotBeNull();
            getQuestionResponseModel!.Answers.Should().HaveCount(count);
        }


        [Fact]
        public async Task DeleteQuestion_ShouldReturnForbidden_WhenRequestNotFromAuthor()
        {
            // Arrange
            await AuthenticateTestUser("iAmTheAuthor" +
                                       nameof(DeleteQuestion_ShouldReturnForbidden_WhenRequestNotFromAuthor));
            var question = new CreateQuestionCommand
            {
                Title = "Help me with my account protection!",
                Content = "Please..."
            };
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);
            var createdQuestionResponse =
                await serverResponse.Content.ReadFromJsonAsync<CreateQuestionCommandResponse>();

            // Act
            await AuthenticateTestUser("iAmTheDataStealer" +
                                       nameof(DeleteQuestion_ShouldReturnForbidden_WhenRequestNotFromAuthor));
            serverResponse = await HttpClient.DeleteAsync($"{Question}/{createdQuestionResponse!.Id}");

            // Assert
            serverResponse.IsSuccessStatusCode.Should().BeFalse();
            serverResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteQuestion_ShouldReturnNoContent_WhenRequestFromAuthor()
        {
            // Arrange
            await AuthenticateTestUser(nameof(DeleteQuestion_ShouldReturnNoContent_WhenRequestFromAuthor));
            var question = new CreateQuestionCommand
            {
                Title = "Help me with my account protection!",
                Content = "Please..."
            };
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);
            var createdQuestionResponse =
                await serverResponse.Content.ReadFromJsonAsync<CreateQuestionCommandResponse>();

            // Act
            serverResponse = await HttpClient.DeleteAsync($"{Question}/{createdQuestionResponse!.Id}");

            // Assert
            serverResponse.IsSuccessStatusCode.Should().BeTrue();
            serverResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteThenGetQuestion_ShouldReturnNotFound_AfterQuestionDeletion()
        {
            // Arrange
            await AuthenticateTestUser(nameof(DeleteThenGetQuestion_ShouldReturnNotFound_AfterQuestionDeletion));
            var question = new CreateQuestionCommand
            {
                Title = "Question to delete",
                Content = "bye"
            };
            var serverResponse = await HttpClient.PostAsJsonAsync(Question, question);
            var createdQuestionResponse =
                await serverResponse.Content.ReadFromJsonAsync<CreateQuestionCommandResponse>();
            await HttpClient.DeleteAsync($"{Question}/{createdQuestionResponse!.Id}");

            // Act
            serverResponse = await HttpClient.GetAsync($"{Question}/{createdQuestionResponse.Id}");

            // Assert
            serverResponse.IsSuccessStatusCode.Should().BeFalse();
            serverResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}