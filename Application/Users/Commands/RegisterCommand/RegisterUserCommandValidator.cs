using FluentValidation;

namespace QAForum.Application.Users.Commands.RegisterCommand
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private const string ConfirmPasswordMessage = "Confirm password field should match with password field";

        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.Password)
                .NotEmpty()
                .Equal(x => x.ConfirmPassword)
                .WithMessage(ConfirmPasswordMessage);
            RuleFor(x => x.Email)
                .NotEmpty();
        }
    }
}