using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ReportService : IReportService
    {
        private readonly IAsyncRepository<Ticket> _ticketRepository;
        private readonly IAppLogger<ReportService> _logger;

        public ReportService(IAsyncRepository<Ticket> ticketRepository,
            IAppLogger<ReportService> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsForOffice(int idOffice, DateTime? initialDate, DateTime? endDate)
        {
            _logger.LogInformation("Obteniendo la lista de personas atendidas por oficina");

            IEnumerable<Ticket> tickets = new List<Ticket>();

            try
            {
                tickets = await _ticketRepository.ListAsync(new TicketSpecification(idOffice, initialDate, endDate));

                _logger.LogInformation("Se obtuvo la lista de personas atendidas por oficina");

                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("No se pudo obtener la lista, ocurrio la siguiente excepción: {0}", ex);
            }

            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsForOperator(string idOperator, DateTime? initialDate, DateTime? endDate)
        {
            _logger.LogInformation("Obteniendo la lista de personas atendidas por operador");

            IEnumerable<Ticket> tickets = new List<Ticket>();

            try
            {
                tickets = await _ticketRepository.ListAsync(new TicketSpecification(idOperator, initialDate, endDate));

                _logger.LogInformation("Se obtuvo la lista de personas atendidas por operador");

                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("No se pudo obtener la lista, ocurrio la siguiente excepción: {0}", ex);
            }

            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsForPeriod(DateTime? initialDate, DateTime? endDate)
        {
            _logger.LogInformation("Obteniendo la lista de tickets emitidos por periodo");

            IEnumerable<Ticket> tickets = new List<Ticket>();

            try
            {
                tickets = await _ticketRepository.ListAsync(new TicketSpecification(initialDate, endDate));
                _logger.LogInformation("Se obtuvo la lista de tickets emitidos por periodo");

                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("No se pudo obtener la lista, ocurrio la siguiente excepción: {0}", ex);
            }

            return tickets;
        }
    }
}
