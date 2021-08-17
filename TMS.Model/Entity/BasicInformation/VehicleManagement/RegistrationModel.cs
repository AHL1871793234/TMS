using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model.Entity
{
    /// <summary>
    /// 车辆管理
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// 车辆管理ID
        /// </summary>
        public int RegistrationID { get; set; }
        /// <summary>
        /// 厂牌型号
        /// </summary>
        public string FactoryPlateModel { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicensePlateNumber { get; set; }
        /// <summary>
        /// 司机名称
        /// </summary>
        public string LicensePlateName { get; set; }
        /// <summary>
        /// 车型(长*宽*高)
        /// </summary>
        public string LicensePlateLWH { get; set; }
        /// <summary>
        /// 车型颜色
        /// </summary>
        public string LicensePlateColour { get; set; }
        /// <summary>
        /// 车辆照片
        /// </summary>
        public string RegistrationImg { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public string SubordinateCompanies { get; set; }
        /// <summary>
        /// 购置日期
        /// </summary>
        public DateTime BuyTime { get; set; }
        /// <summary>
        /// 营运证号
        /// </summary>
        public string ServiceCertificateNumber { get; set; }
        /// <summary>
        /// 保险到期时间
        /// </summary>
        public DateTime InsuranceExpireTime { get; set; }
        /// <summary>
        /// 年检到期时间
        /// </summary>
        public DateTime AnnualExpireTime { get; set; }
        /// <summary>
        /// 保养公里设置
        /// </summary>
        public string MaintainKilometreSetting { get; set; }
        /// <summary>
        /// 保险卡照片
        /// </summary>
        public string MaintainCardImg { get; set; }



        /// <summary>
        /// 转换购置时间
        /// </summary>
        public string BuyDate { get { return BuyTime.ToString("yyyy-MM-dd"); } }
        /// <summary>
        /// 保险到期时间
        /// </summary>
        public string InsuranceExpireDate { get { return InsuranceExpireTime.ToString("yyyy-MM-dd"); } }
        /// <summary>
        /// 年检到期时间
        /// </summary>
        public string AnnualExpireDate { get { return AnnualExpireTime.ToString("yyyy-MM-dd"); } }
    }
}
