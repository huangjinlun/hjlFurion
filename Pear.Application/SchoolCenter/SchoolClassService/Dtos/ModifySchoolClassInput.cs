using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Application.SchoolCenter
{
    public class ModifySchoolClassInput
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空"), StringLength(32, MinimumLength = 2, ErrorMessage = "昵称必须在 2 到 16 个字符之间")]
        public string ClassName { get; set; }
    }
}
