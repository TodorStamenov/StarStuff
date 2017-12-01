namespace StarStuff.Services.Admin.Models.Roles
{
    using StarStuff.Common.Mapping;
    using StarStuff.Data.Models;

    public class RoleServiceModel : IMapFrom<Role>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}