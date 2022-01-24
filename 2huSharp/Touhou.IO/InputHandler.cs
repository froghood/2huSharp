using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;
using SFML.Graphics;

namespace Touhou.IO {
	internal class InputHandler {

		public bool IsTyping = false;

		private Dictionary<Keyboard.Key, Input.Action> _actionsByKey;

		public InputHandler() {
			_actionsByKey = new Dictionary<Keyboard.Key, Input.Action>();
		}

		public void BindEvents(RenderWindow window) {
			window.Closed += _closed;
			window.KeyPressed += _keyPressed;
			window.KeyReleased += _keyReleased;
			window.TextEntered += _textEntered;

			//window.KeyPressed += (_, e) => { Game.SceneManager.ActiveScene.InputPressed(new InputData(e.Code)); }; 
			//window.KeyReleased += (_, e) => { Game.SceneManager.ActiveScene.InputReleased(new InputData(e.Code)); };
			//window.TextEntered += (_, e) => { if (IsTyping) Game.SceneManager.ActiveScene.InputPressed(new InputData(e.Unicode)); };
			//context.JoystickMoved += (s, e) => { Game.State.InputPressed(new InputData(e.JoystickId, e.Axis, e.Position)); };
			//context.JoystickButtonPressed += (s, e) => { Game.State.InputPressed(new InputData(e.JoystickId, e.Button)); };
			//context.JoystickButtonReleased += (s, e) => { Game.State.InputReleased(new InputData(e.JoystickId, e.Button)); };
		}

		

		public void LoadConfig(Config.ConfigInput inputConfig) {
			var fields = inputConfig.Keyboard.GetType().GetFields();
			foreach (var field in fields) {
				_actionsByKey[(Keyboard.Key)field.GetValue(inputConfig.Keyboard)] = Enum.Parse<Input.Action>(field.Name, true);
			}
		}

		private void _closed(object sender, EventArgs e) {
			Game.Window.Close();
		}

		private void _keyPressed(object sender, KeyEventArgs args) {
			Game.SceneManager.ActiveScene.InputPressed(new InputData() {
				Type = Input.Type.Key,
				Key = args.Code,
				Action = _getAction(args.Code)
			});
		}

		private void _keyReleased(object sender, KeyEventArgs args) {
			Game.SceneManager.ActiveScene.InputReleased(new InputData() {
				Type = Input.Type.Key,
				Key = args.Code,
				Action = _getAction(args.Code)
			});
		}

		private void _textEntered(object sender, TextEventArgs args) {
			if (!IsTyping) return;
			Game.SceneManager.ActiveScene.InputPressed(new InputData() {
				Type = Input.Type.Text,
				Unicode = args.Unicode,
			});
		}

		private Input.Action _getAction(Keyboard.Key key) {
			if (!_actionsByKey.ContainsKey(key)) return Input.Action.Unknown;
			return _actionsByKey[key];
		}

	}
}
