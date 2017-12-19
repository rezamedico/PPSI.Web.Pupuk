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
    public class PetaniController : Controller
    {
        #region Private Field
        private PetaniRepository _repository = new PetaniRepository();
        private FunctionHelperRepository _repoHelper = new FunctionHelperRepository();
        IPupukSession _session;
        #endregion

        public PetaniController(IPupukSession session)
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
                DataTablesJsonResult model = _repository.GetAllPetani(request);
                return model;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        
        public IActionResult CreatePetani()
        {
            var lProvinsi = _repoHelper.GetDropdownProvinsi();
            var lKota = _repoHelper.GetDropdownKabupaten();
            var lKecamatan = _repoHelper.GetDropdownKecamatan();
            ViewBag.ddProvinsi = new SelectList(lProvinsi, "RefProvinsiId", "Nama");
            ViewBag.ddKota = new SelectList(lKota, "RefKotaId", "Nama");
            ViewBag.ddKecamatan = new SelectList(lKecamatan, "RefAreaId", "NamaArea");
            return View();
        }

        public IActionResult EditPetani(int petaniId)
        {
            var lProvinsi = _repoHelper.GetDropdownProvinsi();
            var lKota = _repoHelper.GetDropdownKabupaten();
            var lKecamatan = _repoHelper.GetDropdownKecamatan();
            var model = _repository.GetPetaniById(petaniId);
            ViewBag.ddProvinsi = new SelectList(lProvinsi, "RefProvinsiId", "Nama");
            ViewBag.ddKota = new SelectList(lKota, "RefKotaId", "Nama");
            ViewBag.ddKecamatan = new SelectList(lKecamatan, "RefAreaId", "NamaArea");
            return View(model);
        }
        
        [HttpPost]
        public JsonResult CreatePetani(MsAktor param)
        {
            RepoReturnViewModel<MsAktor> o = new RepoReturnViewModel<MsAktor>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.Kode = _repoHelper.GetPetaniCode();
                param.RoleId = 1;                
                param.AddDate = DateTime.Now;
                param.AddBy = _session.NamaAktor;
                strRepo = _repository.SavePetani(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsAktor>>(strRepo);
            }
            catch(Exception ex)
            {
                o = null;
                returnMessage = ex.Message;
            }

            return Json(new { data = o.Payload, message = o.Messages });
        }

        [HttpPost]
        public JsonResult UpdatePetani(MsAktor param)
        {
            RepoReturnViewModel<MsAktor> o = new RepoReturnViewModel<MsAktor>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.EditDate = DateTime.Now;
                param.EditBy = _session.NamaAktor;
                strRepo = _repository.UpdatePetani(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsAktor>>(strRepo);
            }
            catch (Exception ex)
            {
                o.Payload = null;
                o.Messages = ex.Message;
            }
            return Json(new { data = o.Payload, message = o.Messages });
        }


        public JsonResult DeletePetani(string petaniId)
        {
            RepoReturnViewModel<MsAktor> o = new RepoReturnViewModel<MsAktor>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                strRepo = _repository.DeletePetani(Convert.ToInt32(petaniId), _session.NamaAktor);
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
