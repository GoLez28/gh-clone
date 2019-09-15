using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GHtest1 {
    class PopUp {
        public string advice;
        public bool isWarning;
        public double life;
    }
    class Fire {
        public float x;
        public int up;
        public bool open;
        public double life;
        public bool active = false;
        public Fire(float x, int up, bool open) {
            this.x = x;
            this.up = up;
            this.open = open;
            life = 0;
        }
        public void Start() {
            life = 0;
            active = true;
        }
    }
    struct NoteGhost {
        public float id;
        public double start;
        public float delta;
    }
    class Spark {
        public Vector2 pos;
        public Vector2 vel;
        public Vector2 acc;
        public float z;
        public double start;
        public Spark(Vector2 pos, Vector2 vel, float z, double start) {
            acc = new Vector2(0, 0.01f);
            this.vel = vel;
            this.pos = pos;
            this.z = z;
            this.start = start;
        }
        public void Update() {
            vel = Vector2.Add(vel, acc * (float)game.timeEllapsed * 0.8f);
            pos = Vector2.Add(pos, vel * (float)game.timeEllapsed * 0.8f);
        }
    }
    struct SpSpark {
        public int animationStart;
        public float x;
    }
    struct SpLighting {
        public double startTime;
        public double rotation;
        public float x;
    }
    struct Points {
        public float x;
        public double startTime;
        public double limit;
        public int point;
    }
}
