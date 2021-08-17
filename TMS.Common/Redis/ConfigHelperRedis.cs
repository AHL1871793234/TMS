using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Redis
{
    /// <summary>
    /// Redis配置信息
    /// </summary>
    public class ConfigHelperRedis
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string _conn = "";
        /// <summary>
        /// 实例化名称
        /// </summary>
        public static string _name = "";
        /// <summary>
        /// 存储库
        /// </summary>
        public static int _db = 0;
        /// <summary>
        /// 密码
        /// </summary>
        public static string _pwd = "";
        /// <summary>
        /// 端口号
        /// </summary>
        public static int _port = 0;
        /// <summary>
        /// 服务器名称/IP
        /// </summary>
        public static string _server = "";
    }
}
