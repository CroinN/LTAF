using TNRD;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] private SerializableInterface<IWeapon> _weapon;

    public void Shoot()
    {
        _weapon.Value.Shoot();
    }
}
