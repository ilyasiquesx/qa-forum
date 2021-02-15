using FluentValidation;

namespace QAForum.Application.Answers.Commands.CreateAnswer
{
    public class CreateAnswerValidator : AbstractValidator<CreateAnswerCommand>
    {
        public CreateAnswerValidator()
        {
            RuleFor(a => a.Content)
                .NotEmpty()
                .MaximumLength(2048);
        }
    }
}