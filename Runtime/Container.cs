using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.homemade.core
{
    public abstract class Container<T> : Container where T : Container<T>, new()
    {
        public static Container Instance => Lazy.Value;

        private static Lazy<T> _lazy;

        private static Lazy<T> Lazy
        {
            get => _lazy ??= new Lazy<T>(LazyCreate);
            set => _lazy = value;
        }

        private static T LazyCreate()
        {
            var instance = FindObjectOfType<T>(true);
            if (instance is null)
            {
                GameObject ownerObject = new GameObject($"[Container] {typeof(T).Name}");
                DontDestroyOnLoad(ownerObject);
                instance = ownerObject.AddComponent<T>();
                Initialize(instance);
            }

            return instance;
        }

        private static async void Initialize(T container)
        {
            await container.OnPreInitialize();
            await container.OnInitialize();
            await container.OnPostInitialize();
            container.IsInitialized = true;
        }

        protected virtual UniTask OnPreInitialize()
        {
            return UniTask.CompletedTask;
        }

        protected virtual async UniTask OnInitialize()
        {
            await ServiceInjecter();
        }

        protected virtual UniTask OnPostInitialize()
        {
            return UniTask.CompletedTask;
        }
    }
}
