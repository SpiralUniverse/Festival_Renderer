﻿using OpenTK.Windowing.Common;

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
private bool _isDragging; //TODO: use this for orbiting.
public static readonly Vector3[] PointLightPositions =
{
    new Vector3(0.7f, 0.2f, 2.0f),
    new Vector3(2.3f, -3.3f, -4.0f),
    new Vector3(-4.0f, 2.0f, -12.0f),
    new Vector3(0.0f, 0.0f, -3.0f)
};

private System.Numerics.Vector3 _skyColor;


private readonly ImGuiController? _guiController;
private readonly Texture _tex1;
private readonly Texture _tex2;

private readonly CameraArcball _camera;

//private Primitives.LightPlane _light;
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
        _camera = new CameraArcball(new Vector3(5, 3, -3), Vector3.Zero, Vector3.UnitY, ClientSize.X / (ClientSize.Y * 1.0f));
        Shader = new Shader("Resources\\shader vert.glsl", "Resources\\shader frag.glsl");
        LightShader = new Shader("Resources\\shader lamp vert.glsl", "Resources\\shader lamp frag.glsl");
        _guiController = new ImGuiController(ClientSize.X, ClientSize.Y);
        _depthTest = true;
        _cullFace = true;
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
        //_cube = new Primitives.Cube("container2.png","container2_specular.png", 0);
        //_cube.Mesh.SetPosition(Vector3.One);

        //var cube2 = new Primitives.Cube("container2.png", "container2_specular.png", 1);
        //cube2.Mesh.SetPosition(-Vector3.One);
        uint lightId = 2;
        foreach (Vector3 position in PointLightPositions)
        {
            var light = new Primitives.LightPlane("bricks.jpg", "bricks.jpg", lightId++, true);
            light.Mesh.SetPosition(position);
        }
        //WindowState = WindowState.Maximized;
        //SelectSceneObject(cube2.Mesh);
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
        if (KeyboardState.IsKeyPressed(Keys.Escape)) Close();
        

        Vector4 position = new Vector4(_camera.Eye.X, _camera.Eye.Y, _camera.Eye.Z, 1);
        Vector4 pivot = new Vector4(_camera.LookAt.X, _camera.LookAt.Y, _camera.LookAt.Z, 1);

        float deltaAngleX = (float)(2 * Math.PI / ClientSize.X);
        float deltaAngleY = (float)(Math.PI / ClientSize.Y);
        float xAngle = (MouseState.X - LastMousePos.X) * deltaAngleX;
        float yAngle = (MouseState.Y - LastMousePos.Y) * deltaAngleY;

        Quaternion xRotation = Quaternion.FromAxisAngle(_camera.UpVector, xAngle);
        Quaternion yRotation = Quaternion.FromAxisAngle(_camera.RightVector, yAngle);
        
        Quaternion rotation = yRotation * xRotation;
        
        if (KeyboardState.IsKeyDown(Keys.LeftControl))
        {
            
            position = Vector4.Transform(position - pivot, rotation) + pivot;
            _camera.SetUpVector(Vector3.Transform(_camera.UpVector, rotation));
            
            _camera.SetCameraView(position.Xyz, _camera.LookAt, _camera.UpVector);
        }

        LastMousePos.X = MouseState.X;
        LastMousePos.Y = MouseState.Y;
        
        //TODO: added camera Panning and focus.

        base.OnUpdateFrame(args);
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
                case Mesh sceneObject:
                    Console.WriteLine($"Selected {sceneObject.Name} positioned at {sceneObject.Position}.");
                    SelectSceneObject(sceneObject);
                    break;
            }
        }
    }

    /// <summary>
    /// Select an Object.
    /// </summary>
    /// <param name="mesh"></param>
    private void SelectSceneObject(Mesh mesh)
    {
        _camera.SetCameraView(_camera.Eye, mesh.Position, _camera.UpVector);
        mesh._isSelected = true;
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
    /// TODO: Place all UI Updates in another method.
    /// </summary>
    private void Draw()
    {
        foreach (var o in Mesh.Meshes)
        {
            o.UpdateObjectData(/*_light.Object, */_camera.Eye);
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
        fbxPath = @"C:\Users\Khalid\Documents\Cube.fbx";
        ImGui.InputText("FBX Path", ref fbxPath, 256);
        if (ImGui.Button("Load FBX File"))
        {
            if(String.IsNullOrWhiteSpace(fbxPath) && !File.Exists(fbxPath))
                Console.WriteLine("path is empty");
            else
                Statics.LoadFBXFile(fbxPath);
        }
        ImGui.End();
    }
        string fbxPath = String.Empty;
}
