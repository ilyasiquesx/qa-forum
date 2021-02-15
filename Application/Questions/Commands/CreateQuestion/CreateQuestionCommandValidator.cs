using FluentValidation;

namespace QAForum.Application.Questions.Commands.CreateQuestion
{
    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2048);
        }
    }
}