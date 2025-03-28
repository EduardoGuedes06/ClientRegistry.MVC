using ClientRegistry.Domain;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using ClientRegistry.Domain.Models.Data;
using OfficeOpenXml;
using System.Drawing.Printing;
using System.Reflection;

namespace ClientRegistry.Service.Services
{
    public class ClientService : BaseService, IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository, INotificator notificator)
            : base(notificator)
        {
            _clientRepository = clientRepository;
        }

        public async Task<PagedResult<Client>> GetPaged(string? search, int page, int pageSize)
        {
            return await _clientRepository.GetPaged(search, pageSize, page);
        }

        public async Task<IEnumerable<Client>> GetWhitoutPagination(string? search, int pageSize)
        {
            var pagedResult = await _clientRepository.GetPaged(search, pageSize, page: null);
            return pagedResult.Items;
        }

        public async Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> items)
        {
            if (!items.Any())
            {
                return Array.Empty<byte>();
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = properties[col].Name;
                }

                int row = 2;
                foreach (var item in items)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(item);
                        var cell = worksheet.Cells[row, col + 1];

                        if (value is DateTime dateTimeValue)
                        {
                            cell.Value = dateTimeValue;
                            cell.Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                        }
                        else if (value is bool boolValue)
                        {
                            cell.Value = boolValue ? "Yes" : "No";
                        }
                        else
                        {
                            cell.Value = value;
                        }
                    }
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                return package.GetAsByteArray();
            }
        }



        #region Data

        public async Task<CadastroPorDia> GetCadastrosPorDia(DateTime? startDate, DateTime? endDate)
        {
            var (validStartDate, validEndDate) = GetDateRange(startDate, endDate);
            return await _clientRepository.GetCadastrosPorDia(validStartDate, validEndDate);
        }

        public async Task<ProporcaoTipoPessoa> GetProporcaoTipoPessoa(DateTime? startDate, DateTime? endDate)
        {
            var (validStartDate, validEndDate) = GetDateRange(startDate, endDate);
            return await _clientRepository.GetProporcaoTipoPessoa(validStartDate, validEndDate);
        }

        public async Task<EvolucaoCadastros> GetEvolucaoCadastros(DateTime? startDate, DateTime? endDate)
        {
            var (validStartDate, validEndDate) = GetDateRange(startDate, endDate);
            return await _clientRepository.GetEvolucaoCadastros(validStartDate, validEndDate);
        }

        public async Task<CadastroPorDiaTipo> GetCadastroPorDiaTipo(DateTime? startDate, DateTime? endDate)
        {
            var (validStartDate, validEndDate) = GetDateRange(startDate, endDate);
            return await _clientRepository.GetCadastroPorDiaTipo(validStartDate, validEndDate);
        }

        #endregion

        #region Basics
        public async Task<Client?> GetById(Guid id)
        {
            return await _clientRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _clientRepository.ObterTodos();
        }

        public async Task Post(Client client)
        {

            if (await _clientRepository.Buscar(c => c.Document == client.Document).ContinueWith(t => t.Result.Any()))
            {
                Notificar("Já existe um cliente com este documento cadastrado.");
                return;
            }

            await _clientRepository.Adicionar(client);
        }

        public async Task Update(Client client)
        {
            if (await _clientRepository.Buscar(c => c.Document == client.Document && c.Id != client.Id).ContinueWith(t => t.Result.Any()))
            {
                Notificar("Já existe um cliente com este documento cadastrado.");
                return;
            }

            await _clientRepository.Atualizar(client);
        }

        public async Task Remove(Guid id)
        {
            var client = await _clientRepository.ObterPorId(id);
            if (client == null)
            {
                Notificar("Cliente não encontrado.");
                return;
            }

            await _clientRepository.Remover(id);
        }

        public void Dispose()
        {
            _clientRepository?.Dispose();
        }

        #endregion

        #region Util
        private (DateTime startDate, DateTime endDate) GetDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!endDate.HasValue)
            {
                endDate = new DateTime(DateTime.Now.Year, 12, 31);
            }

            return (startDate.Value, endDate.Value);
        }

        #endregion
    }
}
