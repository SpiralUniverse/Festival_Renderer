namespace Festival_Renderer.Source;

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

}