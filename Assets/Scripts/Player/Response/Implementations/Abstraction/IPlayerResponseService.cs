using CodeScripts.Abstraction;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace CodeScripts.PlayerResponse.Implementations.Abstraction
{
    public interface IPlayerResponseService
    {
        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData;
        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData;
    }
}