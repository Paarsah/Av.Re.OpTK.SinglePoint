using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using OpenTK.Graphics.OpenGL4;

namespace Mag3DView.Views
{
    public class OpenGLControl : NativeControlHost
    {
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _shaderProgram;

        // Vertices for a triangle
        private readonly float[] _pointsVertex = {
            -0.5f, -0.5f, 0.0f, // Bottom-left
             0.5f, -0.5f, 0.0f, // Bottom-right
             0.0f,  0.5f, 0.0f  // Top
        };

        public OpenGLControl()
        {
            // Initialization can be done here if necessary
        }

        // Called when the OpenGL context is initialized.
        public void InitializeOpenGL(GlInterface gl)
        {
            // OpenGL initialization logic
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _pointsVertex.Length * sizeof(float), _pointsVertex, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _shaderProgram = CreateShaderProgram();
            GL.UseProgram(_shaderProgram);
            GL.Enable(EnableCap.DepthTest);
        }

        // Clean up OpenGL resources when the control is disposed of.
        public void DeinitializeOpenGL()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteProgram(_shaderProgram);
        }

        // This method is called on every render frame.
        public void RenderOpenGL(GlInterface gl, int fb)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(_shaderProgram);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

        private int CreateShaderProgram()
        {
            // Compile and link shaders
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, @"
                #version 330 core
                layout (location = 0) in vec3 aPosition;
                void main()
                {
                    gl_Position = vec4(aPosition, 1.0);
                }
            ");
            GL.CompileShader(vertexShader);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, @"
                #version 330 core
                out vec4 FragColor;
                void main()
                {
                    FragColor = vec4(1.0, 0.0, 0.0, 1.0); // Red color
                }
            ");
            GL.CompileShader(fragmentShader);

            var shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return shaderProgram;
        }
    }
}
