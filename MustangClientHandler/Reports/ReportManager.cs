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
            switch (reportType)
            {
                case ReportType.ClientReport:
                    return await Task.Run(() => ClientReportSync());
                case ReportType.PaymentReport:
                    return await Task.Run(() => PaymentReportSync());
                case ReportType.UserReport:
                    return await Task.Run(() => UserReportSync());
            }
            return false;
        }
        private static bool UserReportSync()
        {
            msContext _context = new msContext();

            var exelApp = new Application();
            var _book = exelApp.Workbooks.Add(Type.Missing);
            var sheet = (Microsoft.Office.Interop.Excel.Worksheet)_book.ActiveSheet;
            int row = 1;
            sheet.Cells[row, 1] = "User Id";
            sheet.Cells[row, 2] = "User Name";
            sheet.Cells[row, 3] = "User TotalHours";
            sheet.Cells[row, 4] = "User Last Visit";

            foreach (var us in _context.msUsers.ToList())
            {
                row++;
                sheet.Cells[row, 1] = us.UserId;
                sheet.Cells[row, 2] = us.UserName;
                sheet.Cells[row, 3] = us.TotalHours;
                sheet.Cells[row, 4] = $"from : {us.UserLastVisitStart} till : {us.UserLastVisitEnd}";
            }
            sheet.Columns.AutoFit();
            _book.Save();
            _book.Close();
            return true;
        }
        private static bool PaymentReportSync()
        {
            msContext _context = new msContext();

            var exelApp = new Application();
            var _book = exelApp.Workbooks.Add(Type.Missing);
            var sheet = (Microsoft.Office.Interop.Excel.Worksheet)_book.ActiveSheet;
            int row = 1;
            sheet.Cells[row, 1] = "Payment Id";
            sheet.Cells[row, 2] = "Client Name";
            sheet.Cells[row, 3] = "User Name";
            sheet.Cells[row, 4] = "Payment Date";
            sheet.Cells[row, 5] = "Payment Sum";

            foreach (var pm in _context.msPayments.ToList())
            {
                row++;
                sheet.Cells[row, 1] = pm.PaymentId;
                sheet.Cells[row, 2] = _context.msClients.FirstOrDefault(cl => cl.ClientId == pm.ClientId)?.ClientName;
                sheet.Cells[row, 3] = _context.msUsers.FirstOrDefault(us => us.UserId == pm.UserId)?.UserName;
                sheet.Cells[row, 4] = pm.PaymentDate;
                sheet.Cells[row, 5] = pm.PaymentSum;
            }
            sheet.Columns.AutoFit();
            _book.Save();
            _book.Close();
            return true;
        }
        private static bool ClientReportSync()
        {

            msContext _context = new msContext();
            Dictionary<msClient, string> _clients = new Dictionary<msClient, string>();
            foreach (var temp in _context.msClients.Where(cl => _context.msPayments.Any(pp => cl.ClientId == pp.ClientId)))
            {
                string payments = string.Empty;
                foreach (var cc in _context.msPayments.Where(pay => pay.ClientId == temp.ClientId))
                    payments += $"Payment sum : {cc.PaymentSum}, UserName {_context.msUsers.FirstOrDefault(u => u.UserId == cc.UserId)?.UserName}, Date {cc.PaymentDate} {Environment.NewLine}";
                _clients.Add(temp, payments);
            }
            var exelApp = new Application();
            var _book = exelApp.Workbooks.Add(Type.Missing);
            var sheet = (Microsoft.Office.Interop.Excel.Worksheet)_book.ActiveSheet;
            int row = 1;
            sheet.Cells[row, 1] = "Client Id";
            sheet.Cells[row, 2] = "Client Name";
            sheet.Cells[row, 3] = "Client payments                           ";
            foreach (var cl in _clients)
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
