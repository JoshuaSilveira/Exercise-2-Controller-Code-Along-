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
    public class PetController : Controller
    {
       
        private PetGroomingContext db = new PetGroomingContext();
        
       
        public ActionResult List()
        {
            //How could we modify this to include a search bar?
            List<Pet> pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            return View(pets);
        }
        
        public ActionResult Delete(int id)
        {
            string query = "delete from pets where petid=@PetID";
            SqlParameter sqlParam = new SqlParameter("@PetID", id);
            db.Database.ExecuteSqlCommand(query, sqlParam);

            return RedirectToAction("List");
        }
        


        // GET: Pet/Details/5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        
        [HttpPost]
        public ActionResult Add(string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
              //checking if the model is valid based on the annotations in the model class file
            if (ModelState.IsValid)
            {
                string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)";
                SqlParameter[] sqlparams = new SqlParameter[5]; 
                sqlparams[0] = new SqlParameter("@PetName", PetName);
                sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
                sqlparams[2] = new SqlParameter("@PetColor", PetColor);
                sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
                sqlparams[4] = new SqlParameter("@PetNotes", PetNotes);

                
                db.Database.ExecuteSqlCommand(query, sqlparams);
            }
            
            
            return RedirectToAction("List");
        }


        public ActionResult Add()
        {

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }

        public ActionResult Update(int id)
        {
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid =@PetID",new SqlParameter("@PetID",id)).FirstOrDefault();
            if (selectedpet == null)
            {
                return HttpNotFound();
            }
            string query = "select * from species";
            List<Species> selectedspecies = db.Species.SqlQuery(query).ToList();
            UpdatePet viewmodel = new UpdatePet();
            viewmodel.pet = selectedpet;
            viewmodel.species = selectedspecies;
            return View(viewmodel);
        }
        [HttpPost]
        //this can now take a model as an input as the lamba expression in the view bind values to the input elements
        public ActionResult Update(int id,int speciesID,Pet pet)
        {
            Debug.WriteLine("");
            string query = "update pets set PetName = @PetName, Weight =@PetWeight, color=@PetColor,Notes =@PetNotes,SpeciesID=@SpeciesID where petid =@PetID";
            SqlParameter[] sqlparams = new SqlParameter[6]; 
            sqlparams[0] = new SqlParameter("@PetName", pet.PetName);
            sqlparams[1] = new SqlParameter("@PetWeight",pet.Weight);
            sqlparams[2] = new SqlParameter("@PetColor", pet.Color);
            sqlparams[3] = new SqlParameter("@PetNotes", pet.Notes);
            sqlparams[4] = new SqlParameter("@PetID", id);
            sqlparams[5] = new SqlParameter("@SpeciesID", speciesID);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
