using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model.Entity;

namespace TMS.Service.CarRegistration
{
    public interface IVehicleManagementService
    {
        /// <summary>
        /// 显示车辆管理
        /// </summary>
        /// <param name="factoryPlate">厂牌型号</param>
        /// <param name="carNumber">车牌号</param>
        /// <param name="carName">司机名称</param>
        /// <param name="companies">所属公司</param>
        /// <returns></returns>
        List<RegistrationModel> GetCarRegistrations(string factoryPlate, string carNumber, string carName, string companies);

        /// <summary>
        /// 添加车辆管理
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        bool AddCar(RegistrationModel model);

        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        bool DelCar(string id);
        /// <summary>
        /// 反填车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RegistrationModel EditCar(int id);

        /// <summary>
        /// 修改车辆信息
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        bool UpdCar(RegistrationModel model);
    }
}
