//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Faps.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Candidaturas
    {
        public int Codigo_Candidatura { get; set; }
        public int Codigo_user { get; set; }

        [Display(Name = "Status da Candidatura")]
        public int Status_candidatura { get; set; }
        public int Codigo_Vaga { get; set; }
    
        public virtual Usuarios Usuarios { get; set; }
        public virtual Vagas Vagas { get; set; }
    }
}
