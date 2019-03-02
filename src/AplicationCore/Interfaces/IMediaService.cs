using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IMediaService
    {
        Task<Media> AddMedia(Media media);
        Task<Media> GetMediaAsync(int id);
        Task<IEnumerable<Media>> GetAllMediaAsync();
        Task<OperationResult> RemoveAsync(int id);
        Task<OperationResult> EditAsync(Media media);
    }
}
