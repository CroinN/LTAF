using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[DefaultExecutionOrder(-100)]
public class ServiceLocator : MonoBehaviour, IServiceLocator
{
    private const int MaxServiceUsageCountToWarn = 5;
    private const int MaxServiceUsageCountToAssert = 50;

    public event Action<IService> RegisteredEvent;
    public event Action<IService> UnregisteredEvent;

    private readonly Dictionary<int, IService> _services = new Dictionary<int, IService>();
    private readonly Dictionary<int, List<Action<IService>>> _requests = new Dictionary<int, List<Action<IService>>>();
    private readonly Dictionary<int, int> _serviceUsageCount = new Dictionary<int, int>();

    public bool Has<T>() where T : class, IService
    {
        int key = GetKey<T>();
        return _services.ContainsKey(key);
    }

    public T Get<T>() where T : class, IService
    {
        int key = GetKey<T>();
        bool isFound = _services.TryGetValue(key, out IService result);

        Assert.IsTrue(isFound, $"{typeof(T)} is not registered.");
        Assert.IsNotNull(result, $"{typeof(T)} is null.");

        return (T)result;
    }

    public void Request<T>(Action<T> action) where T : class, IService
    {
        if (Has<T>())
        {
            action(Get<T>());
        }
        else
        {
            int key = GetKey<T>();

            bool isAlreadyRequested = _requests.TryGetValue(key, out List<Action<IService>> serviceRequests);
            if (isAlreadyRequested)
            {
                serviceRequests.Add((s) => action((T)s));
            }
            else
            {
                _requests.Add(key, new List<Action<IService>> {(s) => action((T) s)});
            }
        }
    }

    public void Register<T>(T service) where T : class, IService
    {
        Assert.IsNotNull(service, $"{typeof(T)} service should not be null.");

        int key = GetKey<T>();
        bool isAlreadyRegistered = Has<T>();
        Assert.IsFalse(isAlreadyRegistered, $"{typeof(T)} service is already registered.");

        if (!isAlreadyRegistered)
        {
            _services.Add(key, service);
            CheckRequests(service);
            RegisteredEvent?.Invoke(service);
        }
    }

    private void CheckRequests<T>(T service) where T : class, IService
    {
        int key = GetKey<T>();
        if (_requests.TryGetValue(key, out List<Action<IService>> actions))
        {
            foreach (Action<IService> action in actions)
            {
                action(service);
            }
            _requests.Remove(key);
        }
    }

    public void Unregister<T>(T service) where T : class, IService
    {
        Unregister<T>();
    }

    public void Unregister<T>() where T : class, IService
    {
        int key = GetKey<T>();
        bool isAlreadyRegistered = _services.TryGetValue(key, out IService result);
        
        Assert.IsTrue(isAlreadyRegistered, $"{typeof(T)} service is not registered. But you try to remove it.");

        if (isAlreadyRegistered)
        {
            _services.Remove(key);
            UnregisteredEvent?.Invoke(result);
        }
    }

    private int GetKey<T>() where T : IService
    {
        //Maybe change key to be the Type itself, rather than the hash code
        return typeof(T).GetHashCode();
    }
}
