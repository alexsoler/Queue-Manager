using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.ViewModels
{
    public class IndexViewModel : IIndexViewModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAsyncRepository<Office> _officeRepository;
        private readonly IAsyncRepository<Ticket> _ticketRepository;
        private readonly IAsyncRepository<Comment> _commentRepository;
        private readonly IAsyncRepository<Media> _mediaRepository;

        public IndexViewModel(UserManager<ApplicationUser> userManager,
            IAsyncRepository<Office> officeRepository,
            IAsyncRepository<Ticket> ticketRepository,
            IAsyncRepository<Comment> commentRepository,
            IAsyncRepository<Media> mediaRepository)
        {
            _userManager = userManager;
            _officeRepository = officeRepository;
            _ticketRepository = ticketRepository;
            _commentRepository = commentRepository;
            _mediaRepository = mediaRepository;
        }

        public int CantUsuarios { get; set; }
        public int CantOficinas { get; set; }
        
        public int CantComentarios { get; set; }
        public int CantMultimedia { get; set; }

        public int CantTickets { get; set; }
        public int CantTicketsLlamados { get; set; }
        public int CantTicketsEspera { get; set; }
        public int CantTicketsAsistencia { get; set; }

        public string[] Months { get; set; } = new string[7];

        public int[] UsuariosMeses { get; set; } = new int[7];
        public int[] OficinasMeses { get; set; } = new int[7];
        public int[] ComentariosMeses { get; set; } = new int[7];
        public int[] MultimediaMeses { get; set; } = new int[7];
        public int[] TicketsProcesadosMeses { get; set; } = new int[7];
        public int[] TicketsNoProcesadosMeses { get; set; } = new int[7];

        public async Task Loaded()
        {
            CantUsuarios = await _userManager.Users.CountAsync();
            CantOficinas = await _officeRepository.CountAsync();
            CantComentarios = await _commentRepository.CountAsync(new CommentSpecification(false));
            CantMultimedia = await _mediaRepository.CountAsync();
            CantTickets = await _ticketRepository.CountAsync();
            CantTicketsEspera = await _ticketRepository.CountAsync(new TicketSpecification(StatusTicket.OnHold));
            CantTicketsLlamados = await _ticketRepository.CountAsync(new TicketSpecification(StatusTicket.Called));
            CantTicketsAsistencia = await _ticketRepository.CountAsync(new TicketSpecification(StatusTicket.InAssistance));

            var month = DateTime.Today.Month;
            var year = DateTime.Today.Year;

            for (int i = 6; i >= 0; i--)
            {
                Months[i] = GetNameMonth(month);

                UsuariosMeses[i] = await _userManager.Users.CountAsync(x => x.CreationDate.Month.Equals(month)
                                        && x.CreationDate.Year.Equals(year));
                OficinasMeses[i] = await _officeRepository.CountAsync(new OfficeSpecification(month, year));
                MultimediaMeses[i] = await _mediaRepository.CountAsync(new MediaSpecification(month, year));
                ComentariosMeses[i] = await _commentRepository.CountAsync(new CommentSpecification(month, year));
                TicketsProcesadosMeses[i] = await _ticketRepository.CountAsync(new TicketSpecification(month, year, StatusTicket.Processed));
                TicketsNoProcesadosMeses[i] = await _ticketRepository.CountAsync(new TicketSpecification(month, year, StatusTicket.NotProcessed));

                if (month == 1)
                {
                    month = 12;
                    year--;
                }     
                else
                    month--;
            }

        }

        private string GetNameMonth(int numberMonth)
        {
            try
            {
                DateTimeFormatInfo formatDate = CultureInfo.CurrentCulture.DateTimeFormat;
                string nameMonth = formatDate.GetMonthName(numberMonth);

                return nameMonth;
            }
            catch
            {
                return "Desconocido";
            }
        }

        public IndexViewModel GetViewModel() => this;
    }
}
