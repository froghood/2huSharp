using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Touhou.IO;

namespace Touhou.Scene {
	internal class SceneManager : IScene {

		public IScene ActiveScene { get; private set; }
		private Stack<IScene> _sceneStack = new();

		public void PushScene<S>(params object[] args) where S : IScene {
			Game.Command(() => {
				_sceneStack.Push((S)Activator.CreateInstance(typeof(S), args));
				ActiveScene = _sceneStack.Peek();
			});
		}

		public void PopScene() {
			Game.Command(() => {
				_sceneStack.Pop();
				ActiveScene = (_sceneStack.Count > 0) ? _sceneStack.Peek() : null;
			});
		}



		public void Update(float time, float delta) {
			ActiveScene.Update(time, delta);
		}

		public void Render(float time, float delta) {
			ActiveScene.Render(time, delta);
		}

		public void InputPressed(InputData inputData) {
			ActiveScene.InputPressed(inputData);
		}

		public void InputReleased(InputData inputData) {
			ActiveScene.InputReleased(inputData);
		}
	}
}
