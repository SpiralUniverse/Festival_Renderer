using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;

namespace Festival_Renderer.Source;

/// <summary>
/// The Main Render Window.
/// </summary>
internal class Main : GameWindow
{
    
#region -- Fields and Vars --

private int _imGuiShaderProgramHandler;

#region -- Fps Variables --

private int _fpsCounter;
private double _fps;
private double _elapsedTime;

#endregion

private bool _depthTest;
private bool _alphaBlend;
private bool _cullFace;
private float _radius;
private Mesh _selectedMesh = null;
private readonly Grid _grid;

private struct CameraConfig
{
    public bool IsPerspectiveCamera;
    public float DepthNear;
    public float DepthFar;
    public float Fovy;
    public float ScreenWidth;
    public float ScreenHeight;

    public CameraConfig(float depthNear, float depthFar, float screenWidth, float screenHeight,
        bool isPerspectiveCamera, float fovy)
    {
        DepthNear = depthNear;
        DepthFar = depthFar;
        ScreenHeight = screenHeight;
        ScreenWidth = screenWidth;
        IsPerspectiveCamera = isPerspectiveCamera;
        Fovy = fovy;
    }
}

private CameraConfig _cameraConfig;
private int _cameraType = 0;

public static readonly Vector3[] PointLightPositions =
{
    new Vector3(0.7f, 0.2f, 2.0f),
    new Vector3(2.3f, -3.3f, -4.0f),
    new Vector3(-4.0f, 2.0f, -12.0f),
    new Vector3(0.0f, 0.0f, -3.0f)
};

private Vector2 _lastMousePos;
private System.Numerics.Vector3 _skyColor;


private readonly ImGuiController? _guiController;

private string _modelPath = string.Empty;

private readonly CameraArcball _camera;

public static Shader? Shader;
public static Shader? LightShader;

#endregion
    
/// <summary>
/// Constructor To fill variables and set Window Settings.
/// Centers the window.
/// </summary>
    public Main() : base(new GameWindowSettings {RenderFrequency =  60, UpdateFrequency = 60}, new NativeWindowSettings {Size = new Vector2i(1600, 900), APIVersion = new Version(3, 3), WindowState = WindowState.Normal})
    {
        _camera = new CameraArcball(new Vector3(5, 3, -3), Vector3.Zero, Vector3.UnitY, ClientSize.X / (ClientSize.Y * 1.0f));
        Shader = new Shader("Resources\\shader vert.glsl", "Resources\\shader frag.glsl");
        LightShader = new Shader("Resources\\shader lamp vert.glsl", "Resources\\shader lamp frag.glsl");
        _guiController = new ImGuiController(ClientSize.X, ClientSize.Y);
        _depthTest = true;
        _cullFace = true;
        _cameraConfig = new CameraConfig(0.1f, 1000f, ClientSize.X, ClientSize.Y, true, MathHelper.PiOver2);
        CenterWindow();
        _radius = 5;
        Shader gridShader = new Shader("Resources\\shader grid vert.glsl", "Resources\\shader grid frag.glsl");
        _grid = new Grid(gridShader, 40, 1, Color4.Gray);
    }

/// <summary>
/// OnLoad Event Called Directly when the app start.
/// Load, Enable, and Initialize important attributes and functionalities and variables.
/// </summary>
    protected override void OnLoad()
    {
        base.OnLoad();

        using (Stream stream = File.OpenRead("iconTest.png"))
        {
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            Icon = new WindowIcon(new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, image.Data));
        }
        

        Title = "LUCSC Festival Simple Renderer " + GL.GetString(StringName.Version);
        _imGuiShaderProgramHandler = ImGuiController._shader;

        GL.Enable(EnableCap.Blend); 
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.ClearColor(new Color4(0, 32, 48, 255));
        
        _skyColor = new(0, 0.1254902f, 0.1882353f);
        
        foreach (Vector3 position in PointLightPositions)
        {
            var light = new Primitives.LightSphere();
            Mesh.PointLights[^1].SetPosition(position);
        }

        var c = new Primitives.Cube("container2.png", "container2_specular.png", 1, false);
        var p = new Primitives.Cube("container2.png", "container2_specular.png", 2);
    }
    
/// <summary>
/// OnResize Event called when app window is resized.
/// Resize both viewport and UI interface to match new window Size.
/// </summary>
/// <param name="e">Resize event arguments.</param>
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        _guiController?.WindowResized(ClientSize.X, ClientSize.Y);
        _cameraConfig.ScreenWidth = ClientSize.X;
        _cameraConfig.ScreenHeight = ClientSize.Y;

    }
    
/// <summary>
/// OnTextInput event called when use try to input text.
/// Used for UI.
/// </summary>
/// <param name="e">Text input event arguments.</param>
    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        _guiController?.PressChar((char)e.Unicode);
        
    }

protected override void OnMouseWheel(MouseWheelEventArgs e)
{
    base.OnMouseWheel(e);
    _radius -= e.OffsetY * 0.1f;
    _radius = Math.Max(_radius, 0.1f);
}

/// <summary>
/// OnRenderFrame Event Called every Rendering Frame.
/// Updates screen colors data and call all methods related to drawing on screen.
/// Calculates Rendering frames/seconds.
/// </summary>
/// <param name="args">Frame event arguments.</param>
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _guiController?.Update(this, (float)args.Time);
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        
        SendMatricesData();
        Draw();
        
        GL.UseProgram(_imGuiShaderProgramHandler);
        _guiController?.Render();
        ImGuiController.CheckGLError("End of frame");

        #region -- fps Counting --

        _fpsCounter++;
        _elapsedTime += args.Time;
        if (_elapsedTime > 1)
        {
            _fps = _fpsCounter / _elapsedTime;
            _fpsCounter = 0;
            _elapsedTime = 0;
        }
        
        _grid.Draw(_camera.ViewMatrix, _camera.ProjectionMatrix);

        #endregion
        
        SwapBuffers();
        
    }

    
    
    /// <summary>
    /// OnUpdateFrame Event Called on every LOGIC Frame.
    /// Used for app logic and input handling.
    /// Calculates camera orbit angles.
    /// </summary>
    /// <param name="args">Frame event arguments</param>
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (KeyboardState.IsKeyPressed(Keys.Escape)) Close();
        

        _camera.SetCameraConfig(_cameraConfig.DepthNear, _cameraConfig.DepthFar, _cameraConfig.ScreenWidth, _cameraConfig.ScreenHeight, _cameraConfig.IsPerspectiveCamera, _cameraConfig.Fovy);
        Vector4 position = new Vector4(_camera.Eye.X, _camera.Eye.Y, _camera.Eye.Z, 1);
        Vector4 pivot = new Vector4(_camera.LookAt.X, _camera.LookAt.Y, _camera.LookAt.Z, 1);

        float deltaAngleX = (float)(2 * Math.PI / ClientSize.X);
        float deltaAngleY = (float)(Math.PI / ClientSize.Y);
        float xAngle = (MouseState.X - _lastMousePos.X) * deltaAngleX;
        float yAngle = (MouseState.Y - _lastMousePos.Y) * deltaAngleY;

        Quaternion xRotation = Quaternion.FromAxisAngle(_camera.UpVector, xAngle);
        Quaternion yRotation = Quaternion.FromAxisAngle(_camera.RightVector, yAngle);
        
        Quaternion rotation = yRotation * xRotation;
        
        if (KeyboardState.IsKeyDown(Keys.LeftControl) || MouseState.IsButtonDown(MouseButton.Middle))
        {
            
            position = Vector4.Transform(position - pivot, rotation) + pivot;
            _camera.SetUpVector(Vector3.Transform(_camera.UpVector, rotation));
            
            _camera.SetCameraView(position.Xyz, _camera.LookAt, _camera.UpVector);
        }

        if (KeyboardState.IsKeyDown(Keys.LeftShift))
        {
            _cameraOffset = _camera.RightVector * (MouseState.X - _lastMousePos.X) + _camera.UpVector * (_lastMousePos.Y - MouseState.Y);
            _cameraOffset *= 0.1f;
            _camera.SetLookAt(_camera.LookAt + _cameraOffset);
            _camera.SetCameraView(position.Xyz + _cameraOffset, _camera.LookAt, _camera.UpVector); 
        }

        if (_selectedMesh != null)
        {
            var direction = _selectedMesh.Position - _camera.Eye;
            var unitDirection = Vector3.Normalize(direction);
            Vector3 newCameraPosition = _selectedMesh.Position - unitDirection * _radius;
            _camera.setEye(newCameraPosition);
            _camera.UpdateViewMatrix();
        }
        else
        {
            var direction = Vector3.Zero - _camera.Eye;
            var unitDirection = Vector3.Normalize(direction);
            Vector3 newCameraPosition = Vector3.Zero - unitDirection * _radius;
            _camera.setEye(newCameraPosition);
            _camera.UpdateViewMatrix();
        }
        
        
        
        _lastMousePos.X = MouseState.X;
        _lastMousePos.Y = MouseState.Y;


        base.OnUpdateFrame(args);
    }

    private Vector3 _cameraOffset;
    /// <summary>
    /// OnMouseUp event called when mouse button is released.
    /// Used to help in Object Selection.
    /// </summary>
    /// <param name="e">Mouse button event arguments</param>
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if(e.Button != MouseButton.Left) return;
        PickObjectOnScreen((int)MousePosition.Y, (int)MousePosition.Y);
    }

    /// <summary>
    /// Picks an Object based on mouse position. 
    /// </summary>
    /// <param name="mouseX">mouse position on x-axis (Screen Width).</param>
    /// <param name="mouseY">mouse position on y-axis (Screen Height).</param>
    private void PickObjectOnScreen(int mouseX, int mouseY)
    {
        // heavily influenced by: http://antongerdelan.net/opengl/raycasting.html
        // viewport coordinate system
        // normalized device coordinates
        var x = (2f * mouseX) / ClientSize.X - 1f;
        var y = 1f - (2f * mouseY) / ClientSize.Y;
        var z = 1f;
        var rayNormalizedDeviceCoordinates = new Vector3(x, y, z);

        // 4D homogeneous clip coordinates
        var rayClip = new Vector4(rayNormalizedDeviceCoordinates.X, rayNormalizedDeviceCoordinates.Y, -1f, 1f);

        // 4D eye (camera) coordinates
        var rayEye = _camera.ProjectionMatrix.Inverted() * rayClip;
        rayEye = new Vector4(rayEye.X, rayEye.Y, -1f, 0f);

        // 4D world coordinates
        var rayWorldCoordinates = (_camera.ViewMatrix.Inverted() * rayEye).Xyz;
        rayWorldCoordinates.Normalize();
        FindClosestSceneObjectHitByRay(rayWorldCoordinates);
    }

    /// <summary>
    /// Helps in Object Selection.
    /// Finds The Best Candidate.
    /// Based on this blog post https://dreamstatecoding.blogspot.com/2018/03/opengl-4-with-opentk-in-c-part-15.html.
    /// </summary>
    /// <param name="rayWorldCoordinates"></param>
    private void FindClosestSceneObjectHitByRay(Vector3 rayWorldCoordinates)
    {
        Mesh bestCandidate = null;
        double? bestDistance = null;
        foreach (var gameObject in Mesh.Meshes)
        {
            var candidateDistance = gameObject.IntersectsRay(rayWorldCoordinates);
            if (!candidateDistance.HasValue)
                continue;
            if (!bestDistance.HasValue)
            {
                bestDistance = candidateDistance;
                bestCandidate = gameObject;
                continue;
            }
            if (candidateDistance < bestDistance)
            {
                bestDistance = candidateDistance;
                bestCandidate = gameObject;
            }
        }
        if (bestCandidate != null)
        {
            switch (bestCandidate)
            {
                case Mesh mesh:
                    Console.WriteLine($"Selected {mesh.Name} positioned at {mesh.Position}.");
                    SelectMesh(mesh);
                    break;
            }
        }
    }

    /// <summary>
    /// Select a mesh.
    /// </summary>
    /// <param name="mesh">mesh to be selected</param>
    private void SelectMesh(Mesh mesh)
    {
        _camera.SetCameraView(_camera.Eye, mesh.Position, _camera.UpVector);
        _selectedMesh = mesh;
        //TODO: finish implementation.
    }

    /// <summary>
    /// Sends matrices data to the GPU shader program based on their uniform names. 
    /// </summary>
    private void SendMatricesData()
    {
        
        LightShader?.SetMatrix4("view", _camera.ViewMatrix);
        LightShader?.SetMatrix4("proj", _camera.ProjectionMatrix);
        
        Shader?.SetMatrix4("view", _camera.ViewMatrix);
        Shader?.SetMatrix4("proj", _camera.ProjectionMatrix);

    }
    
    /// <summary>
    /// Draws all Objects and Update UI.
    /// </summary>
    private void Draw()
    {
        foreach (var o in Mesh.Meshes)
        {
            o.UpdateObjectData(_camera.Eye);
            o.Draw();
        }

        /*if (ImGui.ImageButton("Texture 1", _tex1.Handel, new(150, 150)))
        {
            Console.WriteLine("Texture 1 bricks To be implemented!");
        }
        else if(ImGui.ImageButton("Texture 2", _tex2.Handel, new(150, 150)))
        {
            Console.WriteLine("Texture 2 container To be implemented!");
        }*/

        //ImGui.DragFloat("camera sensitivity", ref Statics._sensitivity);

        if(_fps <= 20.0f)
            ImGui.TextColored(new(255, 0,0, 255), "Frames / seconds (Update)" + _fps.ToString("F2"));
        else
            ImGui.TextColored(new(0, 255,0, 255), "Frames / seconds (Update)" + _fps.ToString("F2"));
        
        ImGui.Checkbox("DepthTest", ref _depthTest);
        ImGui.Checkbox("Transparency", ref _alphaBlend);
        ImGui.Checkbox("Culling", ref _cullFace);
        
        if (ImGui.ColorEdit3("Sky Color", ref _skyColor))
            GL.ClearColor(Statics.Numerics3ToOpentk3(_skyColor).X, Statics.Numerics3ToOpentk3(_skyColor).Y,
                Statics.Numerics3ToOpentk3(_skyColor).Z, 1);
        
        if(_depthTest)
            GL.Enable(EnableCap.DepthTest);
        else
            GL.Disable(EnableCap.DepthTest);
        
        if(_alphaBlend)
            GL.Enable(EnableCap.Blend);
        else
            GL.Disable(EnableCap.Blend);
        
        if(_cullFace)
            GL.Enable(EnableCap.CullFace);
        else
            GL.Disable(EnableCap.CullFace);
        _modelPath = @"C:\Users\Khalid\Documents\mon.fbx";
        ImGui.InputText("Model Path", ref _modelPath, 256);
        if (ImGui.Button("Load FBX File") || KeyboardState.IsKeyPressed(Keys.Enter))
        {
            if(string.IsNullOrWhiteSpace(_modelPath) && !File.Exists(_modelPath))
                Console.WriteLine("path is empty");
            else
                Statics.LoadModelFile(_modelPath);
        }
        ImGui.End();
        ImGui.Begin("world settings");
        ImGui.DragFloat3("sun", ref Statics.SunColor, Statics.DragSensitivity, 0, 255);
        ImGui.DragFloat("radius", ref _radius, Statics.DragSensitivity, 1, 15);
        ImGui.DragFloat("clip near", ref _cameraConfig.DepthNear, Statics.DragSensitivity);
        ImGui.DragFloat("clip far", ref _cameraConfig.DepthFar, Statics.DragSensitivity);
        ImGui.DragFloat("FOV", ref _cameraConfig.Fovy, Statics.DragSensitivity);
        if (ImGui.BeginCombo("camera type", _cameraType == 0 ? "Perspective" : "Orthographic"))
        {
            if (ImGui.Selectable("perspective", _cameraType == 0))
            {
                _cameraType = 0;
                _cameraConfig.IsPerspectiveCamera = true;
                Console.WriteLine("test1");
            }
            else if (ImGui.Selectable("orthographic", _cameraType == 1))
            {
                _cameraType = 1;
                _cameraConfig.IsPerspectiveCamera = false;
                Console.WriteLine("test");
            }
        }
        ImGui.EndCombo();
        ImGui.End();
    }

}
