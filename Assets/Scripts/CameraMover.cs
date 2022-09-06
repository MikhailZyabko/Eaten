using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private Vector3 _offset;

    [SerializeField] private Camera _camera;

    [SerializeField] private float _speedX = 360f;
    [SerializeField] private float _speedY = 240f;
    [SerializeField] private float limitY = 40f;
    [SerializeField] private float limitX = 40f;
    [SerializeField] private float _targetHideDistance = 2f;

    [SerializeField] private LayerMask _obstacles;
    [SerializeField] private LayerMask _noPlayer;

    private float _maxCameraDistance;
    private float _currentYRotation;

    private Vector3 _localPosition;

    private LayerMask _cameraOriginMask;

    private Vector3 _position 
    { 
        get { return transform.position; }
        set { transform.position = value;}
    }

    private void OnEnable()
    {
        transform.position = _target.position + _offset;
    }

    private void Start()
    {
        _localPosition = _target.InverseTransformPoint(_position);
        _maxCameraDistance = Vector3.Distance(_position, _target.position);
        _cameraOriginMask = _camera.cullingMask;
    }

    private void LateUpdate()
    {
        _position = _target.TransformPoint(_localPosition);

        CameraRotation();
        ObstaclesAvoid();
        PlayerAvoid();
        _localPosition = _target.InverseTransformPoint(_position);
    }

    private void CameraRotation()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        if (mouseY != 0)
        {
            var tmp = Mathf.Clamp(_currentYRotation - mouseY * _speedY * Time.deltaTime, -limitY, limitY);
            if (tmp != _currentYRotation)
            {
                var rotation = tmp - _currentYRotation;
                transform.RotateAround(_target.position, transform.right, rotation);
                _currentYRotation = tmp;
            }
        }

        if (mouseX != 0)
        {
            var tmp = Mathf.Clamp(mouseX * _speedX * Time.deltaTime, -limitX, limitX);
            transform.RotateAround(_target.position, Vector3.up, tmp);
        }

        transform.LookAt(_target);
    }

    private void ObstaclesAvoid()
    {
        var distance = Vector3.Distance(_position, _target.position);
        RaycastHit hit;
        if (Physics.Raycast(_target.position, _position - _target.position, out hit, _maxCameraDistance, _obstacles))
        {
            _position = hit.point;
        }
        else if ( distance < _maxCameraDistance && !Physics.Raycast(_position, -transform.forward, .1f, _obstacles))
        {
            _position -= transform.forward * .05f;
        }
    }

    private void PlayerAvoid()
    {
        var distance = Vector3.Distance(_position, _target.position);

        if ( distance < _targetHideDistance )
        {
            _camera.cullingMask = _noPlayer;
        }
        else
        {
            _camera.cullingMask = _cameraOriginMask;
        }
    }
}
