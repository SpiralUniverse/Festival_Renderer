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

    public Matrix4 ViewMatrix => _viewMatrix;
    public Matrix4 ProjectionMatrix => _projectionMatrix;
    public Vector3 Eye => _eye;
    public Vector3 UpVector => _upVector;
    public Vector3 LookAt => _lookAt;

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
    /// </summary>
    private void UpdateViewMatrix()
    {
        // Generate view matrix using the eye, lookAt, and up vector
        _viewMatrix = Matrix4.LookAt(_eye, _lookAt, _upVector);
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, _aspectRatio, 0.01f, 100f);
        //TODO: change clip space during runtime
    }

    public void SetUpVector(Vector3 newVector)
    {
        _upVector = newVector;
    }
}
