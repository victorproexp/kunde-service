using kundeAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace kundeAPI.Services;

public class KundeService : IKundeService
{
    private readonly IMongoCollection<Kunde> _kundeCollection;
    private readonly ILogger<KundeService> _logger;

    public KundeService(
        IOptions<KundeDatabaseSettings> kundeDatabaseSettings, ILogger<KundeService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _logger.LogInformation(configuration["ConnectionString"]);
        
        var mongoClient = new MongoClient(configuration["ConnectionString"]);

        var mongoDatabase = mongoClient.GetDatabase(
            kundeDatabaseSettings.Value.DatabaseName);

        _kundeCollection = mongoDatabase.GetCollection<Kunde>(
            kundeDatabaseSettings.Value.KundeCollectionName);
    }

    public async Task<List<Kunde>> GetAsync() =>
        await _kundeCollection.Find(_ => true).ToListAsync();

    public async Task<Kunde?> GetAsync(string id) =>
        await _kundeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Kunde newKunde) =>
        await _kundeCollection.InsertOneAsync(newKunde);

    public async Task UpdateAsync(string id, Kunde updatedKunde) =>
        await _kundeCollection.ReplaceOneAsync(x => x.Id == id, updatedKunde);

    public async Task RemoveAsync(string id) =>
        await _kundeCollection.DeleteOneAsync(x => x.Id == id);
}