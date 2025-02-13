public interface IWeapon
{
    void Shoot();
    void Reload();
    void Aim();
    void StopAim();
    void ConnectModule(IWeaponModule weaponModule);
}