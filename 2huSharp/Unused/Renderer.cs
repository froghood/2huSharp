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

		const int NumLayers = 16;

		public Vector2u WindowSize { get => _windowSize; }
		private Vector2u _windowSize;

		public Texture SpriteSheet;
		public Font Font;

		private string _textureDirectory;

		public TextureManager(string textureDirectory) {
			_textureDirectory = textureDirectory;
		}

		public void Draw(int layerIndex, Drawable drawable) {
			_layers.Draw(layerIndex, drawable);
		}

		public Texture GetSubTexture(string spriteId, out SubTexture subTexture) {
			//if (_subTextureMap.TryGetValue(textureId, out SubTexture texture)) {
			//	subTexture = texture;
			//} else {
			//	throw new Exception($"Subtexture \"{textureId}\" not found.");
			//}

			if (!_subTextureMap.ContainsKey(spriteId)) throw new Exception($"SubTexture \"{spriteId}\" not found.");
			subTexture = _subTextureMap[spriteId];
			return _textureLookup[subTexture.TextureName];
		}

		public void Display() {
			_window.Clear(new Color(60, 60, 60));
			_window.Draw(_layers);
			_window.Display();
		}

		public bool IsWindowOpen() {
			return _window.IsOpen;
		}

		public void DispatchEvents() {
			_window.DispatchEvents();
		}

		public void CloseWindow() {
			_window.Close();
		}

		private RenderWindow _window;

		private Layers _layers = new(NumLayers);

		private Dictionary<string, SubTexture> _subTextureMap;
		
		public void LoadTexture(string textureName) {
			if (_textureLookup.ContainsKey(textureName)) return;
			Texture texture = new($"assets/{textureName}.png");
			texture.Smooth = true;
			_textureLookup.Add(textureName, texture);
		}

		public void UnloadTexture(string textureName) {
			_textureLookup.Remove(textureName);
		}

		public void ParseTextureJson(string path) {
			_subTextureMap = JsonConvert.DeserializeObject<Dictionary<string, SubTexture>>(File.ReadAllText(path));
		}

		private static Dictionary<string, Texture> _textureLookup = new();

		
	}

	
}
