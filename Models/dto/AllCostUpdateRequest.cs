using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPage.Models.dto
{
    public class AllCosUpdateRequest
    {
        public int Id { get; set; }
        public int InsideDhaka { get; set; }
        public int OutsideDhaka { get; set; }
        public int PickUp { get; set; }

    }
}