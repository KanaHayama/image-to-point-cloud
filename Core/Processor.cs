using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Core {
    public sealed class Processor {

        private readonly IOptions<ProcessorOptions> _options;

        public Processor(IOptions<ProcessorOptions> options) {
            _options = options;
            if (_options.Value.TotalWidth is null && _options.Value.TotalHeight is null) {
                throw new ArgumentNullException(nameof(_options.Value.TotalWidth), "TotalWidth and TotalHeight cannot be null at the same time.");
            }
        }

        public PointCloud Run(Image<Argb32> image) {
            var width = image.Width;
            var height = image.Height;
            var points = new List<Point>(width * height);
            void accessor(PixelAccessor<Argb32> accessor) {
                var columnSpacing = 0f;
                if (_options.Value.TotalWidth is { } totalWidth) {
                    columnSpacing = width == 1 ? 0f : (float)(totalWidth / (width - 1));
                }
                var rowSpacing = 0f;
                if (_options.Value.TotalHeight is { } totalHeight) {
                    rowSpacing = height == 1 ? 0f : (float)(totalHeight / (height - 1));
                }
                if (_options.Value.TotalWidth is null) {
                    columnSpacing = rowSpacing;
                }
                if (_options.Value.TotalHeight is null) {
                    rowSpacing = columnSpacing;
                }
                totalWidth = accessor.Width * columnSpacing;
                totalHeight = accessor.Height * rowSpacing;
                var halfWidth = (float)(totalWidth / 2);
                var halfHeight = (float)(totalHeight / 2);
                for (var i = 0; i < accessor.Height; i++) {
                    var row = accessor.GetRowSpan(i);
                    for (var j = 0; j < accessor.Width; j++) {
                        var pixel = row[j];
                        var r = pixel.R / 255f;
                        var g = pixel.G / 255f;
                        var b = pixel.B / 255f;
                        var a = pixel.A / 255f;

                        var xx = j * columnSpacing - halfWidth;//2D right
                        var yy = i * rowSpacing - halfHeight;//2D down
                        var zz = 0f;

                        var (x, y, z) = (_options.Value.RightDirection, _options.Value.UpDirection, _options.Value.ForwardDirection) switch { //TOOD: not verified
                            (AxisDirection.PositiveX, AxisDirection.PositiveY, AxisDirection.NegativeZ) => (xx, -yy, -zz),
                            (AxisDirection.PositiveX, AxisDirection.NegativeY, AxisDirection.NegativeZ) => (xx, yy, -zz),
                            _ => throw new NotImplementedException(),
                        };

                        x += _options.Value.TranslateX;
                        y += _options.Value.TranslateY;
                        z += _options.Value.TranslateZ;

                        var point = new Point(x, y, z, r, g, b, a);
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
