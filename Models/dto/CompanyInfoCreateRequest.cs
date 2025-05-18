using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class CompanyInfoCreateRequest
    {
        public string CompanyName { get; set; }
        public IFormFile? CompanyLogoUrl { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyEmail { get; set; }
        public string InsideDhaka { get; set; }
        public string OutsideDhaka { get; set; }

        public string ReviewTitle { get; set; }
        public string ReviewShortDes { get; set; }
        public string ReviewLongDes { get; set; }
        public IFormFile? ReviewImageUrlOne { get; set; }
        public IFormFile? ReviewImageUrlTwo { get; set; }
        public IFormFile? ReviewImageUrlThree { get; set; }

        public string UserId { get; set; }

    }
}