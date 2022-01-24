using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace Touhou.Graphics {
	public class Layers : Drawable {

		private Stack<Drawable>[] _layers;

		public Layers(int numLayers) {
			_layers = new Stack<Drawable>[numLayers];

			foreach (int i in Enumerable.Range(0, numLayers)) {
				_layers[i] = new Stack<Drawable>();
			}
		}

		public void Push(int layerIndex, Drawable drawable) {
			_layers[layerIndex].Push(drawable);
		}

		public void Draw(RenderTarget target, RenderStates states) {
			foreach (Stack<Drawable> layer in _layers) {
				while (layer.Count > 0) {
					target.Draw(layer.Pop());
				}
			}
		}
	}
}
