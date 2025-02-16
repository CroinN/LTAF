using UnityEngine;

public class AbstractWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private AbstractBullet _abstractBullet;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private WeaponStats _weaponStats;

    private GarbageManager _garbageManager;

    private void Start()
    {
        _garbageManager = SL.Get<GarbageManager>();
    }

    public void Shoot()
    {
        AbstractBullet newBullet = CreateBullet();
        newBullet.Init(transform.forward, _weaponStats.damage, _weaponStats.bulletSpeed, _weaponStats.bulletSpeed);
    }

    private AbstractBullet CreateBullet()
    {
        AbstractBullet bullet = _garbageManager.CreateGarbage(_abstractBullet, _shootPoint.position, Quaternion.identity);
        return bullet;
    }

    public void StopShoot()
    {

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
