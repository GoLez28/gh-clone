using System;
using System.IO;

namespace Upbeat {
    class Config : Store {
        private static string baseDirectory = "";
        private static string directory = "";
        public static string file = "config";
        public static string extension = ".ini";
        public static string path;
        private static Type self;

        public static void Init() {
            path = Path.Combine(baseDirectory, directory, file) + extension;
            self = typeof(Config);
            Store.Init(self);
        }

        // Video
        private static int _width = 0;
        public static int width { get { return _width; } set { _width = value; Store.Set("width", value, self); } }
        private static int _height = 0;
        public static int height { get { return _height; } set { _height = value; Store.Set("height", value, self); } }
        private static int _fSwidth = 0;
        public static int fSwidth { get { return _fSwidth; } set { _fSwidth = value; Store.Set("fSwidth", value, self); } }
        private static int _fSheight = 0;
        public static int fSheight { get { return _fSheight; } set { _fSheight = value; Store.Set("fSheight", value, self); } }
        private static bool _vSync = false;
        public static bool vSync { get { return _vSync; } set { _vSync = value; Store.Set("vSync", value, self); } }
        private static int _frameR = 120;
        public static int frameR { get { return _frameR; } set { _frameR = value; Store.Set("frameR", value, self); } }
        private static int _uptMult = 4;
        public static int uptMult { get { return _uptMult; } set { _uptMult = value; Store.Set("uptMult", value, self); } }
        private static bool _fS = true;
        public static bool fS { get { return _fS; } set { _fS = value; Store.Set("fS", value, self); } }
        private static int _master = 100;
        public static int master { get { return _master; } set { _master = value; Store.Set("master", value, self); } }
        private static int _os = 0;
        public static int os { get { return _os; } set { _os = value; Store.Set("os", value, self); } }
        private static bool _showFps = false;
        public static bool showFps { get { return _showFps; } set { _showFps = value; Store.Set("showFps", value, self); } }
        private static bool _spC = true;
        public static bool spC { get { return _spC; } set { _spC = value; Store.Set("spC", value, self); } }
        private static int _maniavol = 100;
        public static int maniavol { get { return _maniavol; } set { _maniavol = value; Store.Set("maniavol", value, self); } }
        private static int _musicvol = 100;
        public static int musicvol { get { return _musicvol; } set { _musicvol = value; Store.Set("musicvol", value, self); } }
        private static int _fxvol = 100;
        public static int fxvol { get { return _fxvol; } set { _fxvol = value; Store.Set("fxvol", value, self); } }
        private static bool _noteInfo = false;
        public static bool noteInfo { get { return _noteInfo; } set { _noteInfo = value; Store.Set("noteInfo", value, self); } }
        private static bool _badPC = false;
        public static bool badPC { get { return _badPC; } set { _badPC = value; Store.Set("badPC", value, self); } }
        private static bool _pitch = true;
        public static bool pitch { get { return _pitch; } set { _pitch = value; Store.Set("pitch", value, self); } }
        private static bool _fpitch = false;
        public static bool fpitch { get { return _fpitch; } set { _fpitch = value; Store.Set("fpitch", value, self); } }
        private static bool _wave = true;
        public static bool wave { get { return _wave; } set { _wave = value; Store.Set("wave", value, self); } }
        private static bool _spark = true;
        public static bool spark { get { return _spark; } set { _spark = value; Store.Set("spark", value, self); } }
        private static bool _failanim = true;
        public static bool failanim { get { return _failanim; } set { _failanim = value; Store.Set("failanim", value, self); } }
        private static bool _fsanim = true;
        public static bool fsanim { get { return _fsanim; } set { _fsanim = value; Store.Set("fsanim", value, self); } }
        private static bool _useghhw = false;
        public static bool useghhw { get { return _useghhw; } set { _useghhw = value; Store.Set("useghhw", value, self); } }
        private static bool _al = false;
        public static bool al { get { return _al; } set { _al = value; Store.Set("al", value, self); } }
        private static bool _singleThread = false;
        public static bool singleThread { get { return _singleThread; } set { _singleThread = value; Store.Set("singleThread", value, self); } }
        private static int _tailQuality = 2;
        public static int tailQuality { get { return _tailQuality; } set { _tailQuality = value; Store.Set("tailQuality", value, self); } }
        private static bool _bendPitch = false;
        public static bool bendPitch { get { return _bendPitch; } set { _bendPitch = value; Store.Set("bendPitch", value, self); } }
        private static int _volup = 97;
        public static int volup { get { return _volup; } set { _volup = value; Store.Set("volup", value, self); } }
        private static int _voldn = 94;
        public static int voldn { get { return _voldn; } set { _voldn = value; Store.Set("voldn", value, self); } }
        private static int _nexts = 91;
        public static int nexts { get { return _nexts; } set { _nexts = value; Store.Set("nexts", value, self); } }
        private static int _prevs = 107;
        public static int prevs { get { return _prevs; } set { _prevs = value; Store.Set("prevs", value, self); } }
        private static int _pause = 103;
        public static int pause { get { return _pause; } set { _pause = value; Store.Set("pause", value, self); } }
        private static bool _menuFx = true;
        public static bool menuFx { get { return _menuFx; } set { _menuFx = value; Store.Set("menuFx", value, self); } }
        private static string _lang = "en";
        public static string lang { get { return _lang; } set { _lang = value; Store.Set("lang", value, self); } }
        private static string _skin = "";
        public static string skin { get { return _skin; } set { _skin = value; Store.Set("skin", value, self); } }
        private static bool _instantChange = true;
        public static bool instantChange { get { return _instantChange; } set { _instantChange = value; Store.Set("instantChange", value, self); } }
        private static bool _showWindow = false;
        public static bool showWindow { get { return _showWindow; } set { _showWindow = value; Store.Set("hitwindow", value, self); } }
        private static bool _audioStabilization = true;
        public static bool audioStabilization { get { return _audioStabilization; } set { _audioStabilization = value; Store.Set("audioStabilization", value, self); } }
        private static bool _videoFlip = false;
        public static bool videoFlip { get { return _videoFlip; } set { _videoFlip = value; Store.Set("videoFlip", value, self); } }
        private static int _diffShown = 1;
        public static int diffShown { get { return _diffShown; } set { _diffShown = value; Store.Set("diffShown", value, self); } }
    }
}
