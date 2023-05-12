namespace Festival_Renderer.Source;

/// <summary>
/// Camera system that simulates Arcball orbit system.
/// </summary>
public class CameraArcball
{
    #region -- Fields and Variables --

    private Matrix4 _viewMatrix;
    private Matrix4 _projectionMatrix;
    private Vector3 _eye;
    private Vector3 _lookAt;
    private Vector3 _upVector;
    private readonly float _aspectRatio;
    private float _screenWidth, _screenHeight;
    private float _depthNear = 0.1f;
    private float _depthFar = 100f;
    private float _fovy = MathHelper.PiOver2;
    private bool _isPerspectiveCamera = true;

    public Matrix4 ViewMatrix => _viewMatrix;
    public Matrix4 ProjectionMatrix => _projectionMatrix;
    public Vector3 Eye => _eye;
    public Vector3 UpVector => _upVector;
    public Vector3 LookAt => _lookAt;

    public float DepthNear => _depthNear;
    public float DepthFar => _depthFar;
    public float Fovy => _fovy;

    #endregion
    
    
    /// <summary>
    /// Default Constructor for default variables (might get null somewhere)
    /// </summary>
    public CameraArcball()
    {
        // Default constructor
    }

    /// <summary>
    /// Constructor to fill up necessary values
    /// </summary>
    /// <param name="eye">3D Vector represents the position of POV aka (Eye, Camera, View).</param>
    /// <param name="lookAt">Point in 3D space at which the eye should be looking at.</param>
    /// <param name="upVector">3D Vector that represent the up direction used in the program.</param>
    /// <param name="aspectRatio">The aspect ratio that your program is using.</param>
    public CameraArcball(Vector3 eye, Vector3 lookAt, Vector3 upVector, float aspectRatio)
    {
        _eye = eye;
        _lookAt = lookAt;
        _upVector = upVector;
        _aspectRatio = aspectRatio;
        UpdateViewMatrix();
    }


    // Camera forward is -z
    /// <summary>
    /// 3D vector that represents the view Direction. 
    /// </summary>
    public Vector3 ViewDirection => -Matrix4.Transpose(_viewMatrix).Row2.Xyz;
    
    /// <summary>
    /// 3D Vector that represents the right Direction.
    /// </summary>
    public Vector3 RightVector => Matrix4.Transpose(_viewMatrix).Row0.Xyz;

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eye">3D Vector represents the position of POV aka (Eye, Camera, View).</param>
    /// <param name="lookAt">Point in 3D space at which the eye should be looking at.</param>
    /// <param name="up">3D Vector that represent the up direction used in the program.</param>
    public void SetCameraView(Vector3 eye, Vector3 lookAt, Vector3 up)
    {
        _eye = eye;
        _lookAt = lookAt;
        _upVector = up;
        UpdateViewMatrix();
    }

    
    /// <summary>
    /// Updates view and projection Matrices.
    /// choose weather perspective or orthographic view
    /// </summary>
    public void UpdateViewMatrix()
    {
        // Generate view matrix using the eye, lookAt, and up vector
        _viewMatrix = Matrix4.LookAt(_eye, _lookAt, _upVector);
        if (_isPerspectiveCamera)
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fovy, _aspectRatio, _depthNear, _depthFar);
        else
            _projectionMatrix = Matrix4.CreateOrthographic(_screenWidth, _screenHeight, _depthNear, _depthFar);
    }

    /// <summary>
    /// updates the UpDirection
    /// </summary>
    /// <param name="newVector"> new value of Up Direction</param>
    public void SetUpVector(Vector3 newVector)
    {
        _upVector = newVector;
    }

    /// <summary>
    /// set new target for camera to look at
    /// </summary>
    /// <param name="cameraLookAt"> target's position</param>
    public void SetLookAt(Vector3 cameraLookAt)
    {
        _lookAt = cameraLookAt;
    }

    /// <summary>
    /// updates Camera configurations
    /// </summary>
    /// <param name="depthNear"></param>
    /// <param name="depthFar"></param>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    /// <param name="isPerspective"></param>
    public void SetCameraConfig(float depthNear, float depthFar, float screenWidth, float screenHeight, bool isPerspective, float fovy)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        _depthFar = depthFar;
        _depthNear = depthNear;
        _isPerspectiveCamera = isPerspective;
        _fovy = fovy;
    }

    /// <summary>
    /// Updates camera Eye.
    /// </summary>
    /// <param name="newCameraPosition">new Eye</param>
    public void setEye(Vector3 newCameraPosition)
    {
        _eye = newCameraPosition;
    }
}
