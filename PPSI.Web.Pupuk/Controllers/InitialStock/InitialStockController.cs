using System;
using System.Collections.Generic;
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

namespace PPSI.Web.Pupuk.Controllers.InitialStock
{
    [PupukActionFilter]
    public class InitialStockController : Controller
    {
        #region Private Field
        private InventoryRepository _repository = new InventoryRepository();
        private FunctionHelperRepository _repoHelper = new FunctionHelperRepository();
        IPupukSession _session;
        #endregion

        public InitialStockController(IPupukSession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }
    }   
}
