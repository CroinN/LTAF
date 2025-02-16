using System;

[Serializable] public struct WeaponStats
{
    public float damage;
    public float bulletSpeed;
    public float bulletLifetime;
    public float fireRate;
    public float reloadTime;
}

public interface IWeapon
{
    void Shoot();
    void StopShoot();
    void Reload();
    void Aim();
    void StopAim();
    void ConnectModule(IWeaponModule weaponModule);
}