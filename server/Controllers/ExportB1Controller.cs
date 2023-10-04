using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B.Data;

namespace B
{
    public partial class ExportB1Controller : ExportController
    {
        private readonly B1Context context;
        private readonly B1Service service;
        public ExportB1Controller(B1Context context, B1Service service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/B1/citizenships/csv")]
        [HttpGet("/export/B1/citizenships/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCitizenshipsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCitizenships(), Request.Query), fileName);
        }

        [HttpGet("/export/B1/citizenships/excel")]
        [HttpGet("/export/B1/citizenships/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCitizenshipsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCitizenships(), Request.Query), fileName);
        }
    }
}
