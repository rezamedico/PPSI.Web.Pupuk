using System;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Collections.Generic;
using System.Linq;
using PPSI.Web.Pupuk.Models;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PPSI.Web.Pupuk.ViewModels;

namespace PPSI.Web.Pupuk.Repository
{
    public class PurchaseOrderRepository
    {
        //Private Field
        private pupuk_ppsi _db;
        private string returnMessage = string.Empty;

        //Default Constructor
        public PurchaseOrderRepository()
        {
            _db = new pupuk_ppsi();
        }

        //To Be Implemented

    }
}
