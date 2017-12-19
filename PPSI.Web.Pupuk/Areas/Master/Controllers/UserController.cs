using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPSI.Web.Pupuk.Models;
using PPSI.Web.Pupuk.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Data;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using PPSI.Web.Pupuk.Repository.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPSI.Web.Pupuk.Repository;
using Microsoft.AspNetCore.Authorization;
using PPSI.Web.Pupuk.ViewModels;
using PPSI.Web.Pupuk.Filters;
using PPSI.Web.Pupuk.Interfaces;

namespace PPSI.Web.Pupuk.Areas.Master.Controllers
{
    [Area("Master")]
    [PupukActionFilter]
    public class UserController : Controller
    {
        #region Private Field
        private UserRepository _repository = new UserRepository();
        private FunctionHelperRepository _repoHelper = new FunctionHelperRepository();
        IPupukSession _session;
        #endregion

        public UserController(IPupukSession session)
        {
            _session = session;
        }
        
        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult IndexDataTable(IDataTablesRequest request)
        {
            try
            {                
                DataTablesJsonResult model = _repository.GetAllUsers(request);
                return model;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        
        public IActionResult CreateUser()
        {
            var lAktor = _repoHelper.GetDropdownAktor();
            ViewBag.ddAktor = new SelectList(lAktor, "AktorId", "Nama");
            return View();
        }

        public IActionResult EditUser(int userId)
        {            
            var model = _repository.GetUserById(userId);
            return View(model);
        }
        
        [HttpPost]
        public JsonResult CreateUser(User param)
        {
            RepoReturnViewModel<User> o = new RepoReturnViewModel<User>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.Password = "12345";
                param.AddDate = DateTime.Now;
                param.AddBy = _session.NamaAktor;
                strRepo = _repository.SaveUser(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<User>>(strRepo);
            }
            catch(Exception ex)
            {
                o = null;
                returnMessage = ex.Message;
            }

            return Json(new { data = o.Payload, message = o.Messages });
        }

        [HttpPost]
        public JsonResult UpdateUser(User param)
        {
            RepoReturnViewModel<User> o = new RepoReturnViewModel<User>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.EditDate = DateTime.Now;
                param.EditBy = _session.NamaAktor;
                strRepo = _repository.UpdateUser(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<User>>(strRepo);
            }
            catch (Exception ex)
            {
                o.Payload = null;
                o.Messages = ex.Message;
            }
            return Json(new { data = o.Payload, message = o.Messages });
        }


        public JsonResult DeleteUser(string userId)
        {
            RepoReturnViewModel<User> o = new RepoReturnViewModel<User>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                strRepo = _repository.DeleteUser(Convert.ToInt32(userId), _session.NamaAktor);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<User>>(strRepo);
            }
            catch (Exception ex)
            {
                o.Payload = null;
                o.Messages = ex.Message;
            }
            return Json(new { data = o.Payload, message = o.Messages });
        }

    }
}
