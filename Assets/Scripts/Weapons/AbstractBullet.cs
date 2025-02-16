using DG.Tweening;
using UnityEngine;

public class AbstractBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private GarbageManager _garbageManager;

    public void Init(Vector3 direction, float damage, float speed, float lifeTime)
    {
        _rigidbody.linearVelocity = direction * speed;
        _rigidbody.angularVelocity = Random.insideUnitSphere * 5;

        KillTimer(lifeTime);
    }

    private void KillTimer(float lifeTime)
    {
        _garbageManager = SL.Get<GarbageManager>();
        DOVirtual.DelayedCall(lifeTime, () => _garbageManager.DestroyGarbage(gameObject));
    }
}
