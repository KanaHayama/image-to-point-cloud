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
                var totalWidth = accessor.Width * _options.Value.ColumnSpacing;
                var totalHeight = accessor.Height * _options.Value.RowSpacing;
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
                        var x = (float)(j * _options.Value.ColumnSpacing) 
                            - halfWidth 
                            + _options.Value.TranslateX
                            ;//Ideally Positive Right
                        var y = (float)(i * _options.Value.RowSpacing) 
                            - halfHeight
                            + _options.Value.TranslateY
                            ;//Ideally Positive Up, but ...
                        var z = 0 
                            + _options.Value.TranslateZ
                            ;//Ideally Positive Back
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
