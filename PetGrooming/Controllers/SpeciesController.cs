using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using PetGrooming.Models.ViewModels;
using System.Diagnostics;
namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string Name)
        {
            string query = "insert into species (Name) values (@SpeciesName)";
            SqlParameter sqlparam = new SqlParameter("@SpeciesName", Name);
            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            
            List<Species> species = db.Species.SqlQuery("Select * from Species").ToList();
            return View(species);
        }
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
           
            Species species = db.Species.SqlQuery("select * from species where SpeciesID=@SpeciesID", new SqlParameter("SpeciesID", id)).FirstOrDefault();
            List<Pet> pets = db.Pets.SqlQuery("select * from Pets where SpeciesID=@SpeciesID", new SqlParameter("SpeciesID", id)).ToList();
            if (species == null)
            {
                return HttpNotFound();
            }
            //we have the particular species and the pets that belongs to it
            ShowSpecies viewmodel = new ShowSpecies();
            viewmodel.species = species;
            viewmodel.pets = pets;
            //we can combine the two and send it to the view instead of just one
            return View(viewmodel);
        }
        public ActionResult Update(int id)
        {
            string query = "select * from species where SpeciesID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            Species slectedspecies = db.Species.SqlQuery(query, param).FirstOrDefault();
            return View(slectedspecies);
        }
        [HttpPost]
        public ActionResult Update(int id, string Name)
        {
            string query = "update species set Name=@SpeciesName where SpeciesID=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@SpeciesName", Name);
            sqlparams[1] = new SqlParameter("@id",id);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");

        }
        public ActionResult Delete(int id)
        {
            string query = "delete from species where SpeciesID=@SpeciesID";
            SqlParameter sqlParam = new SqlParameter("@SpeciesID", id);
            db.Database.ExecuteSqlCommand(query, sqlParam);

            return RedirectToAction("List");
        }

       
    }
}