using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace projetASP.Models
{
    public class Etudiant
    {
        [Key]
        //[Required]
        public string cne { get; set; }
     
       
        public string nom { get; set; }
        //[Required]
    
        //[Required]
       
        public string prenom { get; set; }
        //[Required]
        public string password { get; set; }
        
        //[Required]
        public string nationalite { get; set; }
        //[Required]
        public string cin { get; set; }

        


       // [Required]


        public string email { get; set; }
        
        //[Required]
        public string phone { get; set; }
        //[Required]
        public string gsm { get; set; }
        //[Required]
        public string address { get; set; }
        //[Required]
        public string ville { get; set; }
        
        //[Required]
        public string typeBac { get; set; }
        //[Required]
        public int anneeBac { get; set; }
        //[Required]
     

        public double noteBac { get; set; }
        //[Required]
        public string mentionBac { get; set; }

        //[Required]
        public double noteFstYear { get; set; }
        //[Required]
        public double noteSndYear { get; set; }

        //[Required]
        public DateTime dateNaiss { get; set; }
        //[Required]
        public string lieuNaiss { get; set; }

        //[Required]


        public string photo_link { get; set; }

        public string choix { get; set; }

        public bool validated = false;
        public bool Validated { get; set; }

        public bool modified = false;
        public bool Modified { get; set; }

        [ForeignKey("Filiere")]
        public Nullable<int> idFil { get; set; }


        public virtual Filiere Filiere { get; set; }
    }
}