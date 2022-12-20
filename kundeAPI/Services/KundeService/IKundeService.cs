using kundeAPI.Models;

namespace kundeAPI.Services;

public interface IKundeService
{
    Task<List<Kunde>> GetAsync();
    Task<Kunde?> GetAsync(string id);
    Task CreateAsync(Kunde newKunde);
    Task UpdateAsync(string id, Kunde updatedKunde);
    Task RemoveAsync(string id);
}