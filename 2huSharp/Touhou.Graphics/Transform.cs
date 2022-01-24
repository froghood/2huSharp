using SFML.System;

namespace Touhou.Graphics {
    public class Transform {
        public Vector2f Translation = new(0f, 0f);
        public Vector2f Scale = new(1f, 1f);
        public float Rotation = 0f;
    }
}
