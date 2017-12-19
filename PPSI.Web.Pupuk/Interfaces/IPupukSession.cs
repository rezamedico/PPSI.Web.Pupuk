using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPSI.Web.Pupuk;
using PPSI.Web.Pupuk.ViewModels;

namespace PPSI.Web.Pupuk.Interfaces
{
    public interface IPupukSession
    {
        UserSession GetContract { get; }
        UserSession SetContract { set; }
        void ClearSession();
        bool IfContractAvailable();
        string Username { get; }
        int RoleId { get; }
        int AktorId { get; }
        string RoleName { get; }
        string NamaAktor { get; }
        string Email { get; }
    }
}
