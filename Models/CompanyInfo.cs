using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models
{
    public class CompanyInfo
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyEmail { get; set; }

        public int InsideDhaka { get; set; }
        public int OutsideDhaka { get; set; }

        public string ReviewTitle { get; set; }
        public string ReviewShortDes { get; set; }
        public string ReviewLongDes { get; set; }
        public string ReviewImageUrlOne { get; set; }
        public string ReviewImageUrlTwo { get; set; }
        public string ReviewImageUrlThree { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}