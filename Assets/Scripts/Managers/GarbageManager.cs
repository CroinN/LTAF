using System.Collections.Generic;
using UnityEngine;

public class GarbageManager : MonoBehaviour, IService
{
    private List<object> _garbageList = new List<object>();

    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    public T CreateGarbage<T>(T garbagePrefab, Vector3 position = default, Quaternion rotation = default) where T : Object
    {
        T garbage = Instantiate(garbagePrefab, position, rotation, transform);

        _garbageList.Add(garbage);

        return garbage;
    }

    public void DestroyGarbage(GameObject garbage)
    {
        Destroy(garbage);

        _garbageList.Remove(garbage);
    }

    public void ClearGarbage()
    {
        foreach (object garbage in _garbageList)
        {
            Destroy((GameObject)garbage);
        }

        _garbageList.Clear();
    }

    public void RegisterService()
    {
        SL.Register(this);
    }

    public void UnregisterService()
    {
        SL.Unregister(this);
    }
}
