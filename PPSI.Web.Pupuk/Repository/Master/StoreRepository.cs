using System;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Collections.Generic;
using System.Linq;
using PPSI.Web.Pupuk.Models;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PPSI.Web.Pupuk.ViewModels;

namespace PPSI.Web.Pupuk.Repository.Master
{
    public class StoreRepository
    {
        //Private Field
        private pupuk_ppsi _db;
        private string returnMessage = string.Empty;

        //Default Constructor
        public StoreRepository()
        {
            _db = new pupuk_ppsi();
        }        

        public DataTablesJsonResult GetAllStore(IDataTablesRequest request)
        {
            DataTablesResponse response = null;
            try
            {
                var lStores = (from a in _db.Stores
                               join b in _db.MsAktor on a.DistributorId equals b.AktorId
                               where a.IsDelete == 0
                               select new {
                                   a.Kode,
                                   a.Nama,
                                   DistributorName = b.Nama,
                                   a.StoreId
                               }).ToList();//_db.Stores.Where(x => x.IsDelete == 0).ToList();
                response = DataTablesResponse.Create(request, lStores.Count, lStores.Count, lStores);
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
                response = DataTablesResponse.Create(null, returnMessage);
            }
            return  new DataTablesJsonResult(response, true);
        }

        public Store GetStoreById(int storeId)
        {
            Store o = new Store();
            try
            {
                o = _db.Stores.Where(x => x.StoreId == storeId).FirstOrDefault();
            }
            catch(Exception ex)
            {
                o = null;
            }
            return o;
        }

        public string SaveStore(Store param)
        {            
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            string jsonResult = string.Empty;
            try
            {
                var oExistData = _db.Stores.Where(x => x.Nama == param.Nama).FirstOrDefault();
                if(oExistData == null)
                {
                    _db.InsertWithIdentity(param);
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Successfully Save Into Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "Store " + param.Nama + " Already Exist In Database";
                }
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }
            catch(Exception ex)
            {                
                oReturn.Payload = null;
                oReturn.Messages = ex.Message;
                jsonResult = JsonConvert.SerializeObject(oReturn);
            }

            return jsonResult;
        }

        public string UpdateStore(Store param)
        {
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            string jsonResult = string.Empty;
            try
            {
                var oExistData = _db.Stores.Where(x => x.Nama == param.Nama).FirstOrDefault();
                if (oExistData == null)
                {
                    _db.Update(param) ;
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Updated In The Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "Store " + param.Nama + " Already Exist In Database";
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

        public string DeleteStore(int StoreId, string DeleteBy)
        {
            string jsonResult = string.Empty;
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            try
            {
                var oExistData = _db.Stores.Where(x => x.StoreId == StoreId).FirstOrDefault();
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
