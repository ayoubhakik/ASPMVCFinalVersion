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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
     

        [Required]
        public string nom { get; set; }
        [Required]
        public string prenom { get; set; }
        public string password { get; set; }
        public string nationalite { get; set; }
        public string cin { get; set; }
        public string cne { get; set; }
        public string email { get; set; }

        public string phone { get; set; }
        
        public string gsm { get; set; }
        public string address { get; set; }
        public string ville { get; set; }


        public string typeBac { get; set; }
        public int anneeBac { get; set; }
        public double noteBac { get; set; }
        public string mentionBac { get; set; }


        public double noteFstYear { get; set; }
        public double noteSndYear { get; set; }
        
        public DateTime dateNaiss { get; set; }
        public string lieuNaiss { get; set; }

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