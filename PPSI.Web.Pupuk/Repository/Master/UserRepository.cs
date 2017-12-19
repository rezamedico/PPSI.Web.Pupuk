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
    public class UserRepository
    {
        //Private Field
        private pupuk_ppsi _db;
        private string returnMessage = string.Empty;

        //Default Constructor
        public UserRepository()
        {
            _db = new pupuk_ppsi();
        }        

        public DataTablesJsonResult GetAllUsers(IDataTablesRequest request)
        {
            DataTablesResponse response = null;
            try
            {
                var lUsers = (from a in _db.Users
                              join b in _db.MsAktor on a.AktorId equals b.AktorId
                              where a.IsDelete == 0
                              select new
                              {
                                  UserId = a.UserId,
                                  Nama = b.Nama,
                                  Status = (a.IsDelete == 1 ? "Tidak Aktif" : "Aktif"),
                                  Username = a.Username
                              }).ToList();
                response = DataTablesResponse.Create(request, lUsers.Count, lUsers.Count, lUsers);
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
                response = DataTablesResponse.Create(null, returnMessage);
            }
            return  new DataTablesJsonResult(response, true);
        }

        public User GetUserById(int userId)
        {
            User o = new User();
            try
            {
                o = _db.Users.Where(x => x.UserId == userId).FirstOrDefault();
            }
            catch(Exception ex)
            {
                o = null;
            }
            return o;
        }

        public string SaveUser(User param)
        {            
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            string jsonResult = string.Empty;
            try
            {
                var oExistData = _db.Users.Where(x => x.Username == param.Username).FirstOrDefault();
                if(oExistData == null)
                {
                    _db.InsertWithIdentity(param);
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Successfully Save Into Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "User " + param.Username + " Already Exist In Database";
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

        public string UpdateUser(User param)
        {
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            string jsonResult = string.Empty;
            try
            {
                var oExistData = _db.Users.Where(x => x.Username == param.Username).FirstOrDefault();
                if (oExistData == null)
                {
                    _db.Update(param) ;
                    oReturn.Payload = param;
                    oReturn.Messages = "Data Updated In The Database";
                }
                else
                {
                    oReturn.Payload = null;
                    oReturn.Messages = "User " + param.Username + " Already Exist In Database";
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

        public string DeleteUser(int UserId, string DeleteBy)
        {
            string jsonResult = string.Empty;
            RepoReturnViewModel oReturn = new RepoReturnViewModel();
            try
            {
                var oExistData = _db.Users.Where(x => x.UserId == UserId).FirstOrDefault();
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
