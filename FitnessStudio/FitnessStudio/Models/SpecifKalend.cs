using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessStudio.Models
{
    public class SpecifKalend
    {
        public int SpecifKalendID { get; set; }
        public int SpecifKalendKundID { get; set; }
        public DateTime SpecifKalendDatum { get; set; }
        public TimeSpan SpecifKalendZeit { get; set; }
        public Kunden SpecifKalendKunden { get; set; }
        public Trainer SpecifKalendTrainer { get; set; }
        public string SpecifKalendAktivitat { get; set; }
    }
}
