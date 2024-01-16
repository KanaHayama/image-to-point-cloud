namespace Core {
    public sealed class ProcessorOptions {
        public double? TotalWidth { get; set; } = 1;

        public double? TotalHeight { get; set; }

        public AxisDirection RightDirection { get; set; } = AxisDirection.PositiveX;

        public AxisDirection UpDirection { get; set; } = AxisDirection.PositiveY;

        public AxisDirection ForwardDirection { get; set; } = AxisDirection.NegativeZ;

        public float RotateX { get; set; }

        public float RotateY { get; set; }

        public float RotateZ { get; set; }

        public float TranslateX { get; set; }

        public float TranslateY { get; set; }

        public float TranslateZ { get; set; }

    }
}
