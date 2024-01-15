using System.Text;

namespace Core {
    public sealed class Saver {

        private static readonly Encoding UTF8NoBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        public void Save(PointCloud pointCloud, Stream stream) {
            using var textWriter = new StreamWriter(stream, UTF8NoBOM, leaveOpen: true) { 
                NewLine = "\n",
            };
            WriteHeader(textWriter, pointCloud.Count);
            textWriter.Flush();

            using var binaryWriter = new BinaryWriter(stream, UTF8NoBOM, leaveOpen: true);
            WriteData(binaryWriter, pointCloud);
        }

        private static void WriteHeader(StreamWriter writer, int pointCount) {
            writer.WriteLine("ply");
            writer.WriteLine("format binary_little_endian 1.0");
            writer.WriteLine("element vertex " + pointCount);
            writer.WriteLine("property float x");
            writer.WriteLine("property float y");
            writer.WriteLine("property float z");
            writer.WriteLine("property float f_dc_0");
            writer.WriteLine("property float f_dc_1");
            writer.WriteLine("property float f_dc_2");
            writer.WriteLine("end_header");
        }

        private static void WriteData(BinaryWriter writer, PointCloud pointCloud) {
            foreach (var point in pointCloud) {
                writer.Write(point.X);
                writer.Write(point.Y);
                writer.Write(point.Z);
                writer.Write(point.ColorR);
                writer.Write(point.ColorG);
                writer.Write(point.ColorB);
            }
        }
    }
}
