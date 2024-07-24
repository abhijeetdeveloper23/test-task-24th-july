using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModuleImplementation.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? LogoUrl { get; set; }

        public int? ParentDepartmentId { get; set; }
        // Navigation property for parent department
        public Department? ParentDepartment { get; set; }
        // Navigation property for sub-departments
        public List<Department>? SubDepartments { get; set; }
    }
}

