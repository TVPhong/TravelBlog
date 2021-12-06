using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDuLich.Models
{
    public class Travel
    {
       
        public int Id { get; set; }

        [Required, DisplayName("Tên khu du lịch")]
        public string Name { get; set; }

        [Required, DisplayName("Địa chỉ khu du lịch")]
        public string Adress { get; set; }

        [DisplayName(" Unesco công nhận năm")]
        public int Year { get; set; }

        [DisplayName(" Miêu tả địa điểm du lịch ")]
        public string Descriptions { get; set; }

        [DisplayName(" Cảm xúc về địa điểm du lịch đó ")]
        public string Emotions { get; set; }
        public string DataFile { get; set; }
        //public IFormFile file { get; set; }
    }
}
