using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiHealth.Models
{
    public class Party
    {
        public int PartyId { get; set; }
        [Display(Name="Party Name")]
        public string PartyName { get; set; }     // (Example: Christmas Party, Halloween Party, Birthday Party ...)
        [Display(Name ="Party Date")]
        public DateTime PartyDate { get; set; }
        [Display(Name ="Expected No. of Guests")]
        public int ExpectedNumberOfGuests { get; set; }
        public string Location { get; set; }        // (Example: Sheraton Wall Center)      
    }
}
