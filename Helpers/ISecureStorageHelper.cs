using MrG.AI.Client.Data;
using System.Threading.Tasks;

namespace MrG.AI.Client.Helpers
{
    public interface ISecureStorageHelper
    {
        Task<bool> WriteServer(Server server);
        Task<Server> GetServer();
        Task<string> ReadName();
        Task<string> ReadIP();
        Task<string> ReadToken();
        bool DeleteVariable(string key);
        Task<string> ReadVariableAsync(string key);
        Task<bool> WriteVariableAsync(string key, string value);
    }
}
