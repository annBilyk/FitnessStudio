using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedMenuButton : System.Windows.Controls.Button
    {
        static CustomizedMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedMenuButton), new FrameworkPropertyMetadata(typeof(CustomizedMenuButton)));
        }

        public CustomizedMenuButton() : base() { }
    }
}
