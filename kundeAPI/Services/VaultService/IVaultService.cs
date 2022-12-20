namespace kundeAPI.Services;

public interface IVaultService
{
    string ConnectionString { get; set; }
    Task Configure();
}