using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace Touhou.Graphics {
	internal class FontManager {

		private Dictionary<string, Font> _fonts;
		private string _fontDirectory;

		public FontManager(string fontDirectory) {
			_fontDirectory = fontDirectory;
			_fonts = new Dictionary<string, Font>();
		}

		public void LoadFont(string name) {
			Font font = new($@"{_fontDirectory}\{name}.ttf");
			_fonts.Add(name, font);
		}

		public Font GetFont(string name) {
			return _fonts[name];
		}

	}
}
