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
using Microsoft.AspNetCore.Authorization;
using PPSI.Web.Pupuk.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPSI.Web.Pupuk.ViewModels;
using PPSI.Web.Pupuk.Filters;
using PPSI.Web.Pupuk.Interfaces;
using PPSI.Web.Pupuk.Repository.Master;
using DataTables.AspNet.Core;
using DataTables.AspNet.AspNetCore;

namespace PPSI.Web.Pupuk.Areas.Master.Controllers
{
    [Area("Master")]
    [PupukActionFilter]
    public class DistributorController : Controller
    {
        #region Private Field
        private DistributorRepository _repository = new DistributorRepository();
        private FunctionHelperRepository _repoHelper = new FunctionHelperRepository();
        IPupukSession _session;
        #endregion        

        public DistributorController(IPupukSession session)
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
                DataTablesJsonResult model = _repository.GetAllDistributor(request);
                return model;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public IActionResult CreateDistributor()
        {
            var lProvinsi = _repoHelper.GetDropdownProvinsi();
            var lKota = _repoHelper.GetDropdownKabupaten();
            var lKecamatan = _repoHelper.GetDropdownKecamatan();
            ViewBag.ddProvinsi = new SelectList(lProvinsi, "RefProvinsiId", "Nama");
            ViewBag.ddKota = new SelectList(lKota, "RefKotaId", "Nama");
            ViewBag.ddKecamatan = new SelectList(lKecamatan, "RefAreaId", "NamaArea");
            return View();
        }

        [HttpPost]
        public JsonResult CreateDistributor(MsAktor param)
        {
            RepoReturnViewModel<MsAktor> o = new RepoReturnViewModel<MsAktor>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.Kode = _repoHelper.GetDistributorCode();
                param.RoleId = 2;
                param.AddDate = DateTime.Now;
                param.AddBy = _session.NamaAktor;
                strRepo = _repository.SaveDistributor(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsAktor>>(strRepo);
            }
            catch (Exception ex)
            {
                o = null;
                returnMessage = ex.Message;
            }

            return Json(new { data = o.Payload, message = o.Messages });
        }

        public IActionResult EditDistributor(int distributorId)
        {
            var lProvinsi = _repoHelper.GetDropdownProvinsi();
            var lKota = _repoHelper.GetDropdownKabupaten();
            var lKecamatan = _repoHelper.GetDropdownKecamatan();
            var model = _repository.GetDistributorById(distributorId);
            ViewBag.ddProvinsi = new SelectList(lProvinsi, "RefProvinsiId", "Nama");
            ViewBag.ddKota = new SelectList(lKota, "RefKotaId", "Nama");
            ViewBag.ddKecamatan = new SelectList(lKecamatan, "RefAreaId", "NamaArea");
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateDistributor(MsAktor param)
        {
            RepoReturnViewModel<MsAktor> o = new RepoReturnViewModel<MsAktor>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.EditDate = DateTime.Now;
                param.EditBy = _session.NamaAktor;
                strRepo = _repository.UpdateDistributor(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsAktor>>(strRepo);
            }
            catch (Exception ex)
            {
                o.Payload = null;
                o.Messages = ex.Message;
            }
            return Json(new { data = o.Payload, message = o.Messages });
        }

        public JsonResult DeleteDistributor(string distributorId)
        {
            RepoReturnViewModel<MsAktor> o = new RepoReturnViewModel<MsAktor>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                strRepo = _repository.DeleteDistributor(Convert.ToInt32(distributorId), _session.NamaAktor);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsAktor>>(strRepo);
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
