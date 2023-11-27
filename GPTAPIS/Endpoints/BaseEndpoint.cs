using GPTAPIS.Net.Api;

namespace GPTAPIS.Endpoints;

public abstract class BaseEndpoint
{
    public abstract string Address { get; }
    public abstract HttpClient Client { get; }
    public abstract APIService Service { get; }
    public abstract bool EnableDebug { get; }
}
