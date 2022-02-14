using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_4_1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string wallInfo = string.Empty;

            var walls = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .ToList();
            foreach (var wall in walls)
            {
                double wallVol = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                string vol = UnitUtils.ConvertFromInternalUnits(wallVol, UnitTypeId.CubicMeters).ToString();
                wallInfo += $"{wall.Name}\t{vol}{Environment.NewLine}";
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string csv = Path.Combine(desktopPath, "walls.csv");

            File.WriteAllText(csv, wallInfo);

            return Result.Succeeded;
        }
    }
}
