using System.Threading.Tasks;

namespace Bifrost.Editor
{
    public interface IBifrostLLMProvider
    {
        string Name { get; }
        Task<string> CompleteAsync(string prompt, string model, string apiKey, string endpoint);
        Task<bool> TestConnectionAsync(string apiKey, string endpoint, string model);
    }
} 