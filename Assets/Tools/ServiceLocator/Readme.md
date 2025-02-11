[Back to Main Page](../../../README.md)

# Service Management System

## Overview

This system is designed to streamline the process of managing services within Unity projects. It provides a central point for registering, retrieving, and managing services throughout the lifecycle of your application. The system now includes an updated `IService` interface for service implementations to enforce registration and unregistration logic, a `ServiceLocator` class for managing services, and a `SL` class for simplified static access to the service locator's functionality.

## Getting Started

### Defining Services

Services should implement the updated `IService` interface, which now includes `RegisterService` and `UnregisterService` methods to be implemented. This change ensures that all services adhere to a standardized lifecycle management process.

```csharp
public class MyService : IService
{
    private void Awake()
    {
        RegisterService();
    }
    
    private void OnDestroy()
    {
        UnregisterService();
    }

    public void RegisterService()
    {
        SL.Register(this);
    }

    public void UnregisterService()
    {
        SL.Unregister<MyService>();
    }
}
```


### Registering and Retrieving Services
Services can be registered and retrieved using the `ServiceLocator` instance directly or via the static `SL` class for convenience.

#### Using `SL` Class
```csharp
SL.Register(new MyService());
var myService = SL.Get<MyService>();
```

### Events
You can subscribe to `RegisteredEvent` and `UnregisteredEvent` to be notified when services are added or removed.
```csharp
SL.RegisteredEvent += service => Debug.Log("Service added");
SL.UnregisteredEvent += service => Debug.Log("Service removed");
```

### Requesting Services
If you need a service that might not be registered yet, you can use the `Request` method. Your callback will be invoked once the service becomes available.
```csharp
SL.Request<MyService>(service => {
// Use the service
});
```

### Checking for Service Availability

You can check if a service is currently registered with the `Has` method.
```csharp
bool isRegistered = SL.Has<MyService>();
```

### Removing Services
Services can be unregister when they are no longer needed.
```csharp
SL.UnregisteredEvent<MyService>();
```

## Implementation Details

- `ServiceLocator` is a MonoBehaviour. It manages service registration and retrieval, leveraging Unity's execution order to ensure it is available before other scripts.
- `SL` provides static access to the `ServiceLocator`, simplifying its usage throughout your project without needing to access the ServiceLocator instance directly.
- Services are identified internally using their type's hash code. Consider changing this approach if you encounter collisions or need more flexibility.

## Conclusion

This service management system offers a robust solution for managing dependencies within your Unity projects. It simplifies the process of working with services, making your code cleaner and more maintainable.


## Best Practices and Considerations

While the `ServiceLocator` and `SL` classes provide a powerful and convenient way to manage services within your Unity project, it's important to use them judiciously to avoid potential pitfalls. Here are some guidelines and considerations:

### Use Sparingly

The Service Locator pattern is incredibly useful for accessing services that are genuinely global in nature (e.g., logging, configuration) or for bridging difficult-to-reach areas of your application. However, its convenience can be deceptive. Overuse can lead to a pattern where dependencies are hidden within the implementation rather than being explicit in your components' interfaces. This can make the system harder to understand, debug, and test.

### Favor Explicit Dependencies Where Practical

Whenever possible, prefer to inject dependencies directly into your classes. This makes the dependencies of your classes explicit, improving code readability and maintainability. It also simplifies unit testing, as dependencies can be easily mocked or replaced.

### Documentation and Comments

For each service registered with the `ServiceLocator`, clearly document its intended use case and any considerations or side effects that other developers should be aware of. If a service is only meant to be used in specific circumstances, make that clear both in the service's documentation and in comments where it's retrieved.

### Monitoring Usage

Regularly review how and where the `ServiceLocator` is being used within your project. If you find that certain services are being accessed excessively from many different points in the code, it might be worth considering if those dependencies could be better managed through other means, such as direct injection into the components that require them.

### Encouraging Responsible Use

Encourage your team to discuss and review any new additions to the `ServiceLocator`. A simple peer review process can help ensure that services added to the locator are truly necessary and used appropriately.
