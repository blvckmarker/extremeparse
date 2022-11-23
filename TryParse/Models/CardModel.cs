using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace TryParse.Models
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
        public string? Description { get; set; }
        public string? Number { get; set; }
        public string? Info { get; set; }

        public string? Photo
        {
            get => photo ?? "https://gp2dzm.ru/wp-content/uploads/2018/11/no-photo-male.jpg";
            set { photo = value; }
        }

        public DateTime DateTime { get; set; }
        public string DateTimeParse
        {
            set
            {
                OnDateTimeChange?.Invoke(value, EventArgs.Empty);
            }
        }

        public override string ToString() => String.Concat(Name, Description, Number, Info);

        public string Creator { get; set; }

    }
}
