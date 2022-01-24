using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;
//using SFML.Graphics;

using Touhou.IO;
using Touhou.Graphics;
using Touhou.Extensions;


namespace Touhou {
	internal class Player : ISimulated, IInput {

		public Player(float x, float y) {
			Position = new(x, y);
		}

		public void Update(float time, float delta) {
			float DirAngle = MathF.Atan2(InputDir.Y, InputDir.X);
			Velocity.X = InputDir.X * Movespeed * MathF.Abs(MathF.Cos(DirAngle));
			Velocity.Y = InputDir.Y * Movespeed * MathF.Abs(MathF.Sin(DirAngle));

			Position += Velocity * delta;
			//Console.WriteLine(InputDir);
		}

		public void Render(float time, float delta) {
			if (InputDir.X != 0) {
				FacingDir = -InputDir.X;
			}

			Sprite sprite = new("Reimu", "ReimuIdle1");
			sprite.Position = Position;
			sprite.Scale = new Vector2f(FacingDir, 1f);

			Game.Window.Draw(0, sprite);
		}

		public void InputPressed(InputData inputData) {
			if (inputData.Type == Input.Type.Key || inputData.Type == Input.Type.Button) {
				Input.Action action = inputData.Action;
				InputDir.X += (action == Input.Action.Right).Int() - (action == Input.Action.Left).Int();
				InputDir.Y += (action == Input.Action.Down).Int() - (action == Input.Action.Up).Int();
			}
		}

		public void InputReleased(InputData inputData) {
			if (inputData.Type == Input.Type.Key || inputData.Type == Input.Type.Button) {
				Input.Action action = inputData.Action;
				InputDir.X -= (action == Input.Action.Right).Int() - (action == Input.Action.Left).Int();
				InputDir.Y -= (action == Input.Action.Down).Int() - (action == Input.Action.Up).Int();
			}
		}

		private Vector2f Position;

		private Vector2i InputDir = new(0, 0);
		private int FacingDir = 1;
		private Vector2f Velocity;

		private float Movespeed = 520; //pixels per second
	}
}
