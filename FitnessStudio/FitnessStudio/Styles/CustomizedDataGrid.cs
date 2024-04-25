using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.Styles
{
    public class CustomizedDataGrid : System.Windows.Controls.DataGrid
    {
        static CustomizedDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedDataGrid),
                new FrameworkPropertyMetadata(typeof(CustomizedDataGrid)));
        }

        public CustomizedDataGrid() : base()
        {
        }
    }
}
