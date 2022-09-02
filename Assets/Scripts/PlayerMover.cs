using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private MovementSurface _surface;
    [SerializeField] private float _speed = 4f;

    public void Move(Vector3 direction)
    {
        Vector3 directionAlongSurface = _surface.MovementVector(direction.normalized);
        Vector3 offset = directionAlongSurface * (_speed * Time.deltaTime);

        _rigidBody.MovePosition(_rigidBody.position + offset);
    }
}
