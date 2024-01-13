using System.Collections;

namespace Core {
    public sealed class PointCloud : IReadOnlyList<Point> {

        private readonly Point[] _points;

        public PointCloud(IEnumerable<Point> points) {
            _points = points.ToArray();
        }

        #region IReadOnlyList

        public Point this[int index] => _points[index];

        public int Count => _points.Length;

        public IEnumerator<Point> GetEnumerator() => ((IReadOnlyList<Point>)_points).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();

        #endregion
    }
}
