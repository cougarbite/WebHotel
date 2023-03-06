using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using WebHotels.WebUI.Data;

namespace WebHotels.WebUI.Controllers
{
    public partial class ExportWebHotelsDBController : ExportController
    {
        private readonly WebHotelsDBContext context;
        private readonly WebHotelsDBService service;

        public ExportWebHotelsDBController(WebHotelsDBContext context, WebHotelsDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/WebHotelsDB/clients/csv")]
        [HttpGet("/export/WebHotelsDB/clients/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetClients(), Request.Query), fileName);
        }

        [HttpGet("/export/WebHotelsDB/clients/excel")]
        [HttpGet("/export/WebHotelsDB/clients/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetClients(), Request.Query), fileName);
        }

        [HttpGet("/export/WebHotelsDB/employees/csv")]
        [HttpGet("/export/WebHotelsDB/employees/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmployeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEmployees(), Request.Query), fileName);
        }

        [HttpGet("/export/WebHotelsDB/employees/excel")]
        [HttpGet("/export/WebHotelsDB/employees/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmployeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEmployees(), Request.Query), fileName);
        }
    }
}
