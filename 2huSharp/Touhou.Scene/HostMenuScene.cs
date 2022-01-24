using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Touhou.UI;
using Touhou.IO;
using Touhou.Extensions;

namespace Touhou.Scene {
	internal class HostMenuScene : IScene {

		public HostMenuScene() {
			_baseMenuSelector = new MenuSelector() {
				Id = "main",
				Position = (Vector2f)Game.Window.Size / 2f
			};

			_baseMenuSelector.OnInputPressed += (InputData inputData) => {
				_baseMenuSelector.Index +=
					(inputData.Action == Input.Action.Down).Int() -
					(inputData.Action == Input.Action.Up).Int();
			};

			var menuOptionBack = _baseMenuSelector.AddMenu<MenuOption>("Back", new Vector2f(0f, 80f));
			menuOptionBack.Text.FillColor = Color.White;

			foreach (Menu menu in _baseMenuSelector.GetMenus()) {
			}
		}

		public void Update(float time, float delta) {
			throw new NotImplementedException();
		}

		public void Render(float time, float delta) {
			throw new NotImplementedException();
		}

		public void InputPressed(InputData inputData) {
			throw new NotImplementedException();
		}

		public void InputReleased(InputData inputData) {
			throw new NotImplementedException();
		}

		private MenuSelector _baseMenuSelector;
	}
}
