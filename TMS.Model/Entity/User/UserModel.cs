using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model.Entity
{
    /// <summary>
    /// 用户管理表
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPwd { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string UserSex { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// 用户身份证号
        /// </summary>
        public string UserIdentityCard { get; set; }
        /// <summary>
        /// 用户家庭住址
        /// </summary>
        public string UserHomePlace { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime UserBrithday { get; set; }
        /// <summary>
        /// 院校
        /// </summary>
        public string UserSchool { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string UseMajor { get; set; }
        /// <summary>
        /// 学历 1：小学2：初中3：高中4：大学本科5：研究生
        /// </summary>
        public string UserEducation { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string UserNation { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string UserNativePlace { get; set; }
        /// <summary>
        /// 政治面貌(1：团员2：党员3：平民)
        /// </summary>
        public string UserPoliticsStatus { get; set; }
        /// <summary>
        /// 婚姻 1：已婚2：未婚3:不清楚
        /// </summary>
        public string UseMarriage { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int UserStatus { get; set; }
    }
}
