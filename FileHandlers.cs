using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security;

namespace HealthCareSystem;

class FileHandler
{
    public static string UserFileName = "data/users.csv";

    private const char PrimarySeperator = '|';
    private const char ListSeperator = ',';
    const string DefaultPassword = "123";
    private static List<User> CreateUser()
    {
        Console.WriteLine("file do not exist...");
        const string DefaultPassword = "123";

        var users = new List<User>
        {
            new User(0, "patient", DefaultPassword, Role.Patient),
            new User(1, "personnel", DefaultPassword, Role.personnel),
            new User(2, "Admin1", DefaultPassword, Role.Admin),
            new User(3, "Admin2", DefaultPassword, Role.Admin),
            new User(4, "Admin3", DefaultPassword, Role.Admin),
            new User(5, "Admin4", DefaultPassword, Role.Admin),

            new User(6, "Alice", DefaultPassword, Role.Personnel),
            new User(7, "David", DefaultPassword, Role.Personnel),
            new User(8, "Dr. Bob", DefaultPassword, Role.Personnel),
            new User(9, "Sara", DefaultPassword, Role.Personnel),
            new User(10, "Madde", DefaultPassword, Role.Personnel),
            new User(11, "superAdmin", DefaultPassword, Role.SuperAdmin),
        };

        var mainAdmin = users.FirstOrDefault(u => u.ID == 2);
        if (mainAdmin != null)
        {
            mainAdmin.PermissionsList.Add(Permissions.AddPersonnel);
            mainAdmin.PermissionsList.Add(Permissions.AddRegistrations);
            mainAdmin.Registration = Registreation.Accepted;
        }

        var secondaryAdmin = users.FirstOrDefault(u => u.ID == 4);
        secondaryAdmin?.PermissionsList.Add(Permissions.AddPersonnel);


        var DrBob = users.FirstOrDefault(u => u.ID == 8);
        if (DrBob != null)
        {
            DrBob.PersonnelRole = PersonnelRoles.Doctor;
            DrBob.RoleDetails = "Ortoped";
        }

        var Sara = users.FirstOrDefault(u => u.ID == 9);
        if (Sara != null)
        {
            Sara.PersonnelRole = PersonnelRoles.Doctor;
            Sara.RoleDetails = "Plastic surgery";
        }

        SaveUsersToCsv(users);
        return users.ConvertAll<User>(u => u);
    }

    private static string SpecialPermissions(List<Permissions> permissions)
    {
        var validPermissions = new List<string>();

        foreach (var permi in permissions)
        {
            if (permi != permissions.None)
            {
                validPermissions.Add(permi.ToString());
            }
        }

        return string.Join(ListSeperator, validPermissions);
    }

    private static string SpecialPermissions(List<int> ids)
    {
        var idStrings = new List<string>();

        foreach (var id in ids)
        {
            idStrings.Add(id.ToString());
        }
        return string.Join(ListSeperator, idStrings);
    }


    public static List<User> LoadFromCsv()
    {
        if (!File.Exists(UserFileName))
        {
            return CreateUser();
        }

        List<User> loadUsers = new List<User>();

        try
        {
            string[] lines = File.ReadAllLines(UserFileName);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(PrimarySeperator);

                if (parts.Length == 7)
                {
                    User user = new User();

                    user.ID = int.Parse(parts[0]);
                    user.Username = parts[1];
                    user.PasswordHarsh = parts[2];
                    user.PasswordSalt = parts[3];
                    user.Role = (Role)Enum.Parse(typeof(Role), parts[4], true);
                    user.PersonnelRole = (PersonnelRoles)Enum.Parse(typeof(PersonnelRoles), parts[5], true);
                    user.RoleDetails = parts[6];
                    user.Registration = (Registration)Enum.Parse(typeof(Registration), parts[7], true);


                    string permissionString = parts[8];
                    user.PermissionsList = new List<Permissions>();

                    if (!string.IsNullOrWhiteSpace(permissionString))
                    {
                        string[] permis = permissionString.Split(ListSeperator);

                        foreach (string permisString in permis)
                        {
                            if (Enum.TryParse<Permissions>(permisString.Trim(), true, out Permissions permi))
                            {
                                user.PermissionsList.Add(permi);
                            }
                        }
                    }

                    if (user.PermissionsList.Count == 0) user.PermissionsList.Add(Permissions.None);

                    string assignedIDString = parts[9];
                    user.AssignedPersonnelids = new List<int>();

                    if (!string.IsNullOrWhiteSpace(assignedIDString))
                    {
                        string[] idStrings = assignedIDString.Split(ListSeperator);
                        foreach (string idString in idStrings)
                        {
                            if (int.TryParse(idString.Trim(), out int id)) user.AssignedPersonnelids.Add(id);
                        }
                    }

                    loadUsers.Add(user);
                }
            }
            return loadUsers;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR loading from CSV: {ex.Message}");
            return new List<User>();
        }
    }

    public static void SavedUsersCsv(List<User> users)
    {
        List<string> lines = new List<string>();

        foreach (User user in users)
        {
            if (user is User concreteUser)
            {
                string PermissionsListString = SpecialPermissions(concreteUser.PermissionsList);
                string assignedIDString = SpecialIntList(concreteUser.AssignedPersonnelids);

                string line = string.Join(PrimarySeperator,
                concreteUser.id,
                concreteUser.Username,
                concreteUser.PasswordHarsh,
                concreteUser.PasswordSalt,
                concreteUser.Role,
                concreteUser.PersonnelRole,
                concreteUser.RoleDetails,
                concreteUser.Registration,
                PermissionsListString,
                assignedIDString);

                lines.Add(line);
            }
        }

        try
        {
            File.WriteAllLines(UserFileName, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR saving to CSV: {ex.Message}");
        }
    }
}
