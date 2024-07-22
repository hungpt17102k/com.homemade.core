namespace com.homemade.core
{
    public interface IContainer
    {
        bool IsInitialized { get; }

        bool HasService<TService>() where TService : IService;

        TService GetService<TService>() where TService : IService;

        void RegisterService<TService>(TService service) where TService : IService;

        void UnregisterService<TService>() where TService : IService;
    }
}
