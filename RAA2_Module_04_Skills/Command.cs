#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            EventAction2 myAction2 = new EventAction2();
            ExternalEvent myEvent2 = ExternalEvent.Create(myAction2);

            // open form
            MyForm currentForm = new MyForm(myEvent, myEvent2)
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
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document Doc = uidoc.Document;

            List<ElementId> selectedElems = uidoc.Selection.GetElementIds().ToList();

            TaskDialog.Show("Test", "You selected " + selectedElems.Count.ToString() + " elements.");

            OverrideGraphicSettings newSettings = new OverrideGraphicSettings();

            Color newColor = new Color(255, 0, 0);
            newSettings.SetCutForegroundPatternColor(newColor);
            newSettings.SetSurfaceForegroundPatternColor(newColor);

            FillPatternElement curPatt = GetFillPatternByName(Doc, "<Solid fill>");
            newSettings.SetCutForegroundPatternId(curPatt.Id);
            newSettings.SetSurfaceForegroundPatternId(curPatt.Id);

            using(Transaction t = new Transaction(Doc))
            {
                t.Start("Change element colors");

                foreach(ElementId curId in selectedElems)
                {
                    Doc.ActiveView.SetElementOverrides(curId, newSettings);
                }

                t.Commit();
            }

            //FilteredElementCollector collector = new FilteredElementCollector(Doc);
            //collector.OfCategory(BuiltInCategory.OST_TitleBlocks);

            //using (Transaction t = new Transaction(Doc))
            //{
            //    t.Start("Create new sheet");

            //    ViewSheet newSheet;
            //    if(Globals.IsPlaceholder)
            //    {
            //        newSheet = ViewSheet.CreatePlaceholder(Doc);
            //    }
            //    else
            //    {
            //        newSheet = ViewSheet.Create(Doc, collector.FirstElementId());
            //    }

            //    newSheet.SheetNumber = Globals.SheetNum;
            //    newSheet.Name = Globals.SheetName;

            //    t.Commit();
            //}
        }

        private FillPatternElement GetFillPatternByName(Document doc, string name)
        {
            FillPatternElement curFPE = null;

            curFPE = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, name);

            return curFPE;
        }

        public string GetName()
        {
            return "EventAction";
        }
    }

    public class EventAction2 : IExternalEventHandler
    {
        public void Execute(UIApplication uiapp)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document Doc = uidoc.Document;

            // get all elements in view
            FilteredElementCollector collector = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
            List<Element> viewElements = collector.Cast<Element>().ToList();

            OverrideGraphicSettings newSettings = new OverrideGraphicSettings();

            using (Transaction t = new Transaction(Doc))
            {
                t.Start("Reset elements");

                foreach (Element curElem in viewElements)
                {
                    Doc.ActiveView.SetElementOverrides(curElem.Id, newSettings);
                }

                t.Commit();
            }

           
        }

        public string GetName()
        {
            return "EventAction2";
        }
    }
}
