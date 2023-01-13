namespace ExtremeParse.Models
{
    public class CardModel : IModel
    {
        private event EventHandler OnDateTimeChange;

        private string photo;

        public CardModel()
        {
            OnDateTimeChange += (object value, EventArgs e) => { this.DateTime = DateTime.Parse(value.ToString()); };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Number { get; set; }
        public string Info { get; set; }

        public string? Photo
        {
            get => photo ?? "https://gp2dzm.ru/wp-content/uploads/2018/11/no-photo-male.jpg";
            set => photo = value;
        }

        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            var TrimName = string.Join("", Name.Where(ch => ch != ' '));
            var TrimDesc = string.Join("", Description.Where(ch => ch != ' '));
            var TrimInfo = string.Join("", Info.Where(ch => ch != ' '));
            string TrimNumber = Number is not null ? string.Join("", Number.Where(ch => ch != '-')) : string.Empty;
            return string.Concat(TrimInfo, TrimName, TrimDesc, TrimNumber ?? string.Empty);
        }

        public string Creator { get; set; }
    }
}
