using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedCheckBox : System.Windows.Controls.CheckBox
    {
        static CustomizedCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedCheckBox), new FrameworkPropertyMetadata(typeof(CustomizedCheckBox)));
        }

        public CustomizedCheckBox() : base()
        {

        }
    }
}
