using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MustangClientHandler.EF;
using MustangClientHandler.Enums;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace MustangClientHandler.Reports
{
    public static class ReportManager
    {
        public async static Task<bool> GenerateAsync(ReportType reportType)
        {

            return await Task.Run(()=>ClientReportSync());
        }
        private static bool ClientReportSync()
        {

            msContext _context = new msContext();
            Dictionary<msClient, string> _clients = new Dictionary<msClient, string>();
            foreach(var temp in _context.msClients.Where(cl=>_context.msPayments.Any(pp => cl.ClientId == pp.ClientId)))
            {
                string payments = string.Empty;
                foreach (var cc in _context.msPayments.Where(pay => pay.ClientId == temp.ClientId))
                    payments += $"Payment sum : {cc.PaymentSum}, UserName {_context.msUsers.FirstOrDefault(u => u.UserId == cc.UserId)?.UserName}, Date {cc.PaymentDate} {Environment.NewLine}";
                _clients.Add(temp, payments);
            }
            var exelApp = new Application();
            var _book = exelApp.Workbooks.Add(Type.Missing);
            var sheet  = (Microsoft.Office.Interop.Excel.Worksheet)_book.ActiveSheet;
            int row = 1;
            sheet.Cells[row, 1] = "Client Id";
            sheet.Cells[row, 2] = "Client Name";
            sheet.Cells[row, 3] = "Client payments";
            foreach(var cl in _clients)
            {
                row++;
                sheet.Cells[row, 1] = cl.Key.ClientId;
                sheet.Cells[row, 2] = cl.Key.ClientName;
                sheet.Cells[row, 3] = cl.Value;
            }
            sheet.Columns.AutoFit();
            _book.Save();
            _book.Close();
            return true;
        }
    }
}
