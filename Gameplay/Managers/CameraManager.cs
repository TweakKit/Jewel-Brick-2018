using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField]
    private float _defaultOrthographicSize = 6.5f;
    [SerializeField]
    private float _desiredAspectRatio = 720.0f / 1280.0f;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = gameObject.GetComponent<Camera>();
    }

    private void Start()
    {
        if (_camera.aspect < _desiredAspectRatio)
            _camera.orthographicSize = _defaultOrthographicSize * _desiredAspectRatio / _camera.aspect;
        else
            _camera.orthographicSize = _defaultOrthographicSize;
    }
}