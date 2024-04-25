using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedButton : System.Windows.Controls.Button
    {
        static CustomizedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedButton), new FrameworkPropertyMetadata(typeof(CustomizedButton)));
        }

        public CustomizedButton() : base() { }
    }
}
