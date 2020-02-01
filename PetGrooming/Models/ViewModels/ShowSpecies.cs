using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class ShowSpecies
    {
        public Species species { get; set; }
        public List<Pet> pets { get; set; }

    }
}