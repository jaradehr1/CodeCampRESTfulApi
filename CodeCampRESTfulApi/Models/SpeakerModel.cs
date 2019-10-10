using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Models {
    public class SpeakerModel {
        public int SpeakerId { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string MiddleName { get; set; }
        public string Company { get; set; }
        public string CompanyUrl { get; set; }
        public string BlogUrl { get; set; }
        public string Twitter { get; set; }
        public string GitHub { get; set; }
    }
}
