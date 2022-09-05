using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private MovementSurface _surface;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _turnSpeed = .5f;

<<<<<<< HEAD
    public void MoveForward(Vector3 direction)
=======
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
>>>>>>> remotes/origin/TechArt
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
