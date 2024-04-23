using System;

namespace Permissions.API.Models
{
    public class OperationLog
    {
        public Guid Id { get; set; }
        public string OperationName { get; set; }
        public int PermissionId { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionType { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
