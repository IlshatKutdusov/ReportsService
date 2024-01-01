using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Files
{
    public class FileHelper : IFileHelper
    {
        private class SourceFileData
        {
            public bool DataIsCorrect { get; set; } = true;

            public DataTable DataTable { get; set; }

            public Dictionary<string, string> Providers { get; set; } = new Dictionary<string, string>();
        }

        private IEnumerable<string[]> ReadFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath, Encoding.UTF8).Select(a => a.Split(";"));
            return lines;
        }

        private SourceFileData GetSourceFileData(string filePath)
        {
            var sourceFileData = new SourceFileData();

            var dataTable = new DataTable();
            dataTable.TableName = "CsvFile";

            dataTable.Columns.AddRange(new DataColumn[5]
            {
                new DataColumn("ID", typeof(string)),
                new DataColumn("Name", typeof(string)),
                new DataColumn("Adress", typeof(string)),
                new DataColumn("Service", typeof(string)),
                new DataColumn("Price", typeof(string))
            });

            foreach (var row in ReadFile(filePath))
            {
                if (row.Length == 5)
                {
                    if (!sourceFileData.Providers.ContainsKey(row[1].ToString()))
                        sourceFileData.Providers.Add(row[1].ToString(), row[1].ToString());

                    dataTable.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString());
                }
            }

            if (!headRowIsCorrect(dataTable.Rows[0]))
                sourceFileData.DataIsCorrect = false;

            if (sourceFileData.DataIsCorrect)
                sourceFileData.DataTable = dataTable;

            return sourceFileData;
        }

        private bool headRowIsCorrect(DataRow dataRow)
        {
            if (dataRow[0].ToString() != "ID")
                return false;

            if (dataRow[1].ToString() != "Название")
                return false;

            if (dataRow[2].ToString() != "Адрес")
                return false;

            if (dataRow[3].ToString() != "Услуга")
                return false;

            if (dataRow[4].ToString() != "Стоимость")
                return false;

            return true;
        }

        public async Task<ProvidersResponse> GetProviders(Domain.Entities.File file)
        {
            var csvDataTable = GetSourceFileData(file.Path + file.Name);

            if (!csvDataTable.DataIsCorrect)
                return new ProvidersResponse()
                {
                    Status = "Error",
                    Message = "The data in the uploaded file does not match the pattern!",
                    Done = false
                };

            var providers = new List<string>();
            
            providers.AddRange(csvDataTable.Providers.Keys.Where(x => x != "Название"));

            return new ProvidersResponse()
            {
                Status = "Success",
                Message = "The providers found successfully!",
                Done = true,
                Providers = providers
            };
        }

        public async Task<DefaultResponse> SourceFileDataCheck(Domain.Entities.File file)
        {
            var csvDataTable = GetSourceFileData(file.Path + file.Name);

            if (!csvDataTable.DataIsCorrect)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The data in the uploaded file does not match the pattern!",
                    Done = false
                };

            var reportData = csvDataTable.DataTable;

            if (reportData.Rows.Count <= 1)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The uploaded file is empty or does not match the pattern!",
                    Done = false
                };

            return new DefaultResponse()
            {
                Status = "Success",
                Message = "The data in the uploaded file match the pattern!",
                Done = true
            };
        }
    }
}
