using FluentNh.Stereotypes;

namespace FluentNh.Entities
{
    [Component]
    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}