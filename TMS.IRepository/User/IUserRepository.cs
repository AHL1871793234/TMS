using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model.Entity;

namespace TMS.IRepository.User
{
    public interface IUserRepository
    {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        UserModel GetLogin(string userName, string userPwd);
    }
}
