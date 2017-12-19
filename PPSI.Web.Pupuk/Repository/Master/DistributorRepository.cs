using System;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Collections.Generic;
using System.Linq;
using PPSI.Web.Pupuk.Models;
using LinqToDB;
using PPSI.Web.Pupuk.ViewModels;
using Newtonsoft.Json;

namespace PPSI.Web.Pupuk.Repository.Master
{
    public class DistributorRepository
    {
        //Private Field
        private pupuk_ppsi _db;
        private string returnMessage = string.Empty;

        //Default Constructor
        public DistributorRepository()
        {
            _db = new pupuk_ppsi();
        }

        public DataTablesJsonResult GetAllDistributor(IDataTablesRequest request)
        {
            DataTablesResponse response = null;
            try
            {
                var lMsAktor = _db.MsAktor.Where(x => x.RoleId == 2 && x.IsDelete == 0).ToList();
                response = DataTablesResponse.Create(request, lMsAktor.Count, lMsAktor.Count, lMsAktor);
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
                response = DataTablesResponse.Create(null, returnMessage);
            }

            return new DataTablesJsonResult(response, true);
        }

        public MsAktor GetDistributorById(int distributorId)
        {
            MsAktor o = new MsAktor();
            try
            {
                o = _db.MsAktor.Where(x => x.AktorId == distributorId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                o = null;
            }
            return o;
        }

        public string SaveDistributor(MsAktor param)
        {
            string jsonResult = string.Empty;
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            try
            {
                var oExistData = _db.MsAktor.Where(x => x.Nama == param.Nama && x.RoleId == 2).FirstOrDefault();
                if (oExistData == null)
                {
                    _db.InsertWithIdentity(param);
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Successfully Saved Into Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "Data Already Exist In The Database";
                }
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }
            catch (Exception ex)
            {
                oReturn.Payload = null;
                oReturn.Messages = ex.Message;
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }

            return jsonResult;
        }

        public string UpdateDistributor(MsAktor param)
        {
            string jsonResult = string.Empty;
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            try
            {
                var oExistData = _db.MsAktor.Where(x => x.Nama == param.Nama && x.RoleId == 2).FirstOrDefault();
                if (oExistData == null)
                {
                    _db.Update(param);
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Successfully Updated In The Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "Data Already Exist In The Database";
                }
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
                oReturn.Payload = null;
                oReturn.Messages = ex.Message;
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }

            return jsonResult;
        }

        public string DeleteDistributor(int DistributorId, string DeleteBy)
        {
            string jsonResult = string.Empty;
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            try
            {
                var oExistData = _db.MsAktor.Where(x => x.AktorId == DistributorId).FirstOrDefault();
                if (oExistData != null)
                {
                    oExistData.EditBy = DeleteBy;
                    oExistData.EditDate = DateTime.Now;
                    oExistData.IsDelete = 1;
                    _db.Update(oExistData);
                    oReturn.Payload = oExistData;
                    oReturn.Messages = "Data Deleted In The Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "Data Failed To Delete In The Database";
                }
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
                oReturn.Payload = null;
                oReturn.Messages = ex.Message;
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }

            return jsonResult;
        }

    }

}
