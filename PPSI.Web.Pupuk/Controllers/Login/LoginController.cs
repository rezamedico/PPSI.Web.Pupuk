using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPSI.Web.Pupuk.Models;
using PPSI.Web.Pupuk.Helpers;
using Microsoft.AspNetCore.Http;
using PPSI.Web.Pupuk.Repository;
using PPSI.Web.Pupuk.ViewModels;
using PPSI.Web.Pupuk.Interfaces;
//using Newtonsoft.Json;
//using DataTables.AspNet.Core;
//using DataTables.AspNet.AspNetCore;

namespace PPSI.Web.Pupuk.Controllers
{
    public class LoginController : Controller
    {
        private FunctionHelperRepository _helperRepo = new FunctionHelperRepository();       
        IPupukSession _session;
        
        public LoginController(IPupukSession session)
        {
            _session = session;            
        }

        public IActionResult Index()
        {
            _session.ClearSession();
            return View();
        }

        public IActionResult Login(string username, string password)
        {
            string messages = String.Empty;
            Vlogin vlogin = null;
            try
            {
                vlogin = _helperRepo.LoginValidation(username, password);
                if(vlogin == null)
                {
                    _session.ClearSession();
                    messages = "Please Input Valid Username And Password";
                }
                else
                {
                    //Set The Session
                    UserSession _userSession = new UserSession();
                    _userSession.Username = vlogin.Username;
                    _userSession.RoleId = vlogin.RoleId;
                    _userSession.RoleName = vlogin.RoleName;
                    _userSession.AktorId = vlogin.AktorId;
                    _userSession.NamaAktor = vlogin.NamaAktor;
                    _userSession.Email = vlogin.Email;
                    _session.SetContract = _userSession;
                    messages = "Username And Password Match";
                }                
            }
            catch (Exception ex)
            {
                vlogin = null;
                messages = ex.Message;
            }

            return RedirectToAction("Index", "Petani", new { area = "Master" });
        }

        public IActionResult Logout()
        {
            _session.ClearSession();
            return RedirectToAction("Index");
        }
                
    }
}
