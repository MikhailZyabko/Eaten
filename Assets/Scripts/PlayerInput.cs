using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private PlayerMover _mover;

    private void Start()
    {
        _mover = GetComponent<PlayerMover>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.TransformDirection(new Vector3(0, 0, vertical));
        Vector3 turnDirection = transform.TransformDirection(new Vector3(0, horizontal, 0));

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _mover.MoveForward(moveDirection);
            _mover.MakeTurn(Quaternion.Euler(turnDirection * _mover.TurnSpeed * Time.deltaTime));
            _animator.SetFloat("MoveSpeed", vertical);
        }
        else
        {
            _animator.SetFloat("MoveSpeed", 0);
        }
    }
}
