namespace Festival_Renderer.Source;

public struct Primitives
{
    public struct Cube
    {
        private readonly uint _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public SceneObject SceneObject;

        public Cube(string textureName, uint id)
        {
            _id = id;
            _title = "Cube " + _id;
            _vertices = new[]
            {
                //Position3         TexCoord2       Normal3
                // Front face
                -1.0f, -1.0f,  1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f,// 0
                1.0f, -1.0f,  1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,// 1
                1.0f,  1.0f,  1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f,// 2
                -1.0f,  1.0f,  1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,// 3

                // Back face
                -1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, -1.0f,// 4
                1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 0.0f, -1.0f,// 5
                1.0f,  1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f,// 6
                -1.0f,  1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 0.0f, -1.0f,// 7

                // Left face
                -1.0f, -1.0f, -1.0f, 1.0f, 1.0f, -1.0f, 0.0f, 0.0f,// 8
                -1.0f, -1.0f,  1.0f, 1.0f, 0.0f, -1.0f, 0.0f, 0.0f,// 9
                -1.0f,  1.0f,  1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,// 10
                -1.0f,  1.0f, -1.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f,// 11

                // Right face
                1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f,// 12
                1.0f, -1.0f,  1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f,// 13
                1.0f,  1.0f,  1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,// 14
                1.0f,  1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,// 15

                // Top face
                -1.0f,  1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,// 16
                1.0f,  1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,// 17
                1.0f,  1.0f,  1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f,// 18
                -1.0f,  1.0f,  1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,// 19

                // Bottom face
                -1.0f, -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, -1.0f, 0.0f,// 20
                1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, -1.0f, 0.0f,// 21
                1.0f, -1.0f,  1.0f, 0.0f, 0.0f, 0.0f, -1.0f, 0.0f,// 22
                -1.0f, -1.0f,  1.0f, 0.0f, 1.0f, 0.0f, -1.0f, 0.0f,// 23
            };

            _indices = new uint[]
            {
                0, 1, 2, 0, 2, 3, // Front face
                4, 6, 5, 4, 7, 6, // Back face
                8, 9, 10, 8, 10, 11, // Left face
                12, 14, 13, 12, 15, 14, // Right face
                16, 18, 17, 16, 19, 18, // Top face
                20, 21, 22, 20, 22, 23 // Bottom face
            };
            
            SceneObject = new SceneObject(_title, Vector3.Zero, Vector3.One, Vector3.Zero, new Color4(0.5f, 0.5f, 0.5f, 1.0f), _vertices,
                _indices, "Resources//" + textureName);
        }
    }
    
    public struct Triangle
    {
        private readonly uint _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public SceneObject SceneObject;

        public Triangle(string textureName, uint id)
        {
            _id = id;
            _title = "Triangle " + _id;
            _vertices = new[]
            {
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f,
                0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, -1.0f,
                0.0f, 0.5f, 0.0f, 0.5f, 1.0f, 0.0f, 0.0f, -1.0f,
            };

            _indices = new uint[]
            {
                0, 1, 2
            };
            
            SceneObject = new SceneObject(_title, Vector3.Zero, Vector3.One, Vector3.Zero, new Color4(0.5f, 0.5f, 0.5f, 1.0f), _vertices,
                _indices, "Resources//" + textureName);
        }
    }

    public struct Plane
    {
        private readonly uint _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public SceneObject SceneObject;

        public Plane(string texturePath, uint id)
        {
            _id = id;
            _title = "Plane " + _id;
            
            _vertices = new[]
            {
                -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f,
                1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, -1.0f,
                1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, -1.0f,
                -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, -1.0f
            };
            
            _indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };

            SceneObject = new SceneObject(_title, Vector3.Zero, Vector3.One, new Vector3(90, 0, 0),
                new Color4(0.5f, 0.5f, 0.5f, 1.0f), _vertices, _indices, "Resources//" + texturePath);
        }
    }
    
    public struct LightPlane
    {
        private readonly uint _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public readonly SceneObject SceneObject;

        public LightPlane(string texturePath, uint id, bool isLightSource)
        {
            _id = id;
            _title = "Light Plane " + _id;
            
            _vertices = new[]
            {
                -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f,
                1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, -1.0f,
                1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, -1.0f,
                -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, -1.0f
            };
            
            _indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };

            SceneObject = new SceneObject(_title, new Vector3(0, 1, -2.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0),
                Color4.White, _vertices, _indices, "Resources//" + texturePath, isLightSource);
        }
    }
}

public class SceneObject
{
    public static readonly List<SceneObject> Objects = new List<SceneObject>();
    public Vector3 Position => Statics.Numerics3ToOpentk3(_position);
    
    private Matrix4 _modelMatrix;
    private  Material _objectMaterial;
    
    private readonly int _vao;
    private readonly int _vbo;
    private readonly int _ebo;
    private readonly int _indexCount;
    private readonly string _title;

    private readonly bool _isLightSource;

    private System.Numerics.Vector3 _position;
    private System.Numerics.Vector3 _scale;
    private System.Numerics.Vector3 _rotation;
    private System.Numerics.Vector4 _color;

    private readonly float[] _vertices;
    private readonly uint[] _indices;


    private struct Material
    {
        public float Metallic;
        public float Smoothness;
        public float EmissionStrength;
        public Color4 Color4;
        public Color4 EmissionColor4;
        public readonly Texture Texture;

        public Material(string texturePath, float metallic = 0.5f, float smoothness = 0.5f, float emissionStrength = 0)
        {
            Metallic = metallic;
            Smoothness = smoothness;
            EmissionStrength = emissionStrength;
            Color4 = Color4.LightGray;
            EmissionColor4 = Color4.Black;
            Texture = Texture.LoadFromFile(texturePath);

        }
        //TODO: 3 textures to be added diffuse, normal, specular
        //TODO: implement the metallic and smoothness values in shader
        
    }

    public SceneObject(string title, Vector3 position, Vector3 scale, Vector3 rotation, Color4 color, float[] vertices, uint[] indices, string texturePath, bool isLightSource = false)
    {
        _objectMaterial = new Material(texturePath);
        _position = Statics.Opentk3ToNumerics3(position);
        _scale = Statics.Opentk3ToNumerics3(scale);
        _rotation = Statics.Opentk3ToNumerics3(rotation);
        _color = Statics.Color4ToNumerics4(color);
        _vertices = vertices;
        _indices = indices;
        _title = title;
        _isLightSource = isLightSource;

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StreamDraw);

        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        _indexCount = _indices.Length;

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        
        GL.BindVertexArray(0);
        
        Objects.Add(this);
    }

    public void UpdateObjectData(SceneObject lightSource, Vector3 cameraPosition)
    {
        var sca = _scale;
        var rot = _rotation;
        var pos = _position;
        _objectMaterial.Color4 = Statics.Numerics4ToColor4(_color);
        
        rot.X = rot.X % 360f + (rot.X < 0 ? 360f : 0f);
        rot.Y = rot.Y % 360f + (rot.Y < 0 ? 360f : 0f);
        rot.Z = rot.Z % 360f + (rot.Z < 0 ? 360f : 0f);


        var shader = _isLightSource ? Main.LightShader : Main.Shader;

        shader?.Use();
        if (!_isLightSource)
        {
            shader?.SetColor4("objectColor", _objectMaterial.Color4);
            shader?.SetVector3("viewPos", cameraPosition);
            
            shader?.SetVector3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));
            shader?.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            shader?.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader?.SetFloat("material.shininess", 32.0f);
            
            shader?.SetVector3("light.ambient",  new Vector3(0.2f, 0.2f, 0.2f) * Statics.Numerics4ToVector3(lightSource._color));
            shader?.SetVector3("light.diffuse",  new Vector3(0.5f, 0.5f, 0.5f) * Statics.Numerics4ToVector3(lightSource._color)); // darken the light a bit to fit the scene
            shader?.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f) * 2 *  Statics.Numerics4ToVector3(lightSource._color));
            shader?.SetVector3("light.position", Statics.Numerics3ToOpentk3(lightSource._position));
        }
        else
        {
            shader?.SetColor4("lightColor", Statics.Numerics4ToColor4(lightSource._color));
        }
        
        _modelMatrix = Matrix4.CreateScale(Statics.Numerics3ToOpentk3(sca)) * 
                      Matrix4.CreateFromAxisAngle(Vector3.UnitX, rot.X) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, rot.Y) * Matrix4.CreateFromAxisAngle(Vector3.UnitZ, rot.Z) * 
                      Matrix4.CreateTranslation(Statics.Numerics3ToOpentk3(pos));
        
        
    }

    public void Draw()
    {
        
        var shaderProgram = _isLightSource ? Main.LightShader?.Handle : Main.Shader?.Handle;
        if (_isLightSource)
            Main.LightShader?.Use();
        else Main.Shader?.Use();
            
        _objectMaterial.Texture.Use(0);
        GL.UniformMatrix4(GL.GetUniformLocation((int)shaderProgram, "model"), true, ref _modelMatrix);
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0);
        
        
        if (!ImGui.Begin("Object Settings")) return;
        ImGui.Text(_title + "'s Transform");
        ImGui.DragFloat3(_title + "'s Position", ref _position, Statics.DragSensitivity);
        ImGui.DragFloat3(_title + "'s Rotation", ref _rotation, Statics.DragSensitivity);
        ImGui.DragFloat3(_title + "'s Scale", ref _scale, Statics.DragSensitivity, 0);
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Text(_title + "'s Material");
        ImGui.ColorEdit4(_title + "'s Color", ref _color);
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.End();
    }
    
    ~SceneObject()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
    }
}