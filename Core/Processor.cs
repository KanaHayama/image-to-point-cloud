﻿using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Core {
    public sealed class Processor {

        private readonly IOptions<ProcessorOptions> _options;

        public Processor(IOptions<ProcessorOptions> options) {
            _options = options;
        }

        public PointCloud Run(Image<Rgb24> image) {
            var width = image.Width;
            var height = image.Height;
            var points = new List<Point>(width * height);
            void accessor(PixelAccessor<Rgb24> accessor) {
                for (var y = 0; y < height; y++) {
                    var row = accessor.GetRowSpan(y);
                    for (var x = 0; x < width; x++) {
                        var pixel = row[x];
                        var r = pixel.R / 255f;
                        var g = pixel.G / 255f;
                        var b = pixel.B / 255f;
                        var point = new Point(x, y, 0, r, g, b);
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