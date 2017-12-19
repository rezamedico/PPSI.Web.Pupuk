using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PPSI.Web.Pupuk.Helpers;
using PPSI.Web.Pupuk.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPSI.Web.Pupuk.Filters
{
    public partial class PupukActionFilter : ActionFilterAttribute
    {
        public PupukActionFilter() { }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //var id = User.Identity.IsAuthenticated;
            //check for session
            var session = context.HttpContext.Session.GetSession<UserSession>(Helpers.Statics.SessionStatic.SessionName);
            if (session == null)
            {
                // var returnUrl = context.HttpContext.Request.Path.Value;
                context.Result = new RedirectResult($"~/Login/Index");
                return;
            }

            //session exist, compare session ID
            //_db = (MedicoDbContext)context.HttpContext.ApplicationServices.GetService(typeof(MedicoDbContext));

            //if (_db.AspNetUsers.Any(x => x.MetaData == session.Id)) {
            //    //dual login
            //}
            base.OnActionExecuting(context);
        }

        //internal string CheckAreaExist(ActionExecutingContext context)
        //{
        //    string _area = null;
        //    try
        //    {
        //        _area = context.RouteData.Values["area"].ToString();
        //        // TODO:urgent  role = db.AspNetRoles.FirstOrDefault(x => x.Action == _action && x.Controller == _controller && x.Area == _area);
        //    }
        //    catch
        //    {
        //        _area = null;
        //        // TODO:urgent  role = db.AspNetRoles.FirstOrDefault(x => x.Action == _action && x.Controller == _controller);
        //    }
        //    return _area;
        //}

        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    if (context.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        context.Result = null;
        //    }
        //    //context.Result= new RedirectResult("/account/login");
        //    context.Result = context.Result;
        //}


        //protected void NotAuthorizeAction(AuthorizationFilterContext context)
        //{
        //    context.Result = new RedirectResult("/account/login");
        //}
    }
}
