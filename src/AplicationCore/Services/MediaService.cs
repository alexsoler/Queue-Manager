using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class MediaService : IMediaService
    {
        private readonly IAsyncRepository<Media> _mediaRepository;
        private readonly IAppLogger<MediaService> _logger;

        public MediaService(IAsyncRepository<Media> mediaRepository,
            IAppLogger<MediaService> logger)
        {
            _mediaRepository = mediaRepository;
            _logger = logger;
        }

        public async Task<Media> AddMediaAsync(Media media)
        {
            _logger.LogInformation($"Se comenzo a guardar un archivo multimedia de nombre { media.Name }");

            var addMedia = await _mediaRepository.AddAsync(media);

            _logger.LogInformation($"Se guardo un archivo multimedia de nombre { media.Name }");

            return addMedia;
        }

        public async Task<OperationResult> EditAsync(Media media)
        {
            var operationResult = new OperationResult { Succeeded = true };
            _logger.LogInformation($"Se comenzo a editar el archivo multimedia de nombre { media.Name }");

            try
            {
                await _mediaRepository.UpdateAsync(media);
                _logger.LogInformation($"Se edito el archivo multimedia de nombre {media.Name}");
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"No se pudo editar el archivo multimedia de nombre {media.Name}." +
                    $" Excepcion: {ex.Message}");
                operationResult.Succeeded = false;
                operationResult.Errors.Add($"No se pudo editar el archivo multimedia de nombre {media.Name}.");
            }

            return operationResult;
        }

        public async Task<IEnumerable<Media>> GetAllMediaAsync()
        {
            return await _mediaRepository.ListAllAsync();
        }

        public async Task<IEnumerable<Media>> GetAllMediaNotUsedAsync()
        {
            return await _mediaRepository.ListAsync(new MediaSpecification());
        }

        public async Task<Media> GetMediaAsync(int id)
        {
            return await _mediaRepository.GetByIdAsync(id);
        }

        public async Task<OperationResult> RemoveAsync(int id)
        {
            var operationResult = new OperationResult { Succeeded = true };
            _logger.LogInformation($"Eliminando el archivo multimedia de id: {id}");

            try
            {
                await _mediaRepository.GetByIdAsync(id).ContinueWith(media =>
                {
                    _mediaRepository.DeleteAsync(media.Result);
                });
                _logger.LogInformation($"Se elimino el archivo multimedia de id: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"No se pudo eliminar el archivo multimedia de id: {id}." +
                    $"Excepcion: {ex.Message}");
            }

            return operationResult;
        }

        public async Task<OperationResult> SetAsNotUsed(int id)
        {
            _logger.LogInformation($"Se va a establecer en no usado el archivo multimedia de id {id}");
            var media = await _mediaRepository.GetByIdAsync(id);
            media.Used = false;

            await _mediaRepository.UpdateAsync(media);
            _logger.LogInformation($"Se establecio en no usado el archivo multimedia de id {id}");
            return new OperationResult { Succeeded = true };
        }

        public async Task<OperationResult> SetAsUsed(int id)
        {
            _logger.LogInformation($"Se va a establecer en usado el archivo multimedia de id {id}");
            var media = await _mediaRepository.GetByIdAsync(id);
            media.Used = true;

            await _mediaRepository.UpdateAsync(media);
            _logger.LogInformation($"Se establecio en usado el archivo multimedia de id {id}");
            return new OperationResult { Succeeded = true };
        }
    }
}
