using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private MovementSurface _surface;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _turnSpeed = .5f;

    public void MoveForward(Vector3 direction)
    {
        Vector3 directionAlongSurface = _surface.MovementVector(direction.normalized);
        Vector3 offset = directionAlongSurface * (_moveSpeed * Time.deltaTime);

        _rigidBody.MovePosition(_rigidBody.position + offset);
    }

    public void MakeTurn(Vector3 turn)
    {
        _rigidBody.transform.Rotate(turn, _turnSpeed * Time.deltaTime);
    }
}
