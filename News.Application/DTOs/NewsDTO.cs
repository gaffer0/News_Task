using Microsoft.AspNetCore.Http;
using News.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Application.DTOs
{
    public class NewsDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<IFormFile> Image { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
       

        public string Language1 { get; set; }
        public string Translation1Title { get; set; }
        public string Translation1Content { get; set; }

        public string Language2 { get; set; }
        public string Translation2Title { get; set; }
        public string Translation2Content { get; set; }
    }
}