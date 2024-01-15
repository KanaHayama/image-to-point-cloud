using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Core {
    public sealed class Processor {

        private readonly IOptions<ProcessorOptions> _options;

        public Processor(IOptions<ProcessorOptions> options) {
            _options = options;
        }

        public PointCloud Run(Image<Argb32> image) {
            var width = image.Width;
            var height = image.Height;
            var points = new List<Point>(width * height);
            void accessor(PixelAccessor<Argb32> accessor) {
                for (var i = 0; i < height; i++) {
                    var row = accessor.GetRowSpan(i);
                    for (var j = 0; j < width; j++) {
                        var pixel = row[j];
                        var r = pixel.R / 255f;
                        var g = pixel.G / 255f;
                        var b = pixel.B / 255f;
                        var a = pixel.A / 255f;
                        var x = (float)(j * _options.Value.ColumnSpacing);
                        var y = (float)(i * _options.Value.RowSpacing);
                        var point = new Point(x, y, 0, r, g, b, a);
                        points.Add(point);
                    }
                }
            }
            image.ProcessPixelRows(accessor);
            var result = new PointCloud(points);
            return result;
        }
    }
}
