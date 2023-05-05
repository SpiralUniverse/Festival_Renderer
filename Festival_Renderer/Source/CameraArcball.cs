namespace Festival_Renderer.Source;

using OpenTK;
using OpenTK.Graphics;

public class CameraArcball
{
    #region -- Fields and Variables --

    private Matrix4 viewMatrix;
    private Matrix4 projectionMatrix;
    private Vector3 eye; // Camera position in 3D
    private Vector3 lookAt; // Point that the camera is looking at
    private Vector3 upVector; // Orientation of the camera
    private float aspectRatio;

    public Matrix4 ViewMatrix => viewMatrix;
    public Matrix4 ProjectionMatrix => projectionMatrix;
    public Vector3 Eye => eye;
    public Vector3 UpVector => upVector;
    public Vector3 LookAt => lookAt;

    #endregion
    
    
    
    public CameraArcball()
    {
        // Default constructor
    }

    public CameraArcball(Vector3 eye, Vector3 lookAt, Vector3 upVector, float aspectRatio)
    {
        this.eye = eye;
        this.lookAt = lookAt;
        this.upVector = upVector;
        this.aspectRatio = aspectRatio;
        UpdateViewMatrix();
    }


    // Camera forward is -z
    public Vector3 ViewDirection => -Matrix4.Transpose(viewMatrix).Row2.Xyz;
    public Vector3 RightVector => Matrix4.Transpose(viewMatrix).Row0.Xyz;

    public void SetCameraView(Vector3 eye, Vector3 lookAt, Vector3 up)
    {
        this.eye = eye;
        this.lookAt = lookAt;
        this.upVector = up;
        UpdateViewMatrix();
    }

    public void UpdateViewMatrix()
    {
        // Generate view matrix using the eye, lookAt, and up vector
        viewMatrix = Matrix4.LookAt(eye, lookAt, upVector);
        projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, aspectRatio, 0.01f, 100f);
        //TODO: change clip space during runtime
    }
}
