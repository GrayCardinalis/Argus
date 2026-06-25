using Argus.Dtos.Auditoriums;
using Argus.Dtos.Components;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Services.Interfaces
{
    public interface IAuditoriumService
    {
        Task<List<AuditoriumDto>> GetAllAuditoriumsAsync();
        Task<AuditoriumDto?> GetAuditoriumByIdAsync(Guid id); //? because if the Auditorium with the requested id is not found in the database.
        Task<AuditoriumDto> CreateAuditoriumAsync(CreateAuditoriumDto createAuditoriumDto);
        Task<bool> DeleteAuditoriumAsync(Guid id);
    }
}
