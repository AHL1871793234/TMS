using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Redis
{
    public class APIResultHelper
    {
        /// <summary>
        /// 返回成功的状态码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultData Success(object data)
        {
            return new ResultData
            {
                Code = 200,
                Data = data,
                Message = "操作成功!!"
            };
        }
        /// <summary>
        /// 失败的状态码
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ResultData Error(string msg)
        {
            return new ResultData
            {
                Code = 500,
                Data = null,
                Message = msg,
            };
        }
    }
}
