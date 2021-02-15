using AutoMapper;
using QAForum.Application.Answers.Commands.CreateAnswer;
using QAForum.Application.Answers.Commands.UpdateAnswer;
using QAForum.Application.Answers.Queries.GetAnswer;
using QAForum.Application.Questions.Commands.CreateQuestion;
using QAForum.Application.Questions.Commands.UpdateQuestion;
using QAForum.Application.Questions.Models;
using QAForum.Application.Questions.Queries.GetQuestion;
using QAForum.Application.Users.Commands.RegisterCommand;
using QAForum.Application.Users.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Common.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterUserCommand, User>();
            CreateMap<CreateQuestionCommand, Question>();
            CreateMap<UpdateQuestionCommand, Question>();
            CreateMap<Question, GetQuestionQueryResponse>()
                .ForMember(qr => qr.CreatedBy,
                    opt => opt.MapFrom(q => q.Created));
            CreateMap<User, UserViewModel>();
            CreateMap<User, UserViewModelExtended>()
                .BeforeMap((s, d) => d.QuestionsCount = s.Answers.Count)
                .BeforeMap((s, d) => d.AnswersCount = s.Answers.Count);
            CreateMap<Answer, GetAnswerQueryResponse>()
                .ForMember(qr => qr.CreatedBy,
                    opt => opt.MapFrom(q => q.Created));
            CreateMap<Question, QuestionIndexModel>()
                .BeforeMap((s, d) => d.AnswersCount = s.Answers.Count)
                .ForMember(qr => qr.CreatedBy,
                    opt => opt.MapFrom(q => q.Created));
            CreateMap<CreateAnswerCommand, Answer>();
            CreateMap<UpdateAnswerCommand, Answer>();
        }
    }
}