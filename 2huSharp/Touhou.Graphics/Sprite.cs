using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

namespace Touhou.Graphics {
	public class Sprite : SFML.Graphics.Sprite {

		public Sprite() : base() { }
		public Sprite(string textureName, string subTextureName) : base() {
			var textureSheet = Game.TextureManager.GetTextureSheet(textureName);
			Texture = textureSheet.Texture;
			var subTexture = textureSheet.Atlas[subTextureName];
			TextureRect = subTexture.Region;
			Origin = subTexture.Origin;
		}

	}
	//public class Sprite : Transformable, Drawable {

		//private SFML.Graphics.Sprite _sfSprite = new();

		//public Sprite() { }

		//public Sprite(string id) {
		//	_changeSprite(id);
		//}

		//public override void Draw(RenderTarget target, RenderStates states) {
		//	_sfSprite.Position = Position;
		//	_sfSprite.Scale = Scale;
		//	_sfSprite.Rotation = Rotation;
		//	target.Draw(_sfSprite);
		//}

		//public Vector2f Position = new(0f, 0f);
		//public Vector2f Scale = new(1f, 1f);
		//public float Rotation = 0f;

		//private void _changeSprite(string id) {
		//	SubTexture subTexture = Renderer.GetSubTexture(id);
		//	_sfSprite.Texture = Renderer.TextureSheet;
		//	_sfSprite.TextureRect = subTexture.Region;
		//	_sfSprite.Origin = subTexture.Origin;
		//}

		//public string Id {
		//	set {
		//		_changeSprite(value);
		//	}
		//}
	//	public void Draw(RenderTarget target, RenderStates states) {
	//		throw new NotImplementedException();
	//	}
	//}
}
