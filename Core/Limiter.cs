using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Core {
    public sealed class Limiter {

        private readonly IOptions<LimiterOptions> _options;

        public Limiter(IOptions<LimiterOptions> options) {
            _options = options;
        }

        public bool Limit(in Image<Argb32> image) {
            var count = image.Width * image.Height;
            if (count <= _options.Value.VertexCount) {
                return false;
            }
            var ratio = Math.Sqrt(_options.Value.VertexCount / (double)count);
            var width = (int)Math.Floor(image.Width * ratio);
            var height = (int)Math.Floor(image.Height * ratio);
            var size = new Size(width, height);
            image.Mutate(i => i.Resize(size));
            return true;
        }
    }
}
