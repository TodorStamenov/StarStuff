namespace StarStuff.Services.Models.Astronomers
{
    using Common.Mapping;
    using Data.Models;

    public class AstronmersServiceModel : IMapFrom<User>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}