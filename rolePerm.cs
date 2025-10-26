
namespace HealthCareSystem
{
    public static class RolePermissions
    {
        public static readonly Dictionary<Role, List<Permissions>> Map = new()
        {
          {Role.Patient, new List<Permissions> {Permission.None}},
          {Role.Personnel, new List<Permissions> {Permission.None}},
          {Role.Admin, new List<Permissions> {Permission.None}},
          {Role.superAdmin, new List<Permissions> {Permission.None}}
        };
    }
}
