using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

using Touhou.IO;

namespace Touhou.UI {
	internal abstract class Menu : ISimulated, IInput {

		public Menu() {}

		public Menu(Menu parent, string id) {
			Parent = parent;
			Id = id;
		}

		public Menu Parent;
		public string Id;

		public Vector2f Position { get; set; }

		public Action<float, float> OnUpdate;
		public Action<float, float> OnRender;
		public Action<InputData> OnInputPressed;
		public Action<InputData> OnInputReleased;
		public Action OnHover;
		public Action OnHoverEnd;

		public Vector2f GetGlobalPosition() {
			if (Parent is null) return Position;
			else return Position + Parent.GetGlobalPosition();
		}

		public abstract void Update(float time, float delta);
		public abstract void Render(float time, float delta);
		public abstract void InputPressed(InputData inputData);
		public abstract void InputReleased(InputData inputData);	

		
		

	}
}
