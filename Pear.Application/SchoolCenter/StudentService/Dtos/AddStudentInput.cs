using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Application.SchoolCenter
{
    public class AddStudentInput : ModifyStudentInput
    {
        public int SchoolClassId { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
    }
}
