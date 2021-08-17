using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model.Entity;
using TMS.Common.DB;
using TMS.IRepository.CarRegistration;

namespace TMS.Repository.CarRegistration
{
    public class VehicleManagementRepository:IVehicleManagementRepository
    {
        //调用DapperClientHelper 数据库连接
        private readonly DapperClientHelper _SqlDB;

        //对的每个调用 CreateClient(String) 都保证返回一个新的 HttpClient 实例。 调用方可以无限期缓存返回的 HttpClient 实例，也可以在 块中使用它来释放它。
        //默认IHttpClientFactory 实现可能会缓存基础 HttpMessageHandler 实例以提高性能。
        //调用方还可以根据需要自由地改变返回的 HttpClient 实例的公共属性。
        //小型工厂模式
        public VehicleManagementRepository(IDapperFactory dapperFactory)
        {
            _SqlDB = dapperFactory.CreateClient("SqlDb");
        }

        /// <summary>
        /// 显示车辆管理
        /// </summary>
        /// <returns></returns>
        public List<RegistrationModel> GetCarRegistrations(string factoryPlate, string carNumber, string carName, string companies)
        {
            string sql = "select RegistrationID,FactoryPlateModel,LicensePlateNumber,LicensePlateName,LicensePlateLWH,LicensePlateColour,RegistrationImg,SubordinateCompanies,BuyTime,ServiceCertificateNumber,InsuranceExpireTime,AnnualExpireTime,MaintainKilometreSetting,MaintainCardImg from RegistrationModel";
            List<RegistrationModel> data = _SqlDB.Query<RegistrationModel>(sql);
            if (!string.IsNullOrEmpty(factoryPlate))//厂牌型号
            {
                data = data.Where(x => x.FactoryPlateModel.Contains(factoryPlate)).ToList();
            }
            if (!string.IsNullOrEmpty(carNumber))//车牌号
            {
                data = data.Where(x => x.LicensePlateNumber.Contains(carNumber)).ToList();
            }
            if (!string.IsNullOrEmpty(carName))//司机名称
            {
                data = data.Where(x => x.LicensePlateName.Contains(carName)).ToList();
            }
            if (!string.IsNullOrEmpty(companies))//所属公司
            {
                data = data.Where(x => x.SubordinateCompanies.Contains(companies)).ToList();
            }
            return data;
        }

        /// <summary>
        /// 添加车辆管理
        /// </summary>
        /// <param name="registrationModel">车辆信息</param>
        /// <returns></returns>
        public bool AddCar(RegistrationModel registrationModel)
        {
            string sql = "insert into RegistrationModel values(@FactoryPlateModel,@LicensePlateNumber,@LicensePlateName,@LicensePlateLWH,@LicensePlateColour,@RegistrationImg,@SubordinateCompanies,@BuyTime,@ServiceCertificateNumber,@InsuranceExpireTime,@AnnualExpireTime,@MaintainKilometreSetting,@MaintainCardImg)";
            int code = _SqlDB.Execute(sql, new
            {
                @FactoryPlateModel = registrationModel.FactoryPlateModel,
                @LicensePlateNumber = registrationModel.LicensePlateNumber,
                @LicensePlateName = registrationModel.LicensePlateName,
                @LicensePlateLWH = registrationModel.LicensePlateLWH,
                @LicensePlateColour = registrationModel.LicensePlateColour,
                @RegistrationImg = registrationModel.RegistrationImg,
                @SubordinateCompanies = registrationModel.SubordinateCompanies,
                @BuyTime = registrationModel.BuyTime,
                @ServiceCertificateNumber = registrationModel.ServiceCertificateNumber,
                @InsuranceExpireTime = registrationModel.InsuranceExpireTime,
                @AnnualExpireTime = registrationModel.AnnualExpireTime,
                @MaintainKilometreSetting = registrationModel.MaintainKilometreSetting,
                @MaintainCardImg = registrationModel.MaintainCardImg
            });
            return code == 0 ? true : false;
        }

        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public bool DelCar(string id)
        {
            string sql = "delete from RegistrationModel where RegistrationID in (@ID)";
            int code = _SqlDB.Execute(sql, new { @ID = id });
            return code == 0 ? true : false;
        }

        /// <summary>
        /// 反填车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RegistrationModel EditCar(int id)
        {
            string sql = "select RegistrationID,FactoryPlateModel,LicensePlateNumber,LicensePlateName,LicensePlateLWH,LicensePlateColour,RegistrationImg,SubordinateCompanies,BuyTime,ServiceCertificateNumber,InsuranceExpireTime,AnnualExpireTime,MaintainKilometreSetting,MaintainCardImg from RegistrationModel where RegistrationID=@ID";
            return _SqlDB.QueryFirst<RegistrationModel>(sql, new { @ID = id });
        }

        /// <summary>
        /// 修改车辆信息
        /// </summary>
        /// <param name="registrationModel">车辆信息</param>
        /// <returns></returns>
        public bool UpdCar(RegistrationModel registrationModel)
        {
            string sql = "update RegistrationModel set FactoryPlateModel=@FactoryPlateModel,LicensePlateNumber=@LicensePlateNumber,LicensePlateName=@LicensePlateName,LicensePlateLWH=@LicensePlateLWH,LicensePlateColour=@LicensePlateColour,RegistrationImg=@RegistrationImg,SubordinateCompanies=@SubordinateCompanies,BuyTime=@BuyTime,ServiceCertificateNumber=@ServiceCertificateNumber,InsuranceExpireTime=@InsuranceExpireTime,AnnualExpireTime=@AnnualExpireTime,MaintainKilometreSetting=@MaintainKilometreSetting,MaintainCardImg=@MaintainCardImg where RegistrationID=@RegistrationID";
            int code = _SqlDB.Execute(sql, new
            {
                @RegistrationID = registrationModel.RegistrationID,
                @FactoryPlateModel = registrationModel.FactoryPlateModel,
                @LicensePlateNumber = registrationModel.LicensePlateNumber,
                @LicensePlateName = registrationModel.LicensePlateName,
                @LicensePlateLWH = registrationModel.LicensePlateLWH,
                @LicensePlateColour = registrationModel.LicensePlateColour,
                @RegistrationImg = registrationModel.RegistrationImg,
                @SubordinateCompanies = registrationModel.SubordinateCompanies,
                @BuyTime = registrationModel.BuyTime,
                @ServiceCertificateNumber = registrationModel.ServiceCertificateNumber,
                @InsuranceExpireTime = registrationModel.InsuranceExpireTime,
                @AnnualExpireTime = registrationModel.AnnualExpireTime,
                @MaintainKilometreSetting = registrationModel.MaintainKilometreSetting,
                @MaintainCardImg = registrationModel.MaintainCardImg
            });
            return code == 0 ? true : false;
        }
    }
}
