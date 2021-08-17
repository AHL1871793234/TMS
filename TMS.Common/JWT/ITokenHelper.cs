using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Common.Jwt
{
    /// <summary>
    /// token工具类的接口，方便使用依赖注入，很简单提供两个常用的方法
    /// </summary>
    public interface ITokenHelper
    {
        /// <summary>
        /// 根据一个对象通过反射提供负载，生成token  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <returns></returns>
        TnToken CreateToken<T>(T entity) where T : class;


        /// <summary>
        /// 根据键值对提供负载，生成token
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        TnToken CreateToken(Dictionary<string, string> keyValuePairs);
    }
}
