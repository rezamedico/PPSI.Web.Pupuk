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
    public class PupukRepository
    {
        //Private Field
        private pupuk_ppsi _db;
        private string returnMessage = string.Empty;

        //Default Constructor
        public PupukRepository()
        {
            _db = new pupuk_ppsi();
        }        

        public DataTablesJsonResult GetAllPupuk(IDataTablesRequest request)
        {
            DataTablesResponse response = null;
            try
            {                
                var lPupuk = _db.MsPupuk.Where(x => x.IsDelete == 0).ToList();
                response = DataTablesResponse.Create(request, lPupuk.Count, lPupuk.Count, lPupuk);
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
                response = DataTablesResponse.Create(null, returnMessage);
            }
            return  new DataTablesJsonResult(response, true);
        }

        public MsPupuk GetPupukById(int pupukId)
        {
            MsPupuk o = new MsPupuk();
            try
            {
                o = _db.MsPupuk.Where(x => x.PupukId == pupukId).FirstOrDefault();
            }
            catch(Exception ex)
            {
                o = null;
            }
            return o;
        }

        public string SavePupuk(MsPupuk param)
        {            
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            string jsonResult = string.Empty;
            try
            {
                var oExistData = _db.MsPupuk.Where(x => x.Nama == param.Nama).FirstOrDefault();
                if(oExistData == null)
                {
                    _db.InsertWithIdentity(param);
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Successfully Save Into Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "Pupuk " + param.Nama + " Already Exist In Database";
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

        public string UpdatePupuk(MsPupuk param)
        {
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            string jsonResult = string.Empty;
            try
            {
                var oExistData = _db.MsPupuk.Where(x => x.Nama == param.Nama).FirstOrDefault();
                if (oExistData == null)
                {
                    _db.Update(param) ;
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Updated In The Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "Pupuk " + param.Nama + " Already Exist In Database";
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

        public string DeletePupuk(int PupukId, string DeleteBy)
        {
            string jsonResult = string.Empty;
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            try
            {
                var oExistData = _db.MsPupuk.Where(x => x.PupukId == PupukId).FirstOrDefault();
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
