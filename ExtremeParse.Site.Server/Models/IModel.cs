namespace TryParse.Models
{
    public interface IModel
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public DateTime DateTime { get; protected set; }
        public string Creator { get; protected set; }
    }
}
