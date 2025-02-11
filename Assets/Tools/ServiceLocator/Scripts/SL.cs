using System;
using UnityEngine;

/// <summary>
/// This is just a wrapper class to make the ServiceLocator class more usable by its shorter name and static methods
/// </summary>
[DefaultExecutionOrder(-99)]
public class SL : MonoBehaviour
{
    public event Action<IService> RegisteredEvent;
    public event Action<IService> UnregisteredEvent;

    [SerializeField] private ServiceLocator _instance;
    private static ServiceLocator _staticInstance;

    private void Awake()
    {
        _staticInstance = _instance;

        _staticInstance.RegisteredEvent += (service) =>
        {
            RegisteredEvent?.Invoke(service);
        };
        
        _staticInstance.UnregisteredEvent += (service) =>
        {
            UnregisteredEvent?.Invoke(service);
        };
    }
    
    public static bool Has<T>() where T : class, IService
    {
        return _staticInstance.Has<T>();
    }

    public static T Get<T>() where T : class, IService
    {
        return _staticInstance.Get<T>();
    }

    public static void Request<T>(Action<T> action) where T : class, IService
    {
        _staticInstance.Request(action);
    }

    public static void Register<T>(T service) where T : class, IService
    {
        _staticInstance.Register(service);
    }

    public static void Unregister<T>() where T : class, IService
    {
        _staticInstance.Unregister<T>();
    }

    public static void Unregister<T>(T service) where T : class, IService
    {
        _staticInstance.Unregister<T>(service);
    }
}
