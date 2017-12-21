using System;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Collections.Generic;
using System.Linq;
using PPSI.Web.Pupuk.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinqToDB;

namespace PPSI.Web.Pupuk.Repository
{
    public class FunctionHelperRepository
    {
        //Private Field
        private pupuk_ppsi _db;
        private string exception = string.Empty;

        //Default Constructor
        public FunctionHelperRepository()
        {
            _db = new pupuk_ppsi();
        }
        
        public List<Refprovinsi> GetDropdownProvinsi()
        {
            List<Refprovinsi> lRefProvinsi = new List<Refprovinsi>();
            try
            {
                lRefProvinsi = (from a in _db.Refprovinsis
                                select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lRefProvinsi = null;
            }
            return lRefProvinsi;
        }

        public List<Refkota> GetDropdownKabupaten()
        {
            List<Refkota> lRefKota = new List<Refkota>();
            try
            {
                lRefKota = (from a in _db.Refkotas
                                select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lRefKota = null;
            }
            return lRefKota;
        }

        public List<Refkota> GetDropdownKabupatenByProvinsiId(int provinsiId)
        {
            List<Refkota> lRefKota = new List<Refkota>();
            try
            {
                lRefKota = (from a in _db.Refkotas
                            where a.ProvinsiId == provinsiId
                            select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lRefKota = null;
            }
            return lRefKota;
        }


        public List<Refarea> GetDropdownKecamatan()
        {
            List<Refarea> lRefKecamatan = new List<Refarea>();
            try
            {
                lRefKecamatan = (from a in _db.Refareas
                                select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lRefKecamatan = null;
            }
            return lRefKecamatan;
        }

        public List<Refarea> GetDropdownKecamatanByKotaId(int kotaId)
        {
            List<Refarea> lRefKecamatan = new List<Refarea>();
            try
            {
                lRefKecamatan = (from a in _db.Refareas
                                 where a.KotaId == kotaId
                                 select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lRefKecamatan = null;
            }
            return lRefKecamatan;
        }

        public List<MsAktor> GetDropdownAktor()
        {
            List<MsAktor> lRefAktor = new List<MsAktor>();
            try
            {
                lRefAktor = (from a in _db.MsAktor                             
                             select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lRefAktor = null;
            }
            return lRefAktor;
        }

        public List<MsAktor> GetDropdownAktorByRoleId(int RoleId)
        {
            List<MsAktor> lRefAktor = new List<MsAktor>();
            try
            {
                lRefAktor = (from a in _db.MsAktor
                             where a.RoleId == RoleId
                             select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lRefAktor = null;
            }
            return lRefAktor;
        }

        public List<StaticData> GetDropdownStaticDataBySection(int section)
        {
            List<StaticData> lStaticData = new List<StaticData>();
            try
            {
                lStaticData = (from a in _db.StaticData
                               where a.Section == section
                               select a).ToList();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                lStaticData = null;
            }
            return lStaticData;
        }

        //Sequence Number
        public string GetPetaniCode()
        {
            string code = string.Empty;
            SequenceNumbers o = new SequenceNumbers();
            try
            {
                var seq = (from a in _db.SequenceNumbers
                           where a.FormatKode.Contains("PTN")
                           select a).AsNoTracking().First();
                string[] namesArray = seq.FormatKode.Split('-');
                code = namesArray[0] + (seq.Nilai + 1).ToString("D4");
                o = seq;
                o.Nilai = o.Nilai + 1;
                o.Date = DateTime.Now.Date;
                _db.Update(o);
            }
            catch
            {
                code = string.Empty;
            }
            return code;
        }

        public string GetDistributorCode()
        {
            string code = string.Empty;
            SequenceNumbers o = new SequenceNumbers();
            try
            {
                var seq = (from a in _db.SequenceNumbers
                           where a.FormatKode.Contains("DST")
                           select a).AsNoTracking().First();
                string[] namesArray = seq.FormatKode.Split('-');
                code = namesArray[0] + (seq.Nilai + 1).ToString("D4");
                o = seq;
                o.Nilai = o.Nilai + 1;
                o.Date = DateTime.Now.Date;
                _db.Update(o);
            }
            catch
            {
                code = string.Empty;
            }
            return code;
        }

        public string GetProdusenCode()
        {
            string code = string.Empty;
            SequenceNumbers o = new SequenceNumbers();
            try
            {
                var seq = (from a in _db.SequenceNumbers
                           where a.FormatKode.Contains("PRO")
                           select a).AsNoTracking().First();
                string[] namesArray = seq.FormatKode.Split('-');
                code = namesArray[0] + (seq.Nilai + 1).ToString("D4");
                o = seq;
                o.Nilai = o.Nilai + 1;
                o.Date = DateTime.Now.Date;
                _db.Update(o);
            }
            catch
            {
                code = string.Empty;
            }
            return code;
        }

        public string GetStoreCode()
        {
            string code = string.Empty;
            SequenceNumbers o = new SequenceNumbers();
            try
            {
                var seq = (from a in _db.SequenceNumbers
                           where a.FormatKode.Contains("STORE")
                           select a).AsNoTracking().First();
                string[] namesArray = seq.FormatKode.Split('-');
                code = namesArray[0] + (seq.Nilai + 1).ToString("D4");
                o = seq;
                o.Nilai = o.Nilai + 1;
                o.Date = DateTime.Now.Date;
                _db.Update(o);
            }
            catch
            {
                code = string.Empty;
            }
            return code;
        }

        public Vlogin LoginValidation(string username, string password)
        {
            Vlogin o = new Vlogin();
            try
            {
                o = _db.Vlogins.Where(x => x.Username == username && x.Password == password).FirstOrDefault();                
            }
            catch (Exception ex)
            {
                o = null;
                exception = ex.Message;
            }

            return o;
        }
    }
}
