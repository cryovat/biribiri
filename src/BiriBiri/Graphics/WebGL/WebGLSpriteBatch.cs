using System;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;

namespace BiriBiri.Graphics.WebGL
{
    public class WebGLSpriteBatch
    {
        private static readonly Vector2 One = Vector2.One;
        private static readonly TexCoords WholeTexture = new TexCoords(0, 0, 1, 1);

        private const string CameraMatrixUniformName = "uCameraMatrix";
        private const string SamplerUniformName = "uSampler";

        private const string VertexPositionAttributeName = "aVertexPosition";
        private const string TextureCoordAttributeName = "aTextureCoord";

        private const string VertexShader = @"

attribute vec2 aVertexPosition;
attribute vec2 aTextureCoord;

uniform mat4 uCameraMatrix;

varying highp vec2 vTextureCoord;

void main(void) {
    gl_Position = uCameraMatrix * vec4(aVertexPosition, 0, 1);
    vTextureCoord = aTextureCoord;
}
";
        private const string FragmentShader = @"

varying highp vec2 vTextureCoord;

uniform sampler2D uSampler;

void main(void) {
    gl_FragColor = texture2D(uSampler, vec2(vTextureCoord.s, vTextureCoord.t));
}
";

        private readonly WebGLRenderingContext _context;

        private readonly WebGLShader _vertexShader;
        private readonly WebGLShader _fragmentShader;
        private readonly WebGLProgram _spriteBatchProgram;

        private readonly WebGLBuffer _arrayBuffer;
        private readonly Float32Array _data;

        private WebGLUniformLocation _cameraMatrixLocation;
        private WebGLUniformLocation _samplerLocation;

        private int _vertexPositionLocation;
        private int _textureCoordLocation;

        private int _offset;

        private const int DataSize = 24;

        public WebGLTexture CurrentTexture;
        public Camera CurrentCamera;

        public bool IsStarted;

        public WebGLSpriteBatch(WebGLRenderingContext context, uint capacity = 1000)
        {
            _context = context;

            _arrayBuffer = _context.CreateBuffer();
            _data = new Float32Array(capacity * DataSize);

            _vertexShader = context.CreateShader(context.VERTEX_SHADER);
            _fragmentShader = context.CreateShader(context.FRAGMENT_SHADER);

            _context.ShaderSource(_vertexShader, VertexShader);
            _context.CompileShader(_vertexShader);

            if (_context.GetError() != _context.NO_ERROR) throw new Exception("Vertex shader compilation failed: " + _context.GetShaderInfoLog(_vertexShader));

            _context.ShaderSource(_fragmentShader, FragmentShader);
            _context.CompileShader(_fragmentShader);

            if (_context.GetError() != _context.NO_ERROR) throw new Exception("Fragment shader compilation failed: " + _context.GetShaderInfoLog(_fragmentShader));

            _spriteBatchProgram = context.CreateProgram().As<WebGLProgram>();

            _context.AttachShader(_spriteBatchProgram, _vertexShader);
            _context.AttachShader(_spriteBatchProgram, _fragmentShader);

            _context.LinkProgram(_spriteBatchProgram);

            _cameraMatrixLocation = _context.GetUniformLocation(_spriteBatchProgram, CameraMatrixUniformName);
            _samplerLocation = _context.GetUniformLocation(_spriteBatchProgram, SamplerUniformName);

            _vertexPositionLocation = _context.GetAttribLocation(_spriteBatchProgram, VertexPositionAttributeName);
            _textureCoordLocation = _context.GetAttribLocation(_spriteBatchProgram, TextureCoordAttributeName);
        }

        public void Begin()
        {
            _offset = 0;

            if (CurrentCamera == null) throw new Exception("Camera must be set before calling Begin()");
            if (CurrentTexture == null) throw new Exception("Texture must be set before calling Begin()");

            IsStarted = true;
        }

        public void Draw(Vector2 postion, Vector2 size = null, Vector2 scale = null, TexCoords? texCoords = null)
        {
            size = size ?? One;
            scale = scale ?? One;
            var coords = texCoords ?? WholeTexture;

            var hw = (float)((size.X / 2) * scale.X);
            var hh = (float)((size.Y / 2) * scale.Y);

            var px = (float)postion.X;
            var py = (float) postion.Y;

            var minX = px - hw;
            var maxX = px + hw;

            var minY = py - hh;
            var maxY = py + hh;

            // Tri1 - |/

            var offset = _offset;

            _data[offset+0] = minX;
            _data[offset+1] = minY;

            _data[offset+2] = coords.MinX;
            _data[offset+3] = coords.MinY;

            _data[offset+4] = maxX;
            _data[offset+5] = minY;

            _data[offset+6] = coords.MaxX;
            _data[offset+7] = coords.MinY;

            _data[offset+8] = minX;
            _data[offset+9] = maxY;

            _data[offset+10] = coords.MinX;
            _data[offset+11] = coords.MaxY;

            // Tri2 - /|

            _data[offset+12] = maxX;
            _data[offset+13] = minY;

            _data[offset+14] = coords.MaxX;
            _data[offset+15] = coords.MinY;

            _data[offset+16] = maxX;
            _data[offset+17] = maxY;

            _data[offset+18] = coords.MaxX;
            _data[offset+19] = coords.MaxY;

            _data[offset+20] = minX;
            _data[offset+21] = maxY;

            _data[offset+22] = coords.MinX;
            _data[offset+23] = coords.MaxY;

            _offset += DataSize;
        }

        public void End()
        {
            if (_offset != 0)
            {
                _context.UseProgram(_spriteBatchProgram);

                _context.BindBuffer(_context.ARRAY_BUFFER, _arrayBuffer);
                _context.BufferData(_context.ARRAY_BUFFER, _data, _context.DYNAMIC_DRAW);

                _context.VertexAttribPointer(_vertexPositionLocation, 2, _context.FLOAT, false, 8, 0);
                _context.VertexAttribPointer(_textureCoordLocation, 2, _context.FLOAT, false, 8, 8);

                _context.BindTexture(_context.TEXTURE_2D, CurrentTexture);
                _context.Uniform1i(_samplerLocation, 0);

                _context.UniformMatrix4fv(_cameraMatrixLocation, false, CurrentCamera.Matrix.Raw.As<Array>());

                _context.DrawArrays(_context.TRIANGLES, 0, _offset / DataSize);

                _context.BindBuffer(_context.ARRAY_BUFFER, null);
                _context.BindTexture(_context.TEXTURE_2D, null);

                _context.UseProgram(null);
            }

            _offset = -1;
            IsStarted = false;
        }
    }
}
