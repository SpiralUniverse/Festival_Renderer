using OpenTK.Windowing.Common;

namespace Festival_Renderer.Source;

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
private readonly Camera _viewportCamera;
private readonly Texture _tex1;
private readonly Texture _tex2;

private Primitives.LightPlane _light;
private Primitives.Cube _cube;

public static Shader? Shader;
public static Shader? LightShader;

#endregion
    
    public Main() : base(new GameWindowSettings {RenderFrequency =  60, UpdateFrequency = 60}, new NativeWindowSettings {Size = new Vector2i(1600, 900), APIVersion = new Version(3, 3), WindowState = WindowState.Normal})
    {
        _viewportCamera = new Camera(new OpenTK.Mathematics.Vector3(5, 3, -3), ClientSize.X / (ClientSize.Y * 1.0f));
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

        _light = new Primitives.LightPlane("bricks.jpg", "container.jpg", 2, true);
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        if (_isPaused) return;
        base.OnResize(e);
        
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        _guiController?.WindowResized(ClientSize.X, ClientSize.Y);
        
    }
    
    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        _guiController?.PressChar((char)e.Unicode);
        
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _guiController?.Update(this, (float)args.Time);
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        
        UpdateMatrices();
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
    
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (KeyboardState.IsKeyPressed(Keys.Escape))
        {
            _isPaused = !_isPaused;
        }
        
        if(KeyboardState.IsKeyDown(Keys.F4)) Close();
        if(_isPaused) return;

        if (KeyboardState.IsKeyDown(Keys.W))
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
        }

        base.OnUpdateFrame(args);
    }
    
    private void UpdateMatrices()
    {
        var projection = _viewportCamera.GetProjectionMatrix();
        
        LightShader?.SetMatrix4("view", _viewportCamera.GetViewMatrix());
        LightShader?.SetMatrix4("proj", projection);
        
        Shader?.SetMatrix4("view", _viewportCamera.GetViewMatrix());
        Shader?.SetMatrix4("proj", projection);

    }
    
    private void Draw()
    {
        foreach (var o in SceneObject.Objects)
        {
            o.UpdateObjectData(_light.SceneObject, _viewportCamera.Position);
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
