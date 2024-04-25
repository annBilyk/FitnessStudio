using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedWindow : Window
    {
        static CustomizedWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedWindow), new FrameworkPropertyMetadata(typeof(CustomizedWindow)));
        }

        public CustomizedWindow() : base() { }
    }
}
