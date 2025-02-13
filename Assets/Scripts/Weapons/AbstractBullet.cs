using UnityEngine;

public class AbstractBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private float _speed;
    private float _damage;

    public void Init(Vector3 direction, float damage, float speed)
    {
        _rigidbody.linearVelocity = direction * speed;
        _rigidbody.angularVelocity = Random.insideUnitSphere * 5;
    }
}
