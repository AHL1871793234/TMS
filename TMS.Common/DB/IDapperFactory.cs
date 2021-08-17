using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Common.DB
{
    public interface IDapperFactory
    {
        DapperClientHelper CreateClient(string name);
    }
}
