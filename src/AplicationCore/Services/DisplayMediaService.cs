using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class DisplayMediaService : IDisplayMediaService
    {
        private readonly IAsyncRepository<DisplayMedia> _displayMediaRepository;
        private readonly IAppLogger<DisplayMediaService> _logger;

        public DisplayMediaService(IAsyncRepository<DisplayMedia> displayMediaRepository,
            IAppLogger<DisplayMediaService> logger)
        {
            _displayMediaRepository = displayMediaRepository;
            _logger = logger;
        }

        public async Task<DisplayMedia> AddAsync(DisplayMedia displayMedia)
        {
            _logger.LogInformation($"Se va a asignar el archivo multimedia " +
                $"de id {displayMedia.Id} a la pantalla de visualización");

            await _displayMediaRepository.AddAsync(displayMedia);

            _logger.LogInformation($"Se asigno el archivo multimedia de id {displayMedia.Id} a la pantalla de visualización");

            return displayMedia;
        }

        public async Task<IEnumerable<DisplayMedia>> ListAllAsync()
        {
            return await _displayMediaRepository.ListAsync(new DisplayMediaSpecification());
        }

        public async Task<OperationResult> RemoveAsync(int id)
        {
            _logger.LogInformation($"Se va a quitar el archivo multimedia de id {id} de la pantalla de visualización");

            var media = await _displayMediaRepository.GetByIdAsync(id);

            await _displayMediaRepository.DeleteAsync(media);

            _logger.LogInformation($"Se quito el archivo multimedia de id {id} de la pantalla de visualización");

            return new OperationResult { Succeeded = true };
        }

        public async Task<OperationResult> UpdateOrder(int id, double order)
        {
            _logger.LogInformation($"Se va a cambiar el orden de reproduccion de un archivo multimedia" +
                $" cuyo id de registro es {id}.");

            var displayMedia = await _displayMediaRepository.GetByIdAsync(id);
            displayMedia.Order = order;
            await _displayMediaRepository.UpdateAsync(displayMedia);

            _logger.LogInformation($"Se cambio el orden de reproduccion de un archivo cuyo de id de" +
                $" registro es {id} al siguiente numero de orden {order}");

            return new OperationResult() { Succeeded = true };
        }
    }
}
