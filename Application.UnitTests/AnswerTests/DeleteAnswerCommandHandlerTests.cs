using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using QAForum.Application.Answers.Commands.DeleteAnswer;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Application.Common.Services;
using QAForum.Domain.Entities;
using Xunit;

namespace Application.UnitTests.AnswerTests
{
    public class DeleteAnswerCommandHandlerTests
    {
        private readonly DeleteAnswerCommandHandler _handler;
        private readonly Mock<IAnswerRepository> _mockAnswerRepository;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;

        public DeleteAnswerCommandHandlerTests()
        {
            _mockAnswerRepository = new Mock<IAnswerRepository>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            var accessValidator = new AccessValidator();
            _handler = new DeleteAnswerCommandHandler(
                _mockCurrentUserService.Object,
                _mockAnswerRepository.Object,
                accessValidator);
        }

        [Fact]
        public async Task DeleteHandler_ShouldReturnNoAccessReason_WhenRequestNotFromAuthor()
        {
            // Arrange
            var answer = new Answer
            {
                CreatedBy = "019505af-4ce9-41f9-beb5-e83155523e98",
            };
            _mockAnswerRepository.Setup(a => a.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(answer);

            const string otherUserId = "2b8ff508-68a1-4a22-8497-e5e373567a77";
            _mockCurrentUserService.Setup(u => u.UserId).Returns(otherUserId);

            var deleteCommand = new DeleteAnswerCommand
            {
                Id = 4
            };

            // Act
            var result = await _handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsSucceeded.Should().BeFalse();
            result.ErrorReason.Should().Be(ErrorReason.HaveNoAccess);
        }

        [Fact]
        public async Task DeleteHandler_ShouldReturnSuccessfulResult_WhenRequestFromAuthor()
        {
            // Arrange
            var answer = new Answer
            {
                CreatedBy = "a62b5893-acca-422b-98d1-34784abf1069",
            };
            _mockAnswerRepository.Setup(a => a.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(answer);

            const string authorUserId = "a62b5893-acca-422b-98d1-34784abf1069";
            _mockCurrentUserService.Setup(u => u.UserId).Returns(authorUserId);

            var deleteCommand = new DeleteAnswerCommand
            {
                Id = 4
            };

            // Act
            var result = await _handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteHandler_ShouldReturnNotFoundReason_WhenAnswerDoesntExist()
        {
            // Arrange
            _mockAnswerRepository.Setup(a => a.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null);

            var deleteCommand = new DeleteAnswerCommand
            {
                Id = 4
            };

            // Act
            var result = await _handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.IsSucceeded.Should().BeFalse();
            result.ErrorReason.Should().Be(ErrorReason.NotFound);
        }
    }
}