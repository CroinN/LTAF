using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ServiceLocatorDebugger : MonoBehaviour
{
    private IServiceLocator _serviceLocator;

    [SerializeField] private List<string> _serviceNames;

    private void Awake()
    {
        _serviceLocator = GetComponent<ServiceLocator>();

#if UNITY_EDITOR
        _serviceLocator.RegisteredEvent += OnServiceRegistered;
        _serviceLocator.UnregisteredEvent += OnServiceUnregistered;
#endif
    }

    private void OnServiceRegistered(IService service)
    {
        _serviceNames.Add(service.ToString());
    }

    private void OnServiceUnregistered(IService service)
    {
        _serviceNames.Remove(service.ToString());
    }
}
