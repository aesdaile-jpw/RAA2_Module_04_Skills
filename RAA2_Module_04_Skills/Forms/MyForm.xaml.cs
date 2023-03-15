using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RAA2_Module_04_Skills
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MyForm : Window
    {
        ExternalEvent myEvent;
        ExternalEvent myEvent2;
        public MyForm(ExternalEvent _event, ExternalEvent _event2)
        {
            InitializeComponent();
            myEvent = _event;
            myEvent2 = _event2;
            
        }

        private void btnButton_Click(object sender, RoutedEventArgs e)
        {
            if(cbxBoolean.IsChecked == true)
            {
                Globals.IsPlaceholder= true;
            }
            else
            {
                Globals.IsPlaceholder = false;
            }

            Globals.SheetNum = tbxSheetNum.Text;
            Globals.SheetName = tbxSheetName.Text;

            myEvent.Raise();

        }

        private void btnButton2_Click(object sender, RoutedEventArgs e)
        {
            myEvent2.Raise();
        }
    }
}
