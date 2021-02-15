namespace QAForum.Application.Users.Models
{
    public class UserViewModelExtended : UserViewModel
    {
        public int QuestionsCount { get; set; }

        public int AnswersCount { get; set; }
    }
}