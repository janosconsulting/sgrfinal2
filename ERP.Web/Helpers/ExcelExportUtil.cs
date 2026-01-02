using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace ERP.Web.Helpers
{
    public class ExportPlanSemanalDto
    {
        public string Requerimiento { get; set; }
        public string Detalle { get; set; }
        public string Observacion { get; set; }
        public string EstadoObs { get; set; }
        public string Severidad { get; set; }
        public string RegistradoPor { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
    public static class ExcelExportUtil
    {

        public static byte[] ExportarPlanSemanal(
       List<ExportPlanSemanalDto> data,
       string adicional,
       string usuario,
       string fechaExportacion)
        {
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Datos");

                // ======================
                // CABECERA
                // ======================
                ws.Cells["A1"].Value = "Exportación de Datos - Plan Semanal";
                ws.Cells["A1:G1"].Merge = true;
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.Font.Size = 14;

                ws.Cells["A2"].Value = $"Adicional Seleccionado: {adicional}";
                ws.Cells["A3"].Value = $"Fecha de Exportación: {fechaExportacion}";
                ws.Cells["A4"].Value = $"Usuario que Exporta: {usuario}";

                ws.Cells["A2:A4"].Style.Font.Bold = true;

                // ======================
                // HEADERS
                // ======================
                string[] headers =
                {
                "Requerimiento", "Detalle", "Observación",
                "Estado Obs", "Severidad", "Registrado Por", "Fecha Registro"
            };

                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[6, i + 1].Value = headers[i];
                    ws.Cells[6, i + 1].Style.Font.Bold = true;
                    ws.Cells[6, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[6, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    ws.Cells[6, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // ======================
                // DATA
                // ======================
                int row = 7;

                foreach (var item in data)
                {
                    ws.Cells[row, 1].Value = item.Requerimiento;
                    ws.Cells[row, 2].Value = item.Detalle;
                    ws.Cells[row, 3].Value = item.Observacion;
                    ws.Cells[row, 4].Value = item.EstadoObs;
                    ws.Cells[row, 5].Value = item.Severidad;
                    ws.Cells[row, 6].Value = item.RegistradoPor;
                    ws.Cells[row, 7].Value = item.FechaRegistro?.ToString("yyyy-MM-dd");
                    row++;
                }

                // ======================
                // FORMATOS
                // ======================
                ws.Column(1).Width = 25;
                ws.Column(2).Width = 55;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 15;

                ws.Column(2).Style.WrapText = true;
                ws.Column(3).Style.WrapText = true;

                if (row > 7)
                {
                    var range = ws.Cells[$"A7:G{row - 1}"];
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                return package.GetAsByteArray();
            }
        }
    }
}