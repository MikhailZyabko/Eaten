using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class PlayerInput : MonoBehaviour
{
    private PlayerMover _mover;

    private void Start()
    {
        _mover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        /*if (Input.GetKeyDown(KeyCode.W))
        {
            _mover.Move(Vector3.forward);
        }*/

        _mover.Move(new Vector3(-vertical, 0, horizontal));
    }
}
