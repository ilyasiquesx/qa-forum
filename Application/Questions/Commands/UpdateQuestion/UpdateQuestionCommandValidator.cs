using FluentValidation;

namespace QAForum.Application.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator()
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