﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pear.Application.SchoolCenter
{
   public  class GetSchoolClassListInput
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Required(ErrorMessage = "页码不能为空"), Range(1, int.MaxValue, ErrorMessage = ("页码只能在 1 到 2147483647 之间"))]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页容量
        /// </summary>
        [Required(ErrorMessage = "页容量不能为空"), Range(5, 200, ErrorMessage = ("页码只能在 5 到 200 之间"))]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
