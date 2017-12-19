using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPSI.Web.Pupuk.ViewModels
{
    public class UserSession
    {        
        public string Username { get; set; }        
        public int RoleId { get; set; }
        public int AktorId { get; set; }
        public string RoleName { get; set; }
        public string NamaAktor { get; set; }
        public string Email { get; set; }
    }
}
