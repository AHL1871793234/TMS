using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Common.Jwt
{
    /// <summary>
    /// 存放Token 跟过期时间的类
    /// </summary>
    public class TnToken
    {
        /// <summary>
        /// token字符串
        /// </summary>
        public string TokenStr { get; set; }
        /// <summary>
        /// token过期时间
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
