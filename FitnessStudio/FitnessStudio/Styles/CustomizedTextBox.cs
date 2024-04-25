using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedTextBox : System.Windows.Controls.TextBox
    {
        static CustomizedTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedTextBox), new FrameworkPropertyMetadata(typeof(CustomizedTextBox)));
        }

        public CustomizedTextBox() : base()
        {

        }
    }
}
