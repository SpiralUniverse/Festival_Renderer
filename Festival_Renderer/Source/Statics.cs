namespace Festival_Renderer.Source;
using Assimp;

public static class Statics
{
    public const float CameraSpeed = 1.5f;
    public const float Sensitivity = 0.2f;
    public const float DragSensitivity = 0.025f;

    public static Vector3 Numerics3ToOpentk3(System.Numerics.Vector3 vector) { return new Vector3(vector.X, vector.Y, vector.Z); }
    public static Vector3 Numerics4ToVector3(System.Numerics.Vector4 vector4){ return new Vector3(vector4.X, vector4.Y, vector4.Z); }
    public static Vector4 Numerics4ToOpentk4(System.Numerics.Vector4 vector) { return new Vector4(vector.X, vector.Y, vector.Z, vector.W); }
    public static Color4 Numerics4ToColor4(System.Numerics.Vector4 vector) { return new Color4(vector.X, vector.Y, vector.Z, vector.W); }
    public static Vector4 Color4ToOpentk4(Color4 color4) { return new Vector4(color4.R, color4.G, color4.B, color4.A); }
    public static System.Numerics.Vector3 Opentk3ToNumerics3(Vector3 vector) { return new System.Numerics.Vector3(vector.X, vector.Y, vector.Z); }
    public static System.Numerics.Vector4 Opentk4ToNumerics4(Vector4 vector) { return new System.Numerics.Vector4(vector.X, vector.Y, vector.Z, vector.W); }
    public static System.Numerics.Vector4 Color4ToNumerics4(Color4 color4) { return new System.Numerics.Vector4(color4.R, color4.G, color4.B, color4.A); }
    public static Color4 Opentk4ToColor4(Vector4 vector) { return new Color4(vector.X, vector.Y, vector.Z, vector.W); }

    private static Texture _pink = Texture.LoadFromFile("");
    private static Texture _black = Texture.LoadFromFile("");
    public static void LoadFBXFile(string fbxPath)
    {
        
        AssimpContext importer = new AssimpContext();
        Scene scene = importer.ImportFile(fbxPath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs);



        for (int j = 0; j < scene.Meshes.Count; j++)
        {
            
            float[] vertices = new float[scene.Meshes[j].VertexCount * 8];
            uint[] indices = new uint[scene.Meshes[j].FaceCount * 3];
            
            for (int i = 0; i < scene.Meshes[j].VertexCount; i++)
            {
                vertices[i * 8 + 0] = scene.Meshes[j].Vertices[i].X;
                vertices[i * 8 + 1] = scene.Meshes[j].Vertices[i].Y;
                vertices[i * 8 + 2] = scene.Meshes[j].Vertices[i].Z;

                vertices[i * 8 + 3] = scene.Meshes[j].TextureCoordinateChannels[0][i].X;
                vertices[i * 8 + 4] = scene.Meshes[j].TextureCoordinateChannels[0][i].Y;

                vertices[i * 8 + 5] = scene.Meshes[j].Normals[i].X;
                vertices[i * 8 + 6] = scene.Meshes[j].Normals[i].Y;
                vertices[i * 8 + 7] = scene.Meshes[j].Normals[i].Z;
            }

            for (int i = 0; i < scene.Meshes[j].FaceCount; i++)
            {
                indices[i * 3 + 0] = (uint)scene.Meshes[j].Faces[i].Indices[0];
                indices[i * 3 + 1] = (uint)scene.Meshes[j].Faces[i].Indices[1];
                indices[i * 3 + 2] = (uint)scene.Meshes[j].Faces[i].Indices[2];
            }

            Vector3 position = new Vector3(0, 0, 0);
            Vector3 scale = new Vector3(1, 1, 1); 
            Vector3 rotation = new Vector3(0, 0, 0);
            Color4 color = new Color4(1, 1, 1, 1);

            Mesh mesh = new Mesh("FBX Mesh " + j, position, scale, rotation, color, vertices, indices, "Resources//container2.png", "Resources//container2_specular.png");
        }

    }
}