﻿using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IMediaService
    {
        Task<Media> AddMediaAsync(Media media);
        Task<Media> GetMediaAsync(int id);
        Task<IEnumerable<Media>> GetAllMediaAsync();
        Task<IEnumerable<Media>> GetAllMediaNotUsedAsync();
        Task<IEnumerable<Media>> GetImages();
        Task<OperationResult> RemoveAsync(int id);
        Task<OperationResult> SetAsUsed(int id);
        Task<OperationResult> SetAsNotUsed(int id);
        Task<OperationResult> EditAsync(Media media);
    }
}
