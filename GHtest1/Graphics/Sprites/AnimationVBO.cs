﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Sprites {
    class AnimationVBO : Sprite {
        public Texture2D[] textures;
        public int index;
        public AnimationVBO () {
            textures = new Texture2D[0];
            type = 3;
        }
    }
}