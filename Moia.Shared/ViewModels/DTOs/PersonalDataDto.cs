using Moia.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moia.Shared.ViewModels.DTOs
{
    public class PersonalDataDto : _Model
    {
        public int MuslimeId { get; set; }
        [Required]
        public string NameBeforeFristAr { get; set; }
        public string NameBeforeMiddleAr { get; set; }
        [Required]
        public string NameBeforeLastAr { get; set; }
        [Required]
        public string NameAfter { get; set; }
        
        [Required]
        public string NameAfterEn { get; set; }

        public string NameBeforeFristEn { get; set; }
        public string NameBeforeMiddleEn { get; set; }
        public string NameBeforeLastEn { get; set; }
        [Required]
        public DateTime IslamDate { get; set; }
        //[Required]
        //public DateTime IslamDateHijry { get; set; }
        //[Required]
        public Preacher PreacherName { get; set; }
        [Required]
        public WitnessDto FirstWitness  { get; set; }
        [Required]
        public WitnessDto SecondWitness { get; set; }

    }

}
