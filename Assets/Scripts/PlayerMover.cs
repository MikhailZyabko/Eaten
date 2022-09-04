using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private MovementSurface _surface;
    [SerializeField] private float _speed = 4f;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        Vector3 directionAlongSurface = _surface.MovementVector(direction.normalized);
        Vector3 offset = directionAlongSurface * (_speed * Time.deltaTime);

        _rigidBody.MovePosition(_rigidBody.position + offset);
    }
}
