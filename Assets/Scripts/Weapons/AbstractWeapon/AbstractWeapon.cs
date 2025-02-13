using UnityEngine;

public class AbstractWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private AbstractBullet _abstractBullet;
    [SerializeField] private Transform _shootPoint;

    private GarbageManager _garbageManager;

    private void Start()
    {
        _garbageManager = SL.Get<GarbageManager>();
    }

    [ContextMenu("TestShoot")]
    public void Shoot()
    {
        AbstractBullet newBullet = CreateBullet();
        newBullet.Init(transform.forward, 10, 10);
    }

    private AbstractBullet CreateBullet()
    {
        AbstractBullet bullet = _garbageManager.CreateGarbage(_abstractBullet, _shootPoint.position, Quaternion.identity);
        return bullet;
    }

    public void Aim()
    {

    }

    public void ConnectModule(IWeaponModule weaponModule)
    {

    }

    public void Reload()
    {

    }

    public void StopAim()
    {

    }
}
