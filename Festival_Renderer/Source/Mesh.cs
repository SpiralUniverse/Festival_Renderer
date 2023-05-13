namespace Festival_Renderer.Source;

public struct Primitives
{
    public struct Cube
    {
        private readonly int _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public readonly Mesh Mesh;

        public Cube(string diffuseTextureName, string specularTextureName, int id, bool isUsingTexture = true)
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
            
            Mesh = new Mesh(_title, _id, Vector3.Zero, Vector3.One, Vector3.Zero, new Color4(0.5f, 0.5f, 0.5f, 1.0f), _vertices,
                _indices, "Resources//" + diffuseTextureName, "Resources//" + specularTextureName, isUsingTexture: isUsingTexture);
        }
    }
    
    public struct Triangle
    {
        private readonly int _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public Mesh Mesh;

        public Triangle(string diffuseTextureName, string specularTextureName, int id)
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
            
            Mesh = new Mesh(_title, _id, Vector3.Zero, Vector3.One, Vector3.Zero, new Color4(0.5f, 0.5f, 0.5f, 1.0f), _vertices,
                _indices, "Resources//" + diffuseTextureName, "Resources//" + specularTextureName);
        }
    }

    public struct Plane
    {
        private readonly int _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public Mesh Mesh;

        public Plane(string diffuseTextureName, string specularTextureName, int id)
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

            Mesh = new Mesh(_title, _id, Vector3.Zero, Vector3.One, new Vector3(90, 0, 0),
                new Color4(0.5f, 0.5f, 0.5f, 1.0f), _vertices, _indices, "Resources//" + diffuseTextureName, "Resources//" + specularTextureName);
        }
    }
    
    public struct LightPlane
    {
        private readonly int _id;
        private readonly string _title;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        public readonly Mesh Mesh;

        public LightPlane(string diffuseTextureName, string specularTextureName, int id, bool isLightSource)
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

            Mesh = new Mesh(_title, _id, new Vector3(0, 1, -2.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0),
                Color4.White, _vertices, _indices, "Resources//" + diffuseTextureName, "Resources//" + specularTextureName, isLightSource);
        }
    }
    
    public struct LightSphere
    {
        public LightSphere()
        {
            Statics.LoadModelFile("Resources\\sphere.fbx", true);
        }
    }
}

public class Mesh
{
    public static readonly List<Mesh> PointLights = new();
    public static readonly List<Mesh> Meshes = new();
    public Vector3 Position => Statics.Numerics3ToOpentk3(_position);
    public int Id => _id;
    
    private Matrix4 _modelMatrix;
    private  Material _objectMaterial;
    
    private readonly int _vao;
    private readonly int _vbo;
    private readonly int _ebo;
    private readonly int _indexCount;
    private readonly string _title;
    private int _id;

    
    private readonly bool _isLightSource;

    private System.Numerics.Vector3 _position;
    private System.Numerics.Vector3 _scale;
    private System.Numerics.Vector3 _rotation;
    private System.Numerics.Vector4 _color;

    private bool _isUsingTexture;
    private readonly float[] _vertices;
    private readonly uint[] _indices;

    public string Name => _title;

    private struct Material
    {
        public Color4 Color4;
        public readonly Texture DiffuseTexture;
        public readonly Texture SpecularTexture;

        public Material(string diffuseTexturePath, string specularTexturePath)
        {
            
            Color4 = Color4.White;
            DiffuseTexture = Texture.LoadFromFile(diffuseTexturePath);
            SpecularTexture = Texture.LoadFromFile(specularTexturePath);

        }
        
    }

    public Mesh(string title,int id, Vector3 position, Vector3 scale, Vector3 rotation, Color4 color, float[] vertices, uint[] indices, string diffuseTexturePath, string specularTexturePath, bool isLightSource = false, bool isUsingTexture = false)
    {
        _objectMaterial = new Material(diffuseTexturePath, specularTexturePath);
        _position = Statics.Opentk3ToNumerics3(position);
        _scale = Statics.Opentk3ToNumerics3(scale);
        _rotation = Statics.Opentk3ToNumerics3(rotation);
        _color = Statics.Color4ToNumerics4(color);
        _vertices = vertices;
        _indices = indices;
        _title = title;
        _isLightSource = isLightSource;
        _isUsingTexture = isUsingTexture;
        _id = id;

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
        
        
        if(_isLightSource)
            PointLights.Add(this);
        
        Meshes.Add(this);
    }

    public void UpdateObjectData(Vector3 cameraPosition)
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
            shader?.SetVector3("viewPos", cameraPosition);
            
            _objectMaterial.DiffuseTexture.Use(TextureUnit.Texture0);
            shader?.SetInt("material.diffuse", 0);
            _objectMaterial.SpecularTexture.Use(TextureUnit.Texture1);
            shader?.SetInt("material.specular", 1);
            shader?.SetFloat("material.shininess", 32.0f);
            shader?.SetColor4("material.color", _objectMaterial.Color4);

            
            shader?.SetVector3("dirLight.color", Statics.Numerics3ToOpentk3(Statics.SunColor / 255));
            shader?.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            shader?.SetVector3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
            shader?.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
            shader?.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader?.SetInt("isUsingTexture", _isUsingTexture ? 1 : 0);

            for (var i = 0; i < PointLights.Count; i++)
            {
                shader?.SetVector3($"pointLights[{i}].position", Main.PointLightPositions[i]);
                shader?.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
                shader?.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
                shader?.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
                shader?.SetFloat($"pointLights[{i}].constant", 1.0f);
                shader?.SetFloat($"pointLights[{i}].linear", 0.09f);
                shader?.SetFloat($"pointLights[{i}].quadratic", 0.032f);
                shader?.SetVector3($"pointLights[{i}].color", new Vector3(PointLights[i]._color.X, PointLights[i]._color.Y, PointLights[i]._color.Z));
            }
        }
        else
        {
            shader?.SetColor4("lightColor", Statics.Numerics4ToColor4(_color));
        }
        
        _modelMatrix = Matrix4.CreateScale(Statics.Numerics3ToOpentk3(sca)) * 
                      Matrix4.CreateFromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(rot.X)) * 
                      Matrix4.CreateFromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(rot.Y)) * 
                      Matrix4.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(rot.Z)) * 
                      Matrix4.CreateTranslation(Statics.Numerics3ToOpentk3(pos));
        
        
    }

    public void Draw()
    {
        
        var shaderProgram = _isLightSource ? Main.LightShader?.Handle : Main.Shader?.Handle;
        if (_isLightSource)
            Main.LightShader?.Use();
        else Main.Shader?.Use();
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
    
    public double? IntersectsRay(Vector3 rayDirection)
    {
        var radius = _scale.X;
        var difference = Position - rayDirection;
        var differenceLengthSquared = difference.LengthSquared;
        var sphereRadiusSquared = radius * radius;
        if (differenceLengthSquared < sphereRadiusSquared)
        {
            return 0d;
        }
        var distanceAlongRay = Vector3.Dot(rayDirection, difference);
        if (distanceAlongRay < 0)
        {
            return null;
        }
        var dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;
        var result = (dist < 0) ? null : distanceAlongRay - (double?)Math.Sqrt(dist);
        return result;
    }
    
    ~Mesh()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
    }

    public void SetPosition(Vector3 newPosition)
    {
        _position = Statics.Opentk3ToNumerics3(newPosition);
    }
}