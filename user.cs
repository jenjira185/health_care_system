
namespace HealthCareSystem
{
    public enum Role
    {
        None,
        Patient,
        Personnel,
        Admin,
        superAdmin,
    }

    public enum PersonnelRoles
    {
        None,
        Doctor,
        Administrator,
    }

    public enum Permissions
    {
        None,
        AddRegistrations,
        AddPersonnel,
        AddAdmin,
        AddLocation,
        ViewPatientJournal,
        ViewAllJournals,
        ViewPermissions,
    }

    public enum Registration
    {
        Accepted,
        Denied,
        Pending,
    }

    public enum Region
    {
        None,
        Skåne,
        Götaland,
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string RoleDetails { get; set; } = string.Empty;
        public Role Role { get; set; }
        public PersonnelRoles PersonnelRoles { get; set; }
        public Registration Registration { get; set; }

        public List<Permissions> PermissionList { get; set; } = new List<Permissions> { Permissions.None };
        public List<int> AssignedPersonnelIds { get; set; } = new List<int>();


        public User(int id, string username, string password, Role role)
        {
            Id = id;
            Registration = (role == Role.Patient || role == Role.Admin) ? Registration.Pending : Registration.Accepted;
            Username = username;
            Role = role;
            var (hash, salt) = PasswordHelper.HashPassword(password);
            PasswordHash = hash;
            PasswordSalt = salt;

            Registration = (role == Role.Patient || role == Role.Admin) ? Registration.Pending : Registration.Accepted;
        }

        public bool AssignPersonnel(int personnelId)
        {
            if (this.Role == Role.Patient && !AssignedPersonnelIds.Contains(personnelId))
            {
                AssignedPersonnelIds.Add(personnelId);
                return true;
            }

            return false;
        }

        public void SetRolePersonnel(int handleRole, User persObject, string roleDetails)
        {
            if (persObject.GetRole() == Role.Personnel)
            {
                if (Enum.IsDefined(typeof(PersonnelRoles), handleRole))
                {
                    PersonnelRole = (PersonnelRoles)handleRole;

                    if (PersonnelRole == PersonnelRoles.Doctor)
                    {
                        this.RoleDetails = string.IsNullOrWhiteSpace(roleDetails) ? string.Empty : roleDetails;
                    }
                    else
                    {
                        this.RoleDetails = string.Empty;
                    }
                }
                else
                {
                    Console.WriteLine($"Value {handleRole} is not a valid personnel role");
                    this.RoleDetails = string.Empty;
                }
            }
        }
        public User() { }
        public Role GetRole() => Role;
        public Registration GetRegistration() => Registration;

        public bool TryLogin(string username, string password)
        => Username = username && PasswordHelper.VerifyPassword(password, PasswordHash, PasswordSalt);

        public void SetRolePersonnel()
        {
            PermissionList.Remove(Permissions.AddPersonnel);
            if (PermissionList.Count == 0)
                PermissionList.Add(Permissions.None);
        }

        public void RevokePermission(Permissions permi)
        {
            PermissionList.Remove(permi);
            if(PermissionList.Count == 0)
            PermissionList.Add(Permissions.None);
        }

        public void AcceptAddLocationPermission()
        {
            if (!PermissionList.Contains(Permissions.AddLocation))
                PermissionList.Add(Permissions.AddLocation);
        }

        public void DenyAddLocationPermission()
        {
            PermissionList.Remove(Permissions.AddLocation);
            if (PermissionList.Count == 0)
                PermissionList.Add(Permissions.None);
        }

        public void AcceptAddRegistrationPermission()
        {
            if (!PermissionList.Contains(Permissions.AddRegistrations))
                PermissionList.Add(Permissions.AddRegistrations);
        }


        public void AcceptPending() => Registration = Registration.Accepted;
        public void DenyPending() => Registration = Registration.Denied;

        public void GrantPermission(Permissions permi)
        {
            if (!PermissionList.Contains(permi))
                PermissionList.Add(permi);
        }

        public void DenyAddRegistrationsPermission()
        {
            PermissionList.Remove(Permissions.AddRegistrations);
            if (PermissionList.Count == 0)
                PermissionList.Add(Permissions.None);
        }

        public void AcceptAddPersonnelPermission()
        {
            if (!PermissionList.Contains(Permissions.AddPersonnel))
                PermissionList.Add(Permissions.AddPersonnel);
        }

        public void DenyAddPersonnelPermission()
        {
            PermissionList.Remove(Permissions.AddPersonnel);
            if (PermissionList.Count == 0)
                PermissionList.Add(Permissions.None);
        }

        public void AcceptViewPermissions()
        {
            if (!PermissionList.Contains(Permissions.ViewPermissions))
                PermissionList.Add(Permissions.ViewPermissions);
        }

        public void DenyViewPermissions()
        {
            PermissionList.Remove(Permissions.ViewPermissions);
            if (PermissionList.Count == 0)
                PermissionList.Add(Permissions.None);
        }

        public bool HasPermission(Permissions permission)
        => PermissionList.Contains(permission);

        public string ToPersonnelDisPlay()
        {
            if (Role != Role.Personnel || PersonnelRole != PersonnelRoles.Doctor)
            {
                return $"{Username} - No Doctor";

            }

            string details = string.IsNullOrWhiteSpace(RoleDetails) ? "Unspecified" : RoleDetails;
            return $"Dr. {Username} - {details}";
        }

        
        public override string ToString()
        {
            string roleInfo = (Role == Role.Personnel)
            ? $", Personnel Role: {PersonnelRole}, Details: {RoleDetails}" : string.Empty;

            return $"ID: {Id}, Username: {Username}, Role: {Role}, Registration: {Registration}{roleInfo}, Roles as personnel: {PersonnelRole}, Permissions: {string.Join(",", PermissionList)}";
        }

        public Region? assignedRegion = null;
        public void AssignedRegion(Region region)
        {
            assignedRegion = region;
        }
        public Region? GetAssignedRegion()
        {
            return assignedRegion;
        }
    }
}
