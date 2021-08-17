using System;
using System.Collections.Generic;
using System.Text;


namespace TMS.Common.DB
{
    /// <summary>
    /// 数据库操作参数
    /// </summary>

    public class ConnectionConfig
    {
        public string ConnectionString { get; set; }
        public DbStoreType DbType { get; set; }
    }

    /// <summary>
    /// 枚举判断数据库类型
    /// </summary>
    public enum DbStoreType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3
    }

    public class DapperFactoryOptions
    {
        public IList<Action<ConnectionConfig>> DapperActions { get; } = new List<Action<ConnectionConfig>>();
    }
}
