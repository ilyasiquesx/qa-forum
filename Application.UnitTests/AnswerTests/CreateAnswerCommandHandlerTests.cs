using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using QAForum.Application.Answers.Commands.CreateAnswer;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;
using Xunit;

namespace Application.UnitTests.AnswerTests
{
    public class CreateAnswerCommandHandlerTests
    {
        private readonly Mock<IAnswerRepository> _mockAnswerRepository = new();
        private readonly Mock<IQuestionRepository> _mockQuestionRepository = new();
        private readonly CreateAnswerCommandHandler _handler;

        public CreateAnswerCommandHandlerTests()
        {
            var mapperCfg = new MapperConfiguration(cfg => { cfg.CreateMap<CreateAnswerCommand, Answer>(); });

            _handler = new CreateAnswerCommandHandler(_mockAnswerRepository.Object,
                _mockQuestionRepository.Object,
                new Mapper(mapperCfg));
        }

        [Fact]
        public async Task CreateHandler_ShouldReturnRelatedQuestionId_WhenAnswerCreated()
        {
            // Arrange
            _mockQuestionRepository.Setup(qr => qr.DoesExistAsync(It.IsAny<long>())).ReturnsAsync(true);

            var request = new CreateAnswerCommand
            {
                Content = "Qwerty",
                QuestionId = 2
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.IsSucceeded.Should().BeTrue();
            response.Data.QuestionId.Should().Be(request.QuestionId);
        }

        [Fact]
        public async Task CreateHandler_ShouldReturnNotFoundReason_WhenQuestionDoesntExist()
        {
            // Arrange
            _mockQuestionRepository.Setup(q => q.DoesExistAsync(It.IsAny<long>())).ReturnsAsync(false);

            var request = new CreateAnswerCommand
            {
                Content = "NotQwerty",
                QuestionId = 2
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.IsSucceeded.Should().BeFalse();
            response.ErrorReason.Should().Be(ErrorReason.NotFound);
        }
    }
}