using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;

namespace kundeAPI.Services;
public class VaultService
{
    public string ConnectionString { get; set; } = null!;

    public VaultService()
    {
        Task.Run(() => this.Configure()).Wait();
    }

    public async Task Configure()
    {
        var EndPoint = "https://localhost:8201/";
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, sslPolicyErrors) => { return true; };

        // Initialize one of the several auth methods.
        IAuthMethodInfo authMethod =
            new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
        // Initialize settings. You can also set proxies, custom delegates etc. here.
        var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
        {
            Namespace = "",
            MyHttpClientProviderFunc = handler => new HttpClient(httpClientHandler) {
                BaseAddress = new Uri(EndPoint)
            } 
        };
        IVaultClient vaultClient = new VaultClient(vaultClientSettings);
        Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: "hemmeligheder", mountPoint: "secret");
        ConnectionString = kv2Secret.Data.Data["DB1"].ToString()!;
    }
}