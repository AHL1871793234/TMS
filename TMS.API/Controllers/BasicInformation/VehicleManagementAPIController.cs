using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Repository;
using TMS.IRepository;
using TMS.Model.Entity;
using TMS.Common.UpFiles;
using TMS.Service.CarRegistration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace TMS.API.Controllers.BasicInformation
{
    /// <summary>
    /// 车辆管理API
    /// </summary>
    [Route("VehicleManagementAPI")]
    [ApiController]
    public class VehicleManagementAPIController : Controller
    {
        /// <summary>
        /// 车辆管理
        /// </summary>
        public readonly IVehicleManagementService _carRegistration;

        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="carRegistration"></param>
        /// <param name="hostingEnvironment"></param>
        public VehicleManagementAPIController(IVehicleManagementService carRegistration, IHostingEnvironment hostingEnvironment)
        {
            _carRegistration = carRegistration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 车辆管理—显示基础数据
        /// </summary>
        /// <param name="factoryPlate"></param>厂牌型号
        /// <param name="carNumber"></param>车牌号
        /// <param name="carName"></param>司机名称
        /// <param name="companies"></param>所属公司
        /// <returns></returns>
        [Route(nameof(GetCarRegistration))]
        [HttpGet]
        public IActionResult GetCarRegistration(string factoryPlate = "", string carNumber = "", string carName = "", string companies = "")
        {
            try
            {
                //数据集
                List<RegistrationModel> data = _carRegistration.GetCarRegistrations(factoryPlate, carNumber, carName, companies);
                //判断数据是否为空
                if (data != null)
                {
                    return Ok(new { code = true, meta = 200, msg = "获取成功", count = data.Count, data = data });
                }
                else
                {
                    return Ok(new { code = false, meta = 500, msg = "获取失败", count = data.Count, data = "" });
                }
            }
            catch (Exception)
            {
                return Ok(new { code = false, meta = 500, msg = "获取失败", count = 0, data = "" });
            }
        }

        /// <summary>
        /// 车辆管理—添加基础数据
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        [Route(nameof(AddCar))]
        [HttpPost]
        public IActionResult AddCar([FromForm]RegistrationModel registrationModel)
        {
            //异常处理
            try
            {
                //获取数据集
                bool data = _carRegistration.AddCar(registrationModel);
                //判断数据是否为真(是否存在)
                if (data == true)
                {
                    return Ok(new { code = data, meta = 200, msg = "添加成功!" });
                }
                else
                {
                    return Ok(new { code = data, meta = 500, msg = "添加失败!" });
                }
            }
            //异常处理提示
            catch (Exception)
            {
                return Ok(new { code = false, meta = 500, msg = "添加失败，处里异常!" });
            }
        }

        /// <summary>
        /// 上传图片—保险卡照片
        /// </summary>
        /// <returns></returns>
        [Route(nameof(UpLoadRegistrationImg))]
        [HttpPost]
        public string UpLoadRegistrationImg()
        {
            //通过IFormFile实例直接获取文件信息
            IFormFile formFile = Request.Form.Files[0];
            //调用并实例化上传文件帮助类/保存路径
            UploadFilesHelper upload = new UploadFilesHelper(_hostingEnvironment,"/Image/");
            string file = upload.Main(formFile);
            //返回
            return file;
        }

        /// <summary>
        /// 车辆管理—删除基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(DeleteCar))]
        [HttpPost]
        public IActionResult DeleteCar(string id)
        {
            try
            {
                //从数据集中获取要删除数据的ID，调用执行删除方法
                bool data = _carRegistration.DelCar(id);
                //判断数据是否为真(是否存在)
                if (data == true)
                {
                    return Ok(new { code = data, meta = 200, msg = "删除成功!" });
                }
                else
                {
                    return Ok(new { code = data, meta = 500, msg = "删除失败!" });
                }
            }
            //异常处理提示
            catch (Exception)
            {
                return Ok(new { code = false, meta = 500, msg = "删除失败!" });
            }
        }

        /// <summary>
        /// 车辆管理—获取相关数据信息（反填）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(EditCar))]
        [HttpGet]
        public IActionResult EditCar(int id)
        {
            try
            {
                //获取全部数据信息
                RegistrationModel data = _carRegistration.EditCar(id);
                //判断数据集是否为空
                if (data != null)
                {
                    return Ok(new { code = true, meta = 200, msg = "获取相关信息成功!" });
                }
                else
                {
                    return Ok(new { code = false, meta = 500, msg = "获取相关信息失败!" });
                }
            }
            //异常处理提示语
            catch (Exception)
            {
                return Ok(new { code = false, meta = 500, msg = "数据处理异常!" });
            }
        }

        /// <summary>
        /// 车辆管理—修改相关数据信息
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        [Route(nameof(UpdateCar))]//指定路由
        [HttpPost]
        public IActionResult UpdateCar([FromForm]RegistrationModel registrationModel)
        {
            try
            {
                bool data = _carRegistration.UpdCar(registrationModel);
                if (data == true)
                {
                    return Ok(new { data = data, mate = 200, msg = "修改成功!" });
                }
                else
                {
                    return Ok(new { data = data, mate = 200, msg = "修改失败!" });
                }
            }
            catch (Exception)
            {
                return Ok(new { data = false, mate = 200, msg = "修改成功!" });
            }
        }
    }
}
