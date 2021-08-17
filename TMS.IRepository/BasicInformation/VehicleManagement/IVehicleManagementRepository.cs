using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model.Entity;

namespace TMS.IRepository.CarRegistration
{
    public interface IVehicleManagementRepository
    {
        /// <summary>
        /// 显示车辆管理
        /// </summary>
        /// <returns></returns>
        List<RegistrationModel> GetCarRegistrations(string factoryPlate, string carNumber, string carName, string companies);

        /// <summary>
        /// 添加车辆管理
        /// </summary>
        /// <param name="registrationModel">车辆信息</param>
        /// <returns></returns>
        bool AddCar(RegistrationModel registrationModel);

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
        /// <param name="registrationModel">车辆信息</param>
        /// <returns></returns>
        bool UpdCar(RegistrationModel registrationModel);
    }
}
