using Microsoft.AspNetCore.Identity;

namespace Permissions.API.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}