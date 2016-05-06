using System;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;

namespace BiriBiri.Graphics.WebGL
{
    [FileName("biriBiri.js")]
    public class WebGLSpriteBatch
    {
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
        private readonly Matrix _matrix = Matrix.CreateIdentity();
        private readonly Vector2 _helper = Vector2.Zero;

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

        [FieldProperty]
        public Texture CurrentTexture { get; set; }
        [FieldProperty]
        public Camera CurrentCamera { get; set; }

        [FieldProperty]
        public bool IsStarted { get; private set; }

        private int ViewportWidth { get { return Script.Get<int>(_gl, "drawingBufferWidth"); } }
        private int ViewportHeight { get { return Script.Get<int>(_gl, "drawingBufferHeight"); } }

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

            if (CurrentTexture == null) throw new Exception("Texture must be set before calling Begin()");

            IsStarted = true;
        }

        public void Draw(Vector2 postion, uint frameIndex, Vector2 scale)
        {
            var texCoords = CurrentTexture.Frames[frameIndex];

            var hw = (float)((texCoords.PixelWidth / 2) * scale.X);
            var hh = (float)((texCoords.PixelHeight / 2) * scale.Y);

            var px = (float)postion.X;
            var py = (float)postion.Y;

            var minX = px - hw;
            var maxX = px + hw;

            var minY = py - hh;
            var maxY = py + hh;

            // Tri1 - |/

            var offset = _offset;

            _data[offset + 0] = minX;
            _data[offset + 1] = minY;

            _data[offset + 2] = texCoords.MinX;
            _data[offset + 3] = texCoords.MinY;

            _data[offset + 4] = maxX;
            _data[offset + 5] = minY;

            _data[offset + 6] = texCoords.MaxX;
            _data[offset + 7] = texCoords.MinY;

            _data[offset + 8] = minX;
            _data[offset + 9] = maxY;

            _data[offset + 10] = texCoords.MinX;
            _data[offset + 11] = texCoords.MaxY;

            // Tri2 - /|

            _data[offset + 12] = maxX;
            _data[offset + 13] = minY;

            _data[offset + 14] = texCoords.MaxX;
            _data[offset + 15] = texCoords.MinY;

            _data[offset + 16] = maxX;
            _data[offset + 17] = maxY;

            _data[offset + 18] = texCoords.MaxX;
            _data[offset + 19] = texCoords.MaxY;

            _data[offset + 20] = minX;
            _data[offset + 21] = maxY;

            _data[offset + 22] = texCoords.MinX;
            _data[offset + 23] = texCoords.MaxY;

            _offset += DataSize;
        }

        public void End()
        {
            if (_offset != 0)
            {
                Matrix matrix;

                if (CurrentCamera == null)
                {
                    matrix = _matrix;
                    matrix.InitOrthographicOffCenter(0, ViewportWidth, ViewportHeight, 0, 0, -1);
                }
                else
                {
                    matrix = CurrentCamera.Matrix;
                }

                _gl.UseProgram(_spriteBatchProgram);

                _gl.ActiveTexture(_gl.TEXTURE0);
                _gl.BindTexture(_gl.TEXTURE_2D, CurrentTexture.WebGLTexture);

                _gl.Uniform1i(_samplerLocation, 0);
                _gl.UniformMatrix4fv(_cameraMatrixLocation, false, matrix.Raw.As<Array>());

                _gl.BindBuffer(_gl.ARRAY_BUFFER, _arrayBuffer);
                _gl.BufferData(_gl.ARRAY_BUFFER, _data, _gl.DYNAMIC_DRAW);

                _gl.VertexAttribPointer(_vertexPositionLocation, 2, _gl.FLOAT, false, FloatSize * 4, 0);
                _gl.VertexAttribPointer(_textureCoordLocation, 2, _gl.FLOAT, false, FloatSize * 4, FloatSize * 2);

                _gl.EnableVertexAttribArray(_vertexPositionLocation);
                _gl.EnableVertexAttribArray(_textureCoordLocation);

                _gl.DrawArrays(_gl.TRIANGLES, 0, _offset / 4);

                _gl.BindBuffer(_gl.ARRAY_BUFFER, null);
                _gl.BindTexture(_gl.TEXTURE_2D, null);

                _gl.UseProgram(null);
            }

            _offset = -1;
            IsStarted = false;

            CurrentTexture = null;
            CurrentCamera = null;
        }

        private const int FloatSize = 4;
    }
}
