using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.IRepository.CarRegistration;
using TMS.Model.Entity;

namespace TMS.Service.CarRegistration
{
    public class VehicleManagementService:IVehicleManagementService
    {
        public readonly IVehicleManagementRepository carRegistration;

        public VehicleManagementService(IVehicleManagementRepository _carRegistration)
        {
            carRegistration = _carRegistration;
        }

        /// <summary>
        /// 显示车辆管理
        /// </summary>
        /// <param name="factoryPlate">厂牌型号</param>
        /// <param name="carNumber">车牌号</param>
        /// <param name="carName">司机名称</param>
        /// <param name="companies">所属公司</param>
        /// <returns></returns>
        public List<RegistrationModel> GetCarRegistrations(string factoryPlate, string carNumber, string carName, string companies)
        {
            return carRegistration.GetCarRegistrations(factoryPlate, carNumber, carName, companies);
        }

        /// <summary>
        /// 添加车辆管理
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public bool AddCar(RegistrationModel model)
        {
            return carRegistration.AddCar(model);
        }

        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public bool DelCar(string id)
        {
            return carRegistration.DelCar(id);
        }

        /// <summary>
        /// 反填车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RegistrationModel EditCar(int id)
        {
            return carRegistration.EditCar(id);
        }

        /// <summary>
        /// 修改车辆信息
        /// </summary>
        /// <param name="model">车辆信息</param>
        /// <returns></returns>
        public bool UpdCar(RegistrationModel model)
        {
            return carRegistration.UpdCar(model);
        }
    }
}
