using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Touhou.IO;

namespace Touhou.Scene {
	class LoadingScene : IScene {
		public LoadingScene(Action toLoad, Action onLoad) {
			_onLoad = onLoad;
			_task = new(toLoad);
			//_circleShape.Position = (Vector2f)Renderer.WindowSize / 2f;
			//_circleShape.FillColor = Color.Transparent;
			//_circleShape.OutlineColor = Color.White;
			//_circleShape.OutlineThickness = 2f;

			_loadingCircleTexture = new(128, 128);
			_loadingCircleTexture.Smooth = true;
			_loadingCircleTexture.Clear(Color.Transparent);		
			_vertices = new Vertex[32 + 1];
			_vertices[0] = new Vertex((Vector2f)_loadingCircleTexture.Size / 2f, Color.White);
			for (int i = 1; i < _vertices.Length; i++) {
				float angle = MathF.Tau / _vertices.Length * i;
				_vertices[i] = new Vertex(new Vector2f(
					_loadingCircleTexture.Size.X / 2 + 50f * MathF.Cos(angle),
					_loadingCircleTexture.Size.Y / 2 + 50f * MathF.Sin(angle)
				), Color.White);
			}
			_loadingCircleTexture.Draw(_vertices, PrimitiveType.TriangleFan);
			CircleShape center = new CircleShape(50 - 2);
			center.FillColor = Color.Transparent;
			center.Position = (Vector2f)_loadingCircleTexture.Size / 2f;
			center.Origin = new Vector2f(center.Radius, center.Radius);
			_loadingCircleTexture.Draw(center, new RenderStates(BlendMode.None));
			_loadingCircleTexture.Display();
			_loadingCircleSprite = new(_loadingCircleTexture.Texture);
			_loadingCircleSprite.Origin = (Vector2f)_loadingCircleTexture.Size / 2f;
			_loadingCircleSprite.Position = (Vector2f)Game.Window.Size / 2f;

			_task.Start();
		}

		public void Update(float time, float delta) {
			//Console.WriteLine(time);
			if (_task.IsCompleted) _onLoad.Invoke();
		}

		public void Render(float time, float delta) {
			//_circleShape.Radius += delta * 10f;


			_loadingCircleSprite.Rotation += delta * 500f;
			Game.Window.Draw(0, _loadingCircleSprite);
		}
		public void InputPressed(InputData inputData) {}
		public void InputReleased(InputData inputData) {}

		private Task _task;
		private Action _onLoad;
		private Vertex[] _vertices;
		private RenderTexture _loadingCircleTexture;
		private SFML.Graphics.Sprite _loadingCircleSprite;
	}
}
