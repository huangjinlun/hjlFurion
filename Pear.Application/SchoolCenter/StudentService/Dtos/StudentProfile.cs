using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Application.SchoolCenter
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public int SchoolClassId { get; set; }
        public string SchoolClassName { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
    }
}
