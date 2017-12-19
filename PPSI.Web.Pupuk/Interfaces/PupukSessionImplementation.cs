using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PPSI.Web.Pupuk.ViewModels;
using PPSI.Web.Pupuk.Helpers;

namespace PPSI.Web.Pupuk.Interfaces
{
    public class PupukSessionImplementation : IPupukSession
    {
        readonly IHttpContextAccessor _accessor;
        UserSession _sessionContract;

        public PupukSessionImplementation(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
            _sessionContract = _accessor.HttpContext.Session.GetSession<UserSession>(Helpers.Statics.SessionStatic.SessionName);
        }

        public UserSession GetContract => _accessor.HttpContext.Session.GetSession<UserSession>(Helpers.Statics.SessionStatic.SessionName);

        public UserSession SetContract
        {
            set
            {
                //clear existing session
                ClearSession();

                _sessionContract = value;
                _accessor.HttpContext.Session.SetSession(Helpers.Statics.SessionStatic.SessionName, _sessionContract);
            }
        }

        public string Username
        {
            get
            {
                if (IfContractAvailable())
                {
                    return _sessionContract.Username;
                }
                return null;
            }
        }

        public int RoleId
        {
            get
            {
                if (IfContractAvailable())
                {
                    return _sessionContract.RoleId;
                }
                return 0;
            }
        }

        public int AktorId
        {
            get
            {
                if (IfContractAvailable())
                {
                    return _sessionContract.AktorId;
                }
                return 0;
            }
        }

        public string RoleName
        {
            get
            {
                if (IfContractAvailable())
                {
                    return _sessionContract.RoleName;
                }
                return null;
            }
        }

        public string NamaAktor
        {
            get
            {
                if (IfContractAvailable())
                {
                    return _sessionContract.NamaAktor;
                }
                return null;
            }
        }

        public string Email
        {
            get
            {
                if (IfContractAvailable())
                {
                    return _sessionContract.NamaAktor;
                }
                return null;
            }
        }

        public void ClearSession()
        {
            _accessor.HttpContext.Session.Clear();
        }

        public bool IfContractAvailable()
        {
            _sessionContract = _accessor.HttpContext.Session.GetSession<UserSession>(Helpers.Statics.SessionStatic.SessionName);
            if (_sessionContract == null) { return false; }
            return true;
        }
    }
}
