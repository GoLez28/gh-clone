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
            textures = new Texture2D[0];
            type = 4;
        }
        public Texture2D anim {
            get {
                return textures[Game.animationFrame % textures.Length];
            }
        }
    }
}
