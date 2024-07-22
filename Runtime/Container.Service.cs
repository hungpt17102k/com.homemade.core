using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.homemade.core
{
    public partial class Container : MonoBehaviour, IContainer
    {
        public bool IsInitialized { get => initialized; set => initialized = value; }

        private readonly Dictionary<Type, IService> allServices = new Dictionary<Type, IService>();
        private bool initialized;

        protected async UniTask ServiceInjecter()
        {
            int index = 0;
            foreach (var service in allServices.OrderBy(s => s.Value.Priority))
            {
                Debug.Log($"Inject Service {index}: <color=green>[{service.Key.Name}] => [{service.Value.GetType().Name}]</color>");
                await service.Value.OnInitialize();
                index++;
            }
        }

        public TService GetService<TService>() where TService : IService
        {
            if (allServices.TryGetValue(typeof(TService), out var service))
            {
                return (TService)service;
            }

            return default;
        }

        public bool HasService<TService>() where TService : IService
        {
            return allServices.ContainsKey(typeof(TService));
        }

        private void RegisterService<TService>(Type type, TService service) where TService : IService
        {
            if (typeof(IService).IsAssignableFrom(type))
            {
                allServices.Add(type, service);
            }
            else
            {
                Debug.LogError($"[{GetType().Name}] Object '{type.Name}' is not a valid service");
            }
        }

        public void RegisterService<TService>(TService service) where TService : IService
        {
            var type = typeof(TService);
            RegisterService(type, service);
        }

        public void RegisterAndInitService<TService>(TService service) where TService : IService
        {
            var type = typeof(TService);
            if (typeof(IService).IsAssignableFrom(type))
            {
                service?.OnInitialize();
            }

            RegisterService(type, service);
        }

        public void UnregisterService<TService>() where TService : IService
        {
            allServices.Remove(typeof(TService));
        }
    }
}
