using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Permissions.Infrastructure.Models
{
    public class Permission
    {
        /*
        public int Id { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }

        // Propiedad de navegación para establecer la relación con PermissionType
        public PermissionType PermissionType { get; set; }
        */
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeForename { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeSurname { get; set; }

        public int PermissionTypeId { get; set; }

        [ForeignKey("PermissionTypeId")]
        public virtual PermissionType PermissionType { get; set; }

        [Required]
        public DateTime PermissionDate { get; set; }
    }
  
}
