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

        private readonly WebGLRenderingContext _gl;

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

        public WebGLSpriteBatch(WebGLRenderingContext gl, uint capacity = 1000)
        {
            _gl = gl;

            _arrayBuffer = _gl.CreateBuffer();
            _data = new Float32Array(capacity * DataSize);

            _vertexShader = gl.CreateShader(gl.VERTEX_SHADER);
            _fragmentShader = gl.CreateShader(gl.FRAGMENT_SHADER);

            _gl.ShaderSource(_vertexShader, VertexShader);
            _gl.CompileShader(_vertexShader);

            if (_gl.GetError() != _gl.NO_ERROR) throw new Exception("Vertex shader compilation failed: " + _gl.GetShaderInfoLog(_vertexShader));

            _gl.ShaderSource(_fragmentShader, FragmentShader);
            _gl.CompileShader(_fragmentShader);

            if (_gl.GetError() != _gl.NO_ERROR) throw new Exception("Fragment shader compilation failed: " + _gl.GetShaderInfoLog(_fragmentShader));

            _spriteBatchProgram = gl.CreateProgram().As<WebGLProgram>();

            _gl.AttachShader(_spriteBatchProgram, _vertexShader);
            _gl.AttachShader(_spriteBatchProgram, _fragmentShader);

            _gl.LinkProgram(_spriteBatchProgram);

            _cameraMatrixLocation = _gl.GetUniformLocation(_spriteBatchProgram, CameraMatrixUniformName);
            _samplerLocation = _gl.GetUniformLocation(_spriteBatchProgram, SamplerUniformName);

            _vertexPositionLocation = _gl.GetAttribLocation(_spriteBatchProgram, VertexPositionAttributeName);
            _textureCoordLocation = _gl.GetAttribLocation(_spriteBatchProgram, TextureCoordAttributeName);
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
                _gl.UseProgram(_spriteBatchProgram);

                _gl.ActiveTexture(_gl.TEXTURE0);
                _gl.BindTexture(_gl.TEXTURE_2D, CurrentTexture);

                _gl.Uniform1i(_samplerLocation, 0);
                _gl.UniformMatrix4fv(_cameraMatrixLocation, false, CurrentCamera.Matrix.Raw.As<Array>());

                _gl.BindBuffer(_gl.ARRAY_BUFFER, _arrayBuffer);
                _gl.BufferData(_gl.ARRAY_BUFFER, _data, _gl.DYNAMIC_DRAW);

                _gl.VertexAttribPointer(_vertexPositionLocation, 2, _gl.FLOAT, false, BytesInFloat * 4, 0);
                _gl.VertexAttribPointer(_textureCoordLocation, 2, _gl.FLOAT, false, BytesInFloat * 4, BytesInFloat * 2);

                _gl.EnableVertexAttribArray(_vertexPositionLocation);
                _gl.EnableVertexAttribArray(_textureCoordLocation);

                _gl.DrawArrays(_gl.TRIANGLES, 0, _offset / 4);

                _gl.BindBuffer(_gl.ARRAY_BUFFER, null);
                _gl.BindTexture(_gl.TEXTURE_2D, null);

                _gl.UseProgram(null);
            }

            _offset = -1;
            IsStarted = false;
        }

        private const int BytesInFloat = 4;
    }
}
