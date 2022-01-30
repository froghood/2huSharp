using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Touhou.Graphics;

namespace Touhou.Graphics {
	class GameWindow : RenderWindow {

		public Color ClearColor { get; set; }

		private Layers _layers = new(16);

		public GameWindow(VideoMode mode, string title) : base(mode, title) {}
		public GameWindow(VideoMode mode, string title, Styles style) : base(mode, title, style) {}
		public GameWindow(VideoMode mode, string title, Styles style, ContextSettings settings) : base(mode, title, style, settings) {}
		public GameWindow(IntPtr handle) : base(handle) {}
		public GameWindow(IntPtr handle, ContextSettings settings) : base(handle, settings) {}

		public void Draw(int index, Drawable drawable) {
			_layers.Push(index, drawable);
		}

		public new void Clear() {
			base.Clear(ClearColor);
		}

		public Vector2f GetCenter() {
			return (Vector2f)Size / 2f;
		}

		public override void Display() {
			_layers.Draw(this, RenderStates.Default);
			base.Display();
		}

	}
}
