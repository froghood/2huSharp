using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;
using SFML.Graphics;

using SharpDX.XInput;
using SFML.System;

namespace Touhou.IO {
	internal class InputHandler {

		public bool IsTyping = false;
		public Input.JoystickMode JoystickMode = Input.JoystickMode.Simple;

		private Dictionary<Keyboard.Key, Input.Action> _actionsByKey;
		private Dictionary<Input.Button, Input.Action> _actionsByButton;

		private Controller[] _controllers;
		private GamepadButtonFlags[] _prevButtons;
		private Input.JoystickDirection[] _prevDirections;

		private Queue<KeyEventArgs> _keyEventCache;
		private Queue<TextEventArgs> _textEventCache;

		const float JOYSTICK_MAX = 32767f;
		const float DEADZONE = 0.25f;

		public InputHandler() {
			_keyEventCache = new Queue<KeyEventArgs>();
			_textEventCache = new Queue<TextEventArgs>();

			_actionsByKey = new Dictionary<Keyboard.Key, Input.Action>();
			_actionsByButton = new Dictionary<Input.Button, Input.Action>();

			_controllers = new Controller[] { new Controller(UserIndex.One), new Controller(UserIndex.Two) };
			_prevButtons = new GamepadButtonFlags[_controllers.Length];
			_prevDirections = new Input.JoystickDirection[_controllers.Length];
		}

		public void Poll() {
			//_resetKeyPressedCache();
			Game.Window.DispatchEvents();
			_pollKeyboard();
			_pollControllers();
		}

		private void _pollKeyboard() {
			for(int i = Math.Max(_keyEventCache.Count, _textEventCache.Count); i > 0; i--) {
				var keyArgs = (_keyEventCache.Count > 0) ? _keyEventCache.Dequeue() : null;
				var textArgs = (_textEventCache.Count > 0) ? _textEventCache.Dequeue() : null;

				Game.SceneManager.ActiveScene.InputPressed(new InputData() {
					Type = (IsTyping) ? Input.Type.Text : Input.Type.Key,
					Key = keyArgs.Code,
					Action = _getAction(keyArgs.Code),
					Unicode = textArgs?.Unicode
				});
			}

			//if (_keyEventCache == null && _textEventCache == null) return;

			//Game.SceneManager.ActiveScene.InputPressed(new InputData() {
			//	Type = (IsTyping) ? Input.Type.Text : Input.Type.Key,
			//	Key = _keyEventCache.Code,
			//	Action = _getAction(_keyEventCache.Code),
			//	Unicode = _textEventCache?.Unicode
			//}) ;

			//_keyEventCache = null;
			//_textEventCache = null;

		}

		public void BindEvents(RenderWindow window) {
			window.Closed += _closed;		
			window.KeyPressed += _cacheKeyEvent;
			window.KeyReleased += _keyReleased;
			window.TextEntered += _cacheTextEvent;


			//window.KeyPressed += (_, e) => { Game.SceneManager.ActiveScene.InputPressed(new InputData(e.Code)); }; 
			//window.KeyReleased += (_, e) => { Game.SceneManager.ActiveScene.InputReleased(new InputData(e.Code)); };
			//window.TextEntered += (_, e) => { if (IsTyping) Game.SceneManager.ActiveScene.InputPressed(new InputData(e.Unicode)); };
			//context.JoystickMoved += (s, e) => { Game.State.InputPressed(new InputData(e.JoystickId, e.Axis, e.Position)); };
			//context.JoystickButtonPressed += (s, e) => { Game.State.InputPressed(new InputData(e.JoystickId, e.Button)); };
			//context.JoystickButtonReleased += (s, e) => { Game.State.InputReleased(new InputData(e.JoystickId, e.Button)); };
		}

		public void LoadConfig(Config.ConfigInput inputConfig) {
			var keyboardFields = inputConfig.Keyboard.GetType().GetFields();
			foreach (var field in keyboardFields) {
				_actionsByKey[(Keyboard.Key)field.GetValue(inputConfig.Keyboard)] = Enum.Parse<Input.Action>(field.Name, true);
			}

			var gamepadFields = inputConfig.Gamepad.GetType().GetFields();
			foreach (var field in gamepadFields) {
				_actionsByButton[(Input.Button)field.GetValue(inputConfig.Gamepad)] = Enum.Parse<Input.Action>(field.Name, true);
			}

			foreach (var t in _actionsByButton) {
				Console.WriteLine(t);
			}
		}

		public void BeginTyping() { Game.Command(() => { 
				IsTyping = true;
				Game.Window.SetKeyRepeatEnabled(true);
			}); 
		}

		public void EndTyping() {
			Game.Command(() => { 
				IsTyping = false;
				Game.Window.SetKeyRepeatEnabled(false);
			}); 
		}

		private void _pollControllers() {
			foreach (Controller controller in _controllers) {
				if (!controller.IsConnected) continue; // skip if not connected

				Gamepad state = controller.GetState().Gamepad;
				int user = (int)controller.UserIndex;

				// buttons
				var changedButtons = _prevButtons[user] ^ state.Buttons;

				foreach (GamepadButtonFlags buttonFlag in Enum.GetValues(typeof(GamepadButtonFlags))) {
					if (buttonFlag == GamepadButtonFlags.None) continue; // ignore None
					if (!changedButtons.HasFlag(buttonFlag)) continue; // skip unchanged flags

					var button = _buttonsByFlag[buttonFlag];
					var inputData = new InputData {
						Type = Input.Type.Button,
						User = user,
						Button = button,
						Action = _getAction(button)
					};

					if (state.Buttons.HasFlag(buttonFlag)) {
						Game.SceneManager.ActiveScene.InputPressed(inputData); // pressed
					} else {
						Game.SceneManager.ActiveScene.InputReleased(inputData); // released
					}
				};
			
				_prevButtons[user] = state.Buttons;

				// joystick
				Input.JoystickDirection direction = _getDirection(state);
				ref Input.JoystickDirection prevJoystickDirection = ref _prevDirections[user];
				if (direction != _prevDirections[user]) {
					Game.SceneManager.ActiveScene.InputPressed(new InputData() {
						Type = Input.Type.Joystick,
						User = user,
						Direction = direction,
						Vector = _directionVectors[direction]
					});

					_prevDirections[user] = direction;
				}
			}
		}

		private Input.JoystickDirection _getDirection(Gamepad state) {
			switch (JoystickMode) {
				case Input.JoystickMode.Accurate:
					float joystickMagnitude = MathF.Min(JOYSTICK_MAX, MathF.Sqrt(
						state.LeftThumbX * state.LeftThumbX +
						state.LeftThumbY * state.LeftThumbY));
					if (joystickMagnitude >= JOYSTICK_MAX * DEADZONE) {
						float joystickAngle = (MathF.Atan2(state.LeftThumbY, state.LeftThumbX) + MathF.Tau) % MathF.Tau;
						return (Input.JoystickDirection)(MathF.Round(joystickAngle * 8f / MathF.Tau) % 8f + 1f);
					}
					break;

				case Input.JoystickMode.Simple:
					float xDir = MathF.Round(state.LeftThumbX / JOYSTICK_MAX);
					float yDir = MathF.Round(state.LeftThumbY / JOYSTICK_MAX);
					if (xDir != 0 || yDir != 0) {
						float joystickAngle = (MathF.Atan2(yDir, xDir) + MathF.Tau) % MathF.Tau;
						return (Input.JoystickDirection)(MathF.Round(joystickAngle * 8f / MathF.Tau) % 8f + 1f);
					}
					break;
			}
			return Input.JoystickDirection.Center;
		}

		private void _closed(object sender, EventArgs e) {
			Game.Close();
		}

		private void _cacheKeyEvent(object sender, KeyEventArgs args) {
			_keyEventCache.Enqueue(args);
		}

		private void _cacheTextEvent(object sender, TextEventArgs args) {
			_textEventCache.Enqueue(args);

			//Game.SceneManager.ActiveScene.InputPressed(new InputData() {
			//	Type = (IsTyping) ? Input.Type.Text : Input.Type.Key,
			//	Key = _keyEventArgs.Code,
			//	Action = _getAction(_keyEventArgs.Code),
			//	Unicode = args.Unicode
			//});
		}

		private void _keyReleased(object sender, KeyEventArgs args) {
			//Console.WriteLine("keyReleased");
			Game.SceneManager.ActiveScene.InputReleased(new InputData() {
				Type = (IsTyping) ? Input.Type.Text : Input.Type.Key,
				Key = args.Code,
				Action = _getAction(args.Code)
			});
		}

		

		private Input.Action _getAction(Keyboard.Key key) {
			if (!_actionsByKey.ContainsKey(key)) return Input.Action.Unknown;
			return _actionsByKey[key];
		}

		private Input.Action _getAction(Input.Button button) {
			if (!_actionsByButton.ContainsKey(button)) return Input.Action.Unknown;
			return _actionsByButton[button];
		}

		private void _resetKeyPressedCache() {
			_keyEventCache = null;
			_textEventCache = null;
		}


		// maps
		private Dictionary<GamepadButtonFlags, Input.Button> _buttonsByFlag = new() {
			{ GamepadButtonFlags.A,				Input.Button.A		   },
			{ GamepadButtonFlags.B,				Input.Button.B		   },
			{ GamepadButtonFlags.X,				Input.Button.X		   },
			{ GamepadButtonFlags.Y,				Input.Button.Y		   },
			{ GamepadButtonFlags.LeftShoulder,  Input.Button.LShoulder },
			{ GamepadButtonFlags.RightShoulder, Input.Button.RShoulder },
			{ GamepadButtonFlags.Back,			Input.Button.Select	   },
			{ GamepadButtonFlags.Start,			Input.Button.Start	   },
			{ GamepadButtonFlags.LeftThumb,		Input.Button.LThumb    },
			{ GamepadButtonFlags.RightThumb,	Input.Button.RThumb    },
			{ GamepadButtonFlags.DPadRight,		Input.Button.Right     },
			{ GamepadButtonFlags.DPadLeft,		Input.Button.Left	   },
			{ GamepadButtonFlags.DPadDown,		Input.Button.Down	   },
			{ GamepadButtonFlags.DPadUp,		Input.Button.Up		   }
		};

		private Dictionary<Input.JoystickDirection, Vector2f> _directionVectors = new() {
			{ Input.JoystickDirection.Center,	 new( 0f,  0f) },
			{ Input.JoystickDirection.East,		 new( 1f,  0f) },
			{ Input.JoystickDirection.NorthEast, new( 1f, -1f) },
			{ Input.JoystickDirection.North,	 new( 0f, -1f) },
			{ Input.JoystickDirection.NorthWest, new(-1f, -1f) },
			{ Input.JoystickDirection.West,		 new(-1f,  0f) },
			{ Input.JoystickDirection.SouthWest, new(-1f,  1f) },
			{ Input.JoystickDirection.South,	 new( 0f,  1f) },
			{ Input.JoystickDirection.SouthEast, new( 1f,  1f) }
		};
	}
}
