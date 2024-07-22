using Cysharp.Threading.Tasks;

namespace com.homemade.core
{
    public interface IService
    {
        int Priority { get; }

        UniTask OnInitialize();
    }
}
