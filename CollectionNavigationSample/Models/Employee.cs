using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionNavigationSample.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Employer Employer { get; set; }
        public int EmployerId { get; set; }

        public virtual Department Department { get; set; }
        public int DepartmentId { get; set; }
    }
}
