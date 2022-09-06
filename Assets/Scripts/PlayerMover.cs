using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementSurface))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _turnSpeed = .5f;
    [SerializeField] private float _jumpForce = 1f;

    private MovementSurface _surface;
    private Rigidbody _rigidBody;

    public float TurnSpeed { get { return _turnSpeed; } }
    public float MoveSpeed { get { return _moveSpeed; } }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _surface = GetComponent<MovementSurface>();
    }

    public void MoveForward(Vector3 direction)
    {
        Vector3 directionAlongSurface = _surface.GetMovementVector(direction.normalized);
        Vector3 offset = directionAlongSurface * (_moveSpeed * Time.deltaTime);

        _rigidBody.MovePosition(_rigidBody.position + offset);
    }

    public void MakeTurn(Quaternion turn)
    {
        _rigidBody.MoveRotation(_rigidBody.rotation * turn);       
    }

    public void MakeJump(Vector3 jump)
    {
        _rigidBody.position += jump * _jumpForce;
    }
}
