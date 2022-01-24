using System;
using System.IO;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Newtonsoft.Json;

namespace Touhou.Graphics {

	internal class TextureManager {

		private Dictionary<string, TextureSheet> _textureCache;
		private string _texturesDirectory;

		public TextureManager(string texturesDirectoy) {
			_textureCache = new Dictionary<string, TextureSheet>();
			_texturesDirectory = texturesDirectoy;
		}


		public TextureSheet GetTextureSheet(string name) {
			//if (_subTextureMap.TryGetValue(textureId, out SubTexture texture)) {
			//	subTexture = texture;
			//} else {
			//	throw new Exception($"Subtexture \"{textureId}\" not found.");
			//}

			if (!_textureCache.ContainsKey(name)) throw new Exception($"Texture sheet \"{name}\" not loaded.");
			return _textureCache[name];
		}

		
		
		public void LoadTexture(string name) {
			if (_textureCache.ContainsKey(name)) return;

			string path = $@"{_texturesDirectory}\{name}";
			_textureCache.Add(name, new TextureSheet($"{path}.png", $"{path}.json"));

			//string path = $@"{_texturesDirectory}\{name}";
			//Texture texture = new($"{path}.png");
			//texture.Smooth = true;
			//_textureCache.Add(name, texture);

		}

		public void UnloadTexture(string textureName) {
			_textureCache.Remove(textureName);
		}
	}
}
