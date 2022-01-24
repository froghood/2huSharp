using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

namespace Touhou.Graphics {
	internal readonly struct SubTexture {
		public SubTexture(int x, int y, int w, int h, float oX, float oY) {
			Region = new(x, y, w, h);
			Origin = new(oX, oY);
		}

		public IntRect Region { get; }
		public Vector2f Origin { get; }
	}
}
