#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

#endregion

namespace RAA2_Module_04_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // put any code needed for the form here
            EventAction myAction = new EventAction();
            ExternalEvent myEvent = ExternalEvent.Create(myAction);

            // open form
            MyForm currentForm = new MyForm(myEvent)
            { 
                Width = 300,
                Height = 300,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm.Show();

            // get form data and do something

            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }

    public class EventAction : IExternalEventHandler
    {
        public void Execute(UIApplication uiapp)
        {
            Document Doc = uiapp.ActiveUIDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(Doc);
            collector.OfCategory(BuiltInCategory.OST_TitleBlocks);

            using (Transaction t = new Transaction(Doc))
            {
                t.Start("Create new sheet");

                ViewSheet newSheet;
                if(Globals.IsPlaceholder)
                {
                    newSheet = ViewSheet.CreatePlaceholder(Doc);
                }
                else
                {
                    newSheet = ViewSheet.Create(Doc, collector.FirstElementId());
                }

                newSheet.SheetNumber = Globals.SheetNum;
                newSheet.Name = Globals.SheetName;

                t.Commit();
            }
        }

        public string GetName()
        {
            return "EventAction";
        }
    }
}
