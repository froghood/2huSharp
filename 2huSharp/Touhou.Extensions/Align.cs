namespace Touhou.Extensions {

	using System;
	using SFML.Graphics;
    using SFML.System;

	public static partial class Extension {

		public static void Align(this Text text, Alignment x, Alignment y) {
            FloatRect b = text.GetLocalBounds();
            Vector2f origin = text.Origin;

            switch (x) {
                case Alignment.Center: origin.X = b.Width / 2f + b.Left; break;
                case Alignment.Left: origin.X = b.Left; break;
                case Alignment.Right: origin.X = b.Width + b.Left; break;
            }

            switch (y) {
				case Alignment.Center: origin.Y = b.Height / 2f + b.Top; break;
                case Alignment.Top: origin.Y = b.Top; break;
                case Alignment.Bottom: origin.Y = b.Height + b.Top; break;
            }

			text.Origin = origin;
		}

		public static void Align(this Shape shape, Alignment x, Alignment y) {
			FloatRect b = shape.GetLocalBounds();
			Vector2f origin = shape.Origin;

			switch (x) {
				case Alignment.Center: origin.X = b.Width / 2f + b.Left; break;
				case Alignment.Left: origin.X = b.Left; break;
				case Alignment.Right: origin.X = b.Width + b.Left; break;
			}

			switch (y) {
				case Alignment.Center: origin.Y = b.Height / 2f + b.Top; break;
				case Alignment.Top: origin.Y = b.Top; break;
				case Alignment.Bottom: origin.Y = b.Height + b.Top; break;
			}

			shape.Origin = origin;
		}

		public static int Int(this bool bool_) {
            return bool_ ? 1 : 0;
		}

	}
}

namespace SFML.Graphics {
	public enum Alignment {
		Center,
		Left,
		Right,
		Top,
		Bottom,
	}
}
