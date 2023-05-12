namespace Festival_Renderer.Source;

public class Grid
{
    private readonly int _vao;
    private readonly int _vbo;
    private readonly Shader _shader;

    private int _numLines;
    private float _lineSpacing;
    private Color4 _color4;

    public Grid(Shader shader, int numLines, float lineSpacing, Color4 color)
    {
        _shader = shader;
        _color4 = color;
        _numLines = numLines;
        _lineSpacing = lineSpacing;

        // Vertex data for the lines
        float[] vertices = new float[_numLines * 4 * 3];

        for (int i = 0; i < _numLines; i++)
        {
            // Vertical lines
            vertices[i * 12] = i * _lineSpacing - ((_numLines - 1) * _lineSpacing) / 2f; // x1
            vertices[i * 12 + 1] = 0f; // y1
            vertices[i * 12 + 2] = -((_numLines - 1) * _lineSpacing) / 2f; // z1
            vertices[i * 12 + 3] = i * _lineSpacing - ((_numLines - 1) * _lineSpacing) / 2f; // x2
            vertices[i * 12 + 4] = 0f; // y2
            vertices[i * 12 + 5] = ((_numLines - 1) * _lineSpacing) / 2f; // z2

            // Horizontal lines
            vertices[i * 12 + 6] = -((_numLines - 1) * _lineSpacing) / 2f; // x1
            vertices[i * 12 + 7] = 0f; // y1
            vertices[i * 12 + 8] = i * _lineSpacing - ((_numLines - 1) * _lineSpacing) / 2f; // z1
            vertices[i * 12 + 9] = ((_numLines - 1) * _lineSpacing) / 2f; // x2
            vertices[i * 12 + 10] = 0f; // y2
            vertices[i * 12 + 11] = i * _lineSpacing - ((_numLines - 1) * _lineSpacing) / 2f; // z2
        }

        // Create VAO and VBO
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindVertexArray(0);
    }

    public void Draw(Matrix4 view, Matrix4 projection)
    {
        _shader.Use();

        _shader.SetMatrix4("view", view);
        _shader.SetMatrix4("proj", projection);
        _shader.SetMatrix4("model", Matrix4.Identity);
        _shader.SetColor4("color", _color4);
        //_shader.SetVector3("FogColor", new Vector3(0.5f, 0.5f, 0.5f));
        //_shader.SetFloat("FogDensity", 0.1f);


        GL.BindVertexArray(_vao);

        GL.DrawArrays(PrimitiveType.Lines, 0, _numLines * 4);

        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(_vbo);
        GL.DeleteVertexArray(_vao);
    }
}