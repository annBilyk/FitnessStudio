using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedLabel : System.Windows.Controls.Label
    {
        static CustomizedLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedLabel), new FrameworkPropertyMetadata(typeof(CustomizedLabel)));
        }

        public CustomizedLabel() : base()
        {

        }
    }
}
