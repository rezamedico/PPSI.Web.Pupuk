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
    public class StoreController : Controller
    {
        #region Private Field
        private StoreRepository _repository = new StoreRepository();
        private FunctionHelperRepository _repoHelper = new FunctionHelperRepository();
        IPupukSession _session;
        #endregion

        public StoreController(IPupukSession session)
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
                DataTablesJsonResult model = _repository.GetAllStore(request);
                return model;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        
        public IActionResult CreateStore()
        {
            var lDistributor = _repoHelper.GetDropdownAktorByRoleId(2);
            ViewBag.ddDistributor = new SelectList(lDistributor, "AktorId", "Nama");
            return View();
        }

        public IActionResult EditStore(int storeId)
        {            
            var model = _repository.GetStoreById(storeId);
            return View(model);
        }
        
        [HttpPost]
        public JsonResult CreateStore(Store param)
        {
            RepoReturnViewModel<Store> o = new RepoReturnViewModel<Store>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.Kode = _repoHelper.GetStoreCode();
                param.AddDate = DateTime.Now;
                param.AddBy = _session.NamaAktor;
                strRepo = _repository.SaveStore(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<Store>>(strRepo);
            }
            catch(Exception ex)
            {
                o = null;
                returnMessage = ex.Message;
            }

            return Json(new { data = o.Payload, message = o.Messages });
        }

        [HttpPost]
        public JsonResult UpdateStore(Store param)
        {
            RepoReturnViewModel<Store> o = new RepoReturnViewModel<Store>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.EditDate = DateTime.Now;
                param.EditBy = _session.NamaAktor;
                strRepo = _repository.UpdateStore(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<Store>>(strRepo);
            }
            catch (Exception ex)
            {
                o.Payload = null;
                o.Messages = ex.Message;
            }
            return Json(new { data = o.Payload, message = o.Messages });
        }


        public JsonResult DeleteStore(string storeId)
        {
            RepoReturnViewModel<Store> o = new RepoReturnViewModel<Store>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                strRepo = _repository.DeleteStore(Convert.ToInt32(storeId), _session.NamaAktor);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<Store>>(strRepo);
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
