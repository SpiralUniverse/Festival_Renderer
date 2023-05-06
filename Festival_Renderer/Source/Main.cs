using OpenTK.Windowing.Common;

namespace Festival_Renderer.Source;


/// <summary>
/// The Main Window.
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
private bool _firstMove;
private bool _isPaused;

private Vector2 _lastMousePos;

private System.Numerics.Vector3 _skyColor;

#region --Vars mouse--

private Vector3 _prevMousePos = Vector3.Zero;
private bool _isDragging;
#endregion

private readonly ImGuiController? _guiController;
//private readonly Camera _viewportCamera;
private readonly Texture _tex1;
private readonly Texture _tex2;

private readonly CameraArcball _arcball;

private Primitives.LightPlane _light;
private Primitives.Cube _cube;

public static Shader? Shader;
public static Shader? LightShader;

#endregion
    
/// <summary>
/// Constructor To fill variables and set Window Settings.
/// Centers the window.
/// </summary>
    public Main() : base(new GameWindowSettings {RenderFrequency =  60, UpdateFrequency = 60}, new NativeWindowSettings {Size = new Vector2i(1600, 900), APIVersion = new Version(3, 3), WindowState = WindowState.Normal})
    {
        //_viewportCamera = new Camera(new OpenTK.Mathematics.Vector3(5, 3, -3), ClientSize.X / (ClientSize.Y * 1.0f));
        _arcball = new CameraArcball(new Vector3(5, 3, -3), Vector3.Zero, Vector3.UnitY, ClientSize.X / (ClientSize.Y * 1.0f));
        Shader = new Shader("Resources\\shader vert.glsl", "Resources\\shader frag.glsl");
        LightShader = new Shader("Resources\\shader lamp vert.glsl", "Resources\\shader lamp frag.glsl");
        _guiController = new ImGuiController(ClientSize.X, ClientSize.Y);
        _depthTest = true;
        _cullFace = true;
        _firstMove = true;
        _tex1 = Texture.LoadFromFile("Resources//bricks.jpg");
        _tex2 = Texture.LoadFromFile("Resources//container.jpg");
        CenterWindow();
    }

/// <summary>
/// OnLoad Event Called Directly when the app start.
/// Load, Enable, and Initialize important attributes and functionalities and variables.
/// </summary>
    protected override void OnLoad()
    {
        base.OnLoad();
        
        Title = "LUCSC Festival Simple Renderer " + GL.GetString(StringName.Version);
        _imGuiShaderProgramHandler = ImGuiController._shader;
        
        
        
        GL.Enable(EnableCap.Blend); 
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.ClearColor(new Color4(0, 32, 48, 255));
        _skyColor = new(0, 0.1254902f, 0.1882353f);
        _cube = new Primitives.Cube("container2.png","container2_specular.png", 0);
        _cube.SceneObject.SetPosition(Vector3.One);

        var cube2 = new Primitives.Cube("bricks.jpg", "container.jpg", 1);
        cube2.SceneObject.SetPosition(-Vector3.One);
        _light = new Primitives.LightPlane("bricks.jpg", "container.jpg", 2, true);
        WindowState = WindowState.Maximized;
        SelectSceneObject(cube2.SceneObject);
    }
    
/// <summary>
/// OnResize Event called when app window is resized.
/// Resize both viewport and UI interface to match new window Size.
/// </summary>
/// <param name="e">Resize event arguments.</param>
    protected override void OnResize(ResizeEventArgs e)
    {
        if (_isPaused) return;
        base.OnResize(e);
        
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        _guiController?.WindowResized(ClientSize.X, ClientSize.Y);
        
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

        #endregion
        
        SwapBuffers();
        
    }

    private Vector2 LastMousePos; //todo: move to correct position in code
    
    
    /// <summary>
    /// OnUpdateFrame Event Called on every LOGIC Frame.
    /// Used for app logic and input handling.
    /// Calculates camera orbit angles.
    /// </summary>
    /// <param name="args">Frame event arguments</param>
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (KeyboardState.IsKeyPressed(Keys.Escape))
        {
            _isPaused = !_isPaused;
        }
        
        if(KeyboardState.IsKeyDown(Keys.F4)) Close();
        if(_isPaused) return;

        /*if (KeyboardState.IsKeyDown(Keys.W))
        {
            _viewportCamera.Position += _viewportCamera.Front * Statics.CameraSpeed * (float)args.Time; // Forward
        }

        if (KeyboardState.IsKeyDown(Keys.S))
        {
            _viewportCamera.Position -= _viewportCamera.Front * Statics.CameraSpeed * (float)args.Time; // Backwards
        }
        if (KeyboardState.IsKeyDown(Keys.A))
        {
            _viewportCamera.Position -= _viewportCamera.Right * Statics.CameraSpeed * (float)args.Time; // Left
        }
        if (KeyboardState.IsKeyDown(Keys.D))
        {
            _viewportCamera.Position += _viewportCamera.Right * Statics.CameraSpeed * (float)args.Time; // Right
        }
        if (KeyboardState.IsKeyDown(Keys.Space))
        {
            _viewportCamera.Position += _viewportCamera.Up * Statics.CameraSpeed * (float)args.Time; // Up
        }
        if (KeyboardState.IsKeyDown(Keys.LeftShift))
        {
            _viewportCamera.Position -= _viewportCamera.Up * Statics.CameraSpeed * (float)args.Time; //Bottom
        }
        
        if (_firstMove) 
        {
            _lastMousePos = new Vector2(MouseState.X, MouseState.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = MouseState.X - _lastMousePos.X;
            var deltaY = MouseState.Y - _lastMousePos.Y;
            _lastMousePos = new Vector2(MouseState.X, MouseState.Y);

            
            _viewportCamera.Yaw += deltaX * Statics.Sensitivity;
            _viewportCamera.Pitch -= deltaY * Statics.Sensitivity;
        }*/
        
        
        // Get the homogeneous position of the camera and pivot point
        Vector4 position = new Vector4(_arcball.Eye.X, _arcball.Eye.Y, _arcball.Eye.Z, 1);
        Vector4 pivot = new Vector4(_arcball.LookAt.X, _arcball.LookAt.Y, _arcball.LookAt.Z, 1);

        // Step 1: Calculate the amount of rotation given the mouse movement
        float deltaAngleX = (float)(2 * Math.PI / ClientSize.X); // a movement from left to right = 2 * PI = 360 deg
        float deltaAngleY = (float)(Math.PI / ClientSize.Y); // a movement from top to bottom = PI = 180 deg
        float xAngle = (MouseState.X - LastMousePos.X) * deltaAngleX;
        float yAngle = (MouseState.Y - LastMousePos.Y) * deltaAngleY;

        
        // Extra step to handle the problem when the camera direction is the same as the up vector
        float cosAngle = Vector3.Dot(_arcball.ViewDirection, _arcball.UpVector);
        if (cosAngle * Math.Sign(yAngle) > 0.99f)
            yAngle = 0; //todo: fix rotatio top view
        
        // Step 2: Rotate the camera around the pivot point on the first axis
        Matrix4 rotationMatrixX = Matrix4.CreateFromAxisAngle(_arcball.UpVector, xAngle);
        position = (rotationMatrixX * (position - pivot)) + pivot;

        // Step 3: Rotate the camera around the pivot point on the second axis
        Matrix4 rotationMatrixY = Matrix4.CreateFromAxisAngle(_arcball.RightVector, yAngle);
        Vector4 finalPosition = (rotationMatrixY * (position - pivot)) + pivot;

        // Update the camera view (we keep the same lookat and the same up vector)
        _arcball.SetCameraView(finalPosition.Xyz, _arcball.LookAt, _arcball.UpVector);

        // Update the mouse position for the next rotation
        LastMousePos.X = MouseState.X;
        LastMousePos.Y = MouseState.Y;

        base.OnUpdateFrame(args);
    }

    protected override void OnFocusedChanged(FocusedChangedEventArgs e)
    {
        base.OnFocusedChanged(e);
    }

    /// <summary>
    /// OnMouseUp event called when mouse button is released.
    /// Used to help in Object Selection.
    /// </summary>
    /// <param name="e">Mouse button event arguments</param>
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if(e.Button != MouseButton.Left) return;
        PickObjectOnScreen((int)MousePosition.Y, (int)MousePosition.Y);
        //TODO: unify the usage of accessing mouse position!
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
        var rayEye = _arcball.ProjectionMatrix.Inverted() * rayClip;
        rayEye = new Vector4(rayEye.X, rayEye.Y, -1f, 0f);

        // 4D world coordinates
        var rayWorldCoordinates = (_arcball.ViewMatrix.Inverted() * rayEye).Xyz;
        rayWorldCoordinates.Normalize();
        FindClosestSceneObjectHitByRay(rayWorldCoordinates);
    }

    /// <summary>
    /// Helps in Object Selection.
    /// Finds The Best Candidate.
    /// </summary>
    /// <param name="rayWorldCoordinates"></param>
    private void FindClosestSceneObjectHitByRay(Vector3 rayWorldCoordinates)
    {
        SceneObject bestCandidate = null;
        double? bestDistance = null;
        foreach (var gameObject in SceneObject.Objects)
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
                case SceneObject sceneObject:
                    Console.WriteLine($"Selected {sceneObject.name} positioned at {sceneObject.Position}.");
                    SelectSceneObject(sceneObject);
                    break;
            }
        }
    }

    /// <summary>
    /// Select an Object.
    /// </summary>
    /// <param name="sceneObject"></param>
    private void SelectSceneObject(SceneObject sceneObject)
    {
        _arcball.SetCameraView(_arcball.Eye, sceneObject.Position, _arcball.UpVector);
        sceneObject._isSelected = true;
        //TODO: finish implementation.
    }

    /// <summary>
    /// Sends matrices data to the GPU shader program based on their uniform names. 
    /// </summary>
    private void SendMatricesData()
    {
        
        LightShader?.SetMatrix4("view", _arcball.ViewMatrix);
        LightShader?.SetMatrix4("proj", _arcball.ProjectionMatrix);
        
        Shader?.SetMatrix4("view", _arcball.ViewMatrix);
        Shader?.SetMatrix4("proj", _arcball.ProjectionMatrix);

    }
    
    /// <summary>
    /// Draws all Objects and Update UI.
    /// TODO: Place all UI Updates in another method.
    /// </summary>
    private void Draw()
    {
        foreach (var o in SceneObject.Objects)
        {
            o.UpdateObjectData(_light.SceneObject, _arcball.Eye);
            o.Draw();
        }

        ImGui.Begin("World Settings");
        ImGui.Text("This is Materialized Alpha Version Of The Festival Renderer App");
        ImGui.Text("Keys: Escape ==> pause Screen Orientation, F4 ==> Quit");
        ImGui.Text("mid mouse and drag to orbit");
        ImGui.Text("For Transparency effect pls edit the 4th Color Value");
        ImGui.Text("For Culling move into the box more culling and transparency options to be added");
        ImGui.Text("Play around");
        
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
        ImGui.End();
    }
}
