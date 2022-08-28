using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Transform _ratPosition;

    private void Start()
    {
        transform.position = _ratPosition.position + _offset;
    }
}
