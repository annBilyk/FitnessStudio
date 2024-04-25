using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedComboBox : System.Windows.Controls.ComboBox
    {
        static CustomizedComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedComboBox), new FrameworkPropertyMetadata(typeof(CustomizedComboBox)));
        }

        public CustomizedComboBox() : base()
        {

        }
    }
}
