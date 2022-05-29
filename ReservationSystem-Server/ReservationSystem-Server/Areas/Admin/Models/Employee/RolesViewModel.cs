using System.ComponentModel;

namespace ReservationSystem_Server.Areas.Admin.Models.Employee;

public class RolesViewModel
{
    [ReadOnly(true)]
    public string Id { get; set; } = null!;
    [ReadOnly(true)]
    public string Email { get; set; } = null!;
    public RoleModel[] Roles { get; set; } = null!;
    public class RoleModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}