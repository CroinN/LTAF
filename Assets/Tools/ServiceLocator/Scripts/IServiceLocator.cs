using System;

public interface IServiceLocator
{
    public event Action<IService> RegisteredEvent;
    public event Action<IService> UnregisteredEvent;
    public bool Has<T>() where T : class, IService;
    public T Get<T>() where T : class, IService;
    public void Request<T>(Action<T> action) where T : class, IService;
    public void Register<T>(T service) where T : class, IService;
    public void Unregister<T>() where T : class, IService;
    public void Unregister<T>(T service) where T : class, IService;
}
