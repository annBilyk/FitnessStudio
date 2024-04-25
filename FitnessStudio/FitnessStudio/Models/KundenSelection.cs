using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessStudio.Models
{
    public class KundenSelection
    {
        public int KundID { get; set; }
        public bool Selected { get; set; }
        public DateTime KundGebDatum { get; set; }
        public string KundVorname { get; set; }
        public string KundNachname { get; set; }
    }
}
