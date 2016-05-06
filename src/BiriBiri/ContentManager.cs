using System;
using BiriBiri.Graphics;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;

namespace BiriBiri
{
    public delegate void ContentLoadedHandler(string[] failed);

    public class ContentManager
    {
        public const string DefaultTextureId = "BiriBiriDefault";
        public const string DefaultTextureData = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAMAAACdt4HsAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURYCAgP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADg4B1YAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAADSSURBVFhH7dChEQAgDMDAsv/SIN7mugCvozKbs5A1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XZI1XfqTHl2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2SNV2ShZkLFA8IAbPs+pkAAAAASUVORK5CYII=";

        private readonly object _map = new object();
        private readonly WebGLRenderingContext _context;
        private readonly ContentLoadedHandler _contentLoadedHandler;

        private readonly string[] _failedAssets = new string[0];

        [FieldProperty]
        public int TotalCount { get; private set; }
        [FieldProperty]
        public int FailedCount { get; private set; }
        [FieldProperty]
        public int LoadedCount { get; private set; }

        public int PendingCount
        {
            get { return TotalCount - LoadedCount - FailedCount; }
        }

        public ContentManager(WebGLRenderingContext context, ContentLoadedHandler contentLoadedHandler)
        {
            _context = context;
            _contentLoadedHandler = contentLoadedHandler;

            AddTexture(DefaultTextureId, DefaultTextureData);
        }

        public void AddTexture(string id, string url)
        {
            TotalCount++;

            var image = new ImageElement();
            image.OnLoad = evt =>
            {
                var tex = _context.CreateTexture();

                _context.BindTexture(_context.TEXTURE_2D, tex);
                _context.TexImage2D(_context.TEXTURE_2D, 0, _context.RGBA, _context.RGBA, _context.UNSIGNED_BYTE, image);
                _context.TexParameteri(_context.TEXTURE_2D, _context.TEXTURE_MAG_FILTER, _context.NEAREST);
                _context.TexParameteri(_context.TEXTURE_2D, _context.TEXTURE_MIN_FILTER, _context.NEAREST);
                _context.BindTexture(_context.TEXTURE_2D, null);

                Script.Set(_map, id, new Texture(tex, (uint)image.NaturalWidth, (uint)image.NaturalHeight));

                LoadedCount++;

                if (TotalCount == LoadedCount + FailedCount)
                {
                    _contentLoadedHandler(_failedAssets);
                }
            };

            image.OnError = (message, url2, lineNumber, columnNumber, error) =>
            {
                FailedCount++;

                if (TotalCount == LoadedCount + FailedCount)
                {
                    _failedAssets.Push(url);
                    _contentLoadedHandler(_failedAssets);
                }

                return true;
            };

            image.Src = url;
        }

        public Texture GetTexture(string id)
        {
            var tex = Script.Get<Texture>(_map, id);

            if (tex == null) throw new Exception("Texture unknown or load failed: " + id);

            return tex;
        }
    }
}
