using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Sprites {
    class AnimationVertex : Sprite {
        public Texture2D[] textures;
        public Vector4 vertices;
        public AnimationVertex() {
            type = 4;
        }
    }
}
