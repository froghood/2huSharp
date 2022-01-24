using SFML.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;

using Touhou.IO;

namespace Touhou.UI {
	internal class MenuSelector : Menu {

		public int Index { get => _index;
			set {
				GetMenuByIndex().OnHoverEnd?.Invoke();
				_index = (value + _menuList.Count) % _menuList.Count;
				GetMenuByIndex().OnHover?.Invoke();
			}
		}

		public MenuSelector() { }

		public MenuSelector(string id, Vector2f positon = new Vector2f()) : base(null, id) {
			Position = positon;
		}

		public MenuSelector(Menu parent, string id) : base(parent, id) {
			Position = new();
		}

		public MenuSelector(Menu parent, string id, Vector2f positon = new Vector2f()) : base(parent, id) {
			Position = positon;
		}


		public M AddMenu<M>(string id, params object[] args) where M : Menu {
			Type type = typeof(M);
			object[] newArgs = new object[] { this, id }.Concat(args).ToArray();
			M menu = (M)Activator.CreateInstance(type, newArgs);

			// create list for that type if it does not exist
			if (!_menuDict.ContainsKey(type)) _menuDict[type] = new();

			_menuDict[type].Add(menu);
			_menuList.Add(menu);
			return menu;
		}

		public List<Menu> GetMenus() { return _menuList; }
		public List<M> GetMenus<M>() where M : Menu {
			Type type = typeof(M);
			if (!_menuDict.ContainsKey(type)) return null;
			return _menuDict[type].Cast<M>().ToList();
		}


		public override void Update(float time, float delta) {
			OnUpdate?.Invoke(time, delta);
			foreach (Menu menu in _menuList) {
				menu.Update(time, delta);
			}
		}

		public override void Render(float time, float delta) {
			OnRender?.Invoke(time, delta);
			foreach (Menu menu in _menuList) {
				menu.Render(time, delta);
			}
		}

		public override void InputPressed(InputData inputData) {
			OnInputPressed?.Invoke(inputData);
			GetMenuByIndex().InputPressed(inputData);
		}

		public override void InputReleased(InputData inputData) {
			OnInputReleased?.Invoke(inputData);
			GetMenuByIndex().InputReleased(inputData);
		}

		//public void Select() { GetMenuByIndex().OnSelect?.Invoke(); }
		//public void Select(int index) { GetMenuByIndex(index).OnSelect?.Invoke(); }

		public Menu GetMenuByIndex() { return _menuList[_index]; }
		public Menu GetMenuByIndex(int index) { return _menuList[index]; }

		public T GetMenuByIndex<T>() where T : Menu { return _menuList[_index] as T; }
		public T GetMenuByIndex<T>(int index) where T : Menu { return _menuList[index] as T; }

		public bool GetMenuByIndex<T>(out T menu, int? index = null) where T : Menu {
			index ??= _index;
			menu = _menuList[index.Value] as T;
			return (menu == null);
		}



		private int _index = 0;
		private List<Menu> _menuList = new();
		private Dictionary<Type, List<Menu>> _menuDict = new();
	}
}
