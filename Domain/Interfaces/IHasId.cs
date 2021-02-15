namespace QAForum.Domain.Interfaces
{
    public interface IHasId<T>
    {
        public T Id { get; set; }
    }
}