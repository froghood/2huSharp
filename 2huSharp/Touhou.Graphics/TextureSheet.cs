using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

using Newtonsoft.Json;

namespace Touhou.Graphics {
	internal class TextureSheet {

		public Texture Texture;
		public Dictionary<string, SubTexture> Atlas;

		public TextureSheet(string texturePath, string jsonPath) {
			Texture = new Texture(texturePath);
			Texture.Smooth = true;
			var json = File.ReadAllText(jsonPath);
			Atlas = JsonConvert.DeserializeObject<Dictionary<string, SubTexture>>(json);
		}

	}
}
