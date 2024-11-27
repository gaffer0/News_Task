using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.Entities
{
    public class NewTranslation
    {
        [Key]
        public int TranslationId { get; set; }
        [ForeignKey("New")]
        public int NewId { get; set; }
        public string Language1 {  get; set; }
        public string Language1Title { get; set; }
        public string Language1Content { get; set; }
        public string Language2 { get; set; }
        public string Language2Title { get; set; }
        public string Language2Content { get; set; }
        public New New { get; set; }


    }
}
