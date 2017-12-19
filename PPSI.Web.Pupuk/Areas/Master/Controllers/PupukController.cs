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
    public class PupukController : Controller
    {
        #region Private Field
        private PupukRepository _repository = new PupukRepository();
        private FunctionHelperRepository _repoHelper = new FunctionHelperRepository();
        IPupukSession _session;
        #endregion

        public PupukController(IPupukSession session)
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
                DataTablesJsonResult model = _repository.GetAllPupuk(request);
                return model;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        
        public IActionResult CreatePupuk()
        {
            ViewBag.ddSumberPupuk = new SelectList(_repoHelper.GetDropdownStaticDataBySection(100), "StaticDataId", "Nama");
            ViewBag.ddJenisUnsur = new SelectList(_repoHelper.GetDropdownStaticDataBySection(200), "StaticDataId", "Nama");
            ViewBag.ddBentukPupuk = new SelectList(_repoHelper.GetDropdownStaticDataBySection(300), "StaticDataId", "Nama");
            ViewBag.ddCaraPakai = new SelectList(_repoHelper.GetDropdownStaticDataBySection(400), "StaticDataId", "Nama");
            ViewBag.ddCaraPelepasan = new SelectList(_repoHelper.GetDropdownStaticDataBySection(500), "StaticDataId", "Nama");            

            return View();
        }

        public IActionResult EditPupuk(int pupukId)
        {            
            var model = _repository.GetPupukById(pupukId);

            ViewBag.ddSumberPupuk = new SelectList(_repoHelper.GetDropdownStaticDataBySection(100), "StaticDataId", "Nama", model.SumberPupukId);
            ViewBag.ddJenisUnsur = new SelectList(_repoHelper.GetDropdownStaticDataBySection(200), "StaticDataId", "Nama", model.JenisUnsurId);
            ViewBag.ddBentukPupuk = new SelectList(_repoHelper.GetDropdownStaticDataBySection(300), "StaticDataId", "Nama", model.BentukPupukId);
            ViewBag.ddCaraPakai = new SelectList(_repoHelper.GetDropdownStaticDataBySection(400), "StaticDataId", "Nama", model.CaraPakaiId);
            ViewBag.ddCaraPelepasan = new SelectList(_repoHelper.GetDropdownStaticDataBySection(500), "StaticDataId", "Nama", model.CaraPelepasanId);

            return View(model);
        }
        
        [HttpPost]
        public JsonResult CreatePupuk(MsPupuk param)
        {
            RepoReturnViewModel<MsPupuk> o = new RepoReturnViewModel<MsPupuk>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {                
                param.AddDate = DateTime.Now;
                param.AddBy = _session.NamaAktor;
                strRepo = _repository.SavePupuk(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsPupuk>>(strRepo);
            }
            catch(Exception ex)
            {
                o = null;
                returnMessage = ex.Message;
            }

            return Json(new { data = o.Payload, message = o.Messages });
        }

        [HttpPost]
        public JsonResult UpdatePupuk(MsPupuk param)
        {
            RepoReturnViewModel<MsPupuk> o = new RepoReturnViewModel<MsPupuk>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                param.EditDate = DateTime.Now;
                param.EditBy = _session.NamaAktor;
                strRepo = _repository.UpdatePupuk(param);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsPupuk>>(strRepo);
            }
            catch (Exception ex)
            {
                o.Payload = null;
                o.Messages = ex.Message;
            }
            return Json(new { data = o.Payload, message = o.Messages });
        }


        public JsonResult DeletePupuk(string pupukId)
        {
            RepoReturnViewModel<MsPupuk> o = new RepoReturnViewModel<MsPupuk>();
            string returnMessage = string.Empty;
            string strRepo = string.Empty;
            try
            {
                strRepo = _repository.DeletePupuk(Convert.ToInt32(pupukId), _session.NamaAktor);
                o = JsonConvert.DeserializeObject<RepoReturnViewModel<MsPupuk>>(strRepo);
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
