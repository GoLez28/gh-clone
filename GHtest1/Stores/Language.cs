using System;
using System.IO;

namespace Upbeat
{
    class Language : Store
    {
        private static string baseDirectory = "Content";
        private static string directory = "Languages";
        public static string extension = ".ini";
        public static string path;
        private static Type self;

        // The current game language file instead of "file" variable
        public static string language = "en";

        public static void Init()
        {
            path = Path.Combine(baseDirectory, directory, language) + extension;
            self = typeof(Language);
            Store.Init(self);
        }

        public static void LoadLanguage()
        {
            if (File.Exists(Path.Combine(baseDirectory, directory, language) + extension))
            {
                Init();
            }
        }


        private static string _menuPlay = "Play";
        public static string menuPlay { get { return _menuPlay; } set { _menuPlay = value; Store.Set("menuPlay", value, self); } }

        private static string _menuEditor = "Editor";
        public static string menuEditor { get { return _menuEditor; } set { _menuEditor = value; Store.Set("menuEditor", value, self); } }

        private static string _menuOptions = "Options";
        public static string menuOptions { get { return _menuOptions; } set { _menuOptions = value; Store.Set("menuOptions", value, self); } }

        private static string _menuExit = "Exit";
        public static string menuExit { get { return _menuExit; } set { _menuExit = value; Store.Set("menuExit", value, self); } }

        private static string _menuLocalPlay = "Local Play";
        public static string menuLocalPlay { get { return _menuLocalPlay; } set { _menuLocalPlay = value; Store.Set("menuLocalPlay", value, self); } }

        private static string _menuOnlinePlay = "Online Play";
        public static string menuOnlinePlay { get { return _menuOnlinePlay; } set { _menuOnlinePlay = value; Store.Set("menuOnlinePlay", value, self); } }

        private static string _menuPressBtn = "Press Button";
        public static string menuPressBtn { get { return _menuPressBtn; } set { _menuPressBtn = value; Store.Set("menuPressBtn", value, self); } }

        private static string _menuKeyboard = "Keyboard";
        public static string menuKeyboard { get { return _menuKeyboard; } set { _menuKeyboard = value; Store.Set("menuKeyboard", value, self); } }

        private static string _menuController = "Controller";
        public static string menuController { get { return _menuController; } set { _menuController = value; Store.Set("menuController", value, self); } }

        private static string _menuPlaying = "Now Playing";
        public static string menuPlaying { get { return _menuPlaying; } set { _menuPlaying = value; Store.Set("menuPlaying", value, self); } }

        private static string _menuBlueTo = "(Blue To Change)";
        public static string menuBlueTo { get { return _menuBlueTo; } set { _menuBlueTo = value; Store.Set("menuBlueTo", value, self); } }

        private static string _menuScan = "Scanning";
        public static string menuScan { get { return _menuScan; } set { _menuScan = value; Store.Set("menuScan", value, self); } }

        private static string _menuCalcDiff = "Calculating Difficulties";
        public static string menuCalcDiff { get { return _menuCalcDiff; } set { _menuCalcDiff = value; Store.Set("menuCalcDiff", value, self); } }

        private static string _menuCache = "Caching";
        public static string menuCache { get { return _menuCache; } set { _menuCache = value; Store.Set("menuCache", value, self); } }

        private static string _menuPlayerHelp = "Vol+ {0}  Vol- {1}     Prev {2}  Pause {3}  Next {4}";
        public static string menuPlayerHelp { get { return _menuPlayerHelp; } set { _menuPlayerHelp = value; Store.Set("menuPlayerHelp", value, self); } }

        private static string _menuModPlayer = "Player {0}";
        public static string menuModPlayer { get { return _menuModPlayer; } set { _menuModPlayer = value; Store.Set("menuModPlayer", value, self); } }

        private static string _menuModMods = "Mods";
        public static string menuModMods { get { return _menuModMods; } set { _menuModMods = value; Store.Set("menuModMods", value, self); } }

        private static string _menuModOptions = "Options";
        public static string menuModOptions { get { return _menuModOptions; } set { _menuModOptions = value; Store.Set("menuModOptions", value, self); } }

        private static string _menuModHard = "Hard";
        public static string menuModHard { get { return _menuModHard; } set { _menuModHard = value; Store.Set("menuModHard", value, self); } }

        private static string _menuModEasy = "Easy";
        public static string menuModEasy { get { return _menuModEasy; } set { _menuModEasy = value; Store.Set("menuModEasy", value, self); } }

        private static string _menuModSpeed = "Speed";
        public static string menuModSpeed { get { return _menuModSpeed; } set { _menuModSpeed = value; Store.Set("menuModSpeed", value, self); } }

        private static string _menuModHidden = "Hidden";
        public static string menuModHidden { get { return _menuModHidden; } set { _menuModHidden = value; Store.Set("menuModHidden", value, self); } }

        private static string _menuModAuto = "Auto";
        public static string menuModAuto { get { return _menuModAuto; } set { _menuModAuto = value; Store.Set("menuModAuto", value, self); } }

        private static string _menuModNotes = "Notes: {0}";
        public static string menuModNotes { get { return _menuModNotes; } set { _menuModNotes = value; Store.Set("menuModNotes", value, self); } }

        private static string _menuModInput = "Input: {0}";
        public static string menuModInput { get { return _menuModInput; } set { _menuModInput = value; Store.Set("menuModInput", value, self); } }

        private static string _menuModInNormal = "Normal";
        public static string menuModInNormal { get { return _menuModInNormal; } set { _menuModInNormal = value; Store.Set("menuModInNormal", value, self); } }

        private static string _menuModInAllstrum = "All Strum";
        public static string menuModInAllstrum { get { return _menuModInAllstrum; } set { _menuModInAllstrum = value; Store.Set("menuModInAllstrum", value, self); } }

        private static string _menuModInAlltap = "All Tap";
        public static string menuModInAlltap { get { return _menuModInAlltap; } set { _menuModInAlltap = value; Store.Set("menuModInAlltap", value, self); } }

        private static string _menuModInStrum = "Strum Only";
        public static string menuModInStrum { get { return _menuModInStrum; } set { _menuModInStrum = value; Store.Set("menuModInStrum", value, self); } }

        private static string _menuModInFretless = "Fretless";
        public static string menuModInFretless { get { return _menuModInFretless; } set { _menuModInFretless = value; Store.Set("menuModInFretless", value, self); } }

        private static string _menuModNotesNormal = "Normal";
        public static string menuModNotesNormal { get { return _menuModNotesNormal; } set { _menuModNotesNormal = value; Store.Set("menuModNotesNormal", value, self); } }

        private static string _menuModNotesFlip = "Flip";
        public static string menuModNotesFlip { get { return _menuModNotesFlip; } set { _menuModNotesFlip = value; Store.Set("menuModNotesFlip", value, self); } }

        private static string _menuModNotesShuffle = "Shuffle";
        public static string menuModNotesShuffle { get { return _menuModNotesShuffle; } set { _menuModNotesShuffle = value; Store.Set("menuModNotesShuffle", value, self); } }

        private static string _menuModNotesRandom = "Total Random";
        public static string menuModNotesRandom { get { return _menuModNotesRandom; } set { _menuModNotesRandom = value; Store.Set("menuModNotesRandom", value, self); } }

        private static string _menuModNofail = "No Fail";
        public static string menuModNofail { get { return _menuModNofail; } set { _menuModNofail = value; Store.Set("menuModNofail", value, self); } }

        private static string _menuModPerformance = "Performance Mode";
        public static string menuModPerformance { get { return _menuModPerformance; } set { _menuModPerformance = value; Store.Set("menuModPerformance", value, self); } }

        private static string _menuModTransform = "Transform";
        public static string menuModTransform { get { return _menuModTransform; } set { _menuModTransform = value; Store.Set("menuModTransform", value, self); } }

        private static string _menuModAutoSP = "Auto SP";
        public static string menuModAutoSP { get { return _menuModAutoSP; } set { _menuModAutoSP = value; Store.Set("menuModAutoSP", value, self); } }

        private static string _menuModQuit = "Quit Profile";
        public static string menuModQuit { get { return _menuModQuit; } set { _menuModQuit = value; Store.Set("menuModQuit", value, self); } }

        private static string _menuOptionMode = "Game Mode: {0}";
        public static string menuOptionMode { get { return _menuOptionMode; } set { _menuOptionMode = value; Store.Set("menuOptionMode", value, self); } }

        private static string _menuProfileCreateIn = "Create Profile";
        public static string menuProfileCreateIn { get { return _menuProfileCreateIn; } set { _menuProfileCreateIn = value; Store.Set("menuProfileCreateIn", value, self); } }

        private static string _menuProfileCreate = "Create Profile";
        public static string menuProfileCreate { get { return _menuProfileCreate; } set { _menuProfileCreate = value; Store.Set("menuProfileCreate", value, self); } }

        private static string _menuProfileCancel = "Escape to Cancel";
        public static string menuProfileCancel { get { return _menuProfileCancel; } set { _menuProfileCancel = value; Store.Set("menuProfileCancel", value, self); } }

        private static string _menuProfileAccept = "Enter to Accept";
        public static string menuProfileAccept { get { return _menuProfileAccept; } set { _menuProfileAccept = value; Store.Set("menuProfileAccept", value, self); } }

        private static string _menuBtnsMain = "  {0} Select   {1} Next Song";
        public static string menuBtnsMain { get { return _menuBtnsMain; } set { _menuBtnsMain = value; Store.Set("menuBtnsMain", value, self); } }

        private static string _menuBtnsSong = "  {0} Select   {1} Back   {2} Search   {3} Random   {4} Change sort";
        public static string menuBtnsSong { get { return _menuBtnsSong; } set { _menuBtnsSong = value; Store.Set("menuBtnsSong", value, self); } }

        private static string _menuBtnsOptions = "  {0} Select   {1} Back";
        public static string menuBtnsOptions { get { return _menuBtnsOptions; } set { _menuBtnsOptions = value; Store.Set("menuBtnsOptions", value, self); } }

        private static string _menuBtnsDiff = "  {0} Select   {1} Back   {2} Records";
        public static string menuBtnsDiff { get { return _menuBtnsDiff; } set { _menuBtnsDiff = value; Store.Set("menuBtnsDiff", value, self); } }

        private static string _menuBtnsRecords = "  {0} Select   {1} Back   {2} Difficulties";
        public static string menuBtnsRecords { get { return _menuBtnsRecords; } set { _menuBtnsRecords = value; Store.Set("menuBtnsRecords", value, self); } }

        private static string _menuVolume = "Master";
        public static string menuVolume { get { return _menuVolume; } set { _menuVolume = value; Store.Set("menuVolume", value, self); } }

        private static string _gameScore = "Score: {0}";
        public static string gameScore { get { return _gameScore; } set { _gameScore = value; Store.Set("gameScore", value, self); } }

        private static string _gamePause = "PAUSE";
        public static string gamePause { get { return _gamePause; } set { _gamePause = value; Store.Set("gamePause", value, self); } }

        private static string _gamePausePlayer = "Player {0}";
        public static string gamePausePlayer { get { return _gamePausePlayer; } set { _gamePausePlayer = value; Store.Set("gamePausePlayer", value, self); } }

        private static string _gamePauseResume = "Resume";
        public static string gamePauseResume { get { return _gamePauseResume; } set { _gamePauseResume = value; Store.Set("gamePauseResume", value, self); } }

        private static string _gamePauseOptions = "Options";
        public static string gamePauseOptions { get { return _gamePauseOptions; } set { _gamePauseOptions = value; Store.Set("gamePauseOptions", value, self); } }

        private static string _gamePauseRestart = "Restart";
        public static string gamePauseRestart { get { return _gamePauseRestart; } set { _gamePauseRestart = value; Store.Set("gamePauseRestart", value, self); } }

        private static string _gamePauseExit = "Quit";
        public static string gamePauseExit { get { return _gamePauseExit; } set { _gamePauseExit = value; Store.Set("gamePauseExit", value, self); } }

        private static string _gameFail = "Song Failed";
        public static string gameFail { get { return _gameFail; } set { _gameFail = value; Store.Set("gameFail", value, self); } }

        private static string _gameFailRestart = "Retry";
        public static string gameFailRestart { get { return _gameFailRestart; } set { _gameFailRestart = value; Store.Set("gameFailRestart", value, self); } }

        private static string _gameFailExit = "Quit";
        public static string gameFailExit { get { return _gameFailExit; } set { _gameFailExit = value; Store.Set("gameFailExit", value, self); } }

        private static string _gameFailSave = "Save Play";
        public static string gameFailSave { get { return _gameFailSave; } set { _gameFailSave = value; Store.Set("gameFailSave", value, self); } }

        private static string _gameFPS = " FPS";
        public static string gameFPS { get { return _gameFPS; } set { _gameFPS = value; Store.Set("gameFPS", value, self); } }

        private static string _songSortBy = "Sorting by: ";
        public static string songSortBy { get { return _songSortBy; } set { _songSortBy = value; Store.Set("songSortBy", value, self); } }

        private static string _songSortName = "Name";
        public static string songSortName { get { return _songSortName; } set { _songSortName = value; Store.Set("songSortName", value, self); } }

        private static string _songSortArtist = "Artist";
        public static string songSortArtist { get { return _songSortArtist; } set { _songSortArtist = value; Store.Set("songSortArtist", value, self); } }

        private static string _songSortAlbum = "Album";
        public static string songSortAlbum { get { return _songSortAlbum; } set { _songSortAlbum = value; Store.Set("songSortAlbum", value, self); } }

        private static string _songSortYear = "Year";
        public static string songSortYear { get { return _songSortYear; } set { _songSortYear = value; Store.Set("songSortYear", value, self); } }

        private static string _songSortLength = "Length";
        public static string songSortLength { get { return _songSortLength; } set { _songSortLength = value; Store.Set("songSortLength", value, self); } }

        private static string _songSortPath = "Path";
        public static string songSortPath { get { return _songSortPath; } set { _songSortPath = value; Store.Set("songSortPath", value, self); } }

        private static string _songSortGenre = "Genre";
        public static string songSortGenre { get { return _songSortGenre; } set { _songSortGenre = value; Store.Set("songSortGenre", value, self); } }

        private static string _songSortCharter = "Charter";
        public static string songSortCharter { get { return _songSortCharter; } set { _songSortCharter = value; Store.Set("songSortCharter", value, self); } }

        private static string _songSortDiff = "Difficulty";
        public static string songSortDiff { get { return _songSortDiff; } set { _songSortDiff = value; Store.Set("songSortDiff", value, self); } }

        private static string _songSortByInstrument = "Instrument sort:";
        public static string songSortByInstrument { get { return _songSortByInstrument; } set { _songSortByInstrument = value; Store.Set("songSortByInstrument", value, self); } }

        private static string _songSortInsOn = "On";
        public static string songSortInsOn { get { return _songSortInsOn; } set { _songSortInsOn = value; Store.Set("songSortInsOn", value, self); } }

        private static string _songSortInsOff = "Off";
        public static string songSortInsOff { get { return _songSortInsOff; } set { _songSortInsOff = value; Store.Set("songSortInsOff", value, self); } }

        private static string _songCount = "Songs: ";
        public static string songCount { get { return _songCount; } set { _songCount = value; Store.Set("songCount", value, self); } }

        private static string _songDifficultyList = "Difficulty: ";
        public static string songDifficultyList { get { return _songDifficultyList; } set { _songDifficultyList = value; Store.Set("songDifficultyList", value, self); } }

        private static string _songDifficultyEasy = "Easy";
        public static string songDifficultyEasy { get { return _songDifficultyEasy; } set { _songDifficultyEasy = value; Store.Set("songDifficultyEasy", value, self); } }

        private static string _songDifficultyMedium = "Medium";
        public static string songDifficultyMedium { get { return _songDifficultyMedium; } set { _songDifficultyMedium = value; Store.Set("songDifficultyMedium", value, self); } }

        private static string _songDifficultyHard = "Hard";
        public static string songDifficultyHard { get { return _songDifficultyHard; } set { _songDifficultyHard = value; Store.Set("songDifficultyHard", value, self); } }

        private static string _songDifficultyExpert = "Expert";
        public static string songDifficultyExpert { get { return _songDifficultyExpert; } set { _songDifficultyExpert = value; Store.Set("songDifficultyExpert", value, self); } }

        private static string _songInstrumentGuitar = "Guitar";
        public static string songInstrumentGuitar { get { return _songInstrumentGuitar; } set { _songInstrumentGuitar = value; Store.Set("songInstrumentGuitar", value, self); } }

        private static string _songInstrumentGuitar2 = "Guitar 2";
        public static string songInstrumentGuitar2 { get { return _songInstrumentGuitar2; } set { _songInstrumentGuitar2 = value; Store.Set("songInstrumentGuitar2", value, self); } }

        private static string _songInstrumentBass = "Bass";
        public static string songInstrumentBass { get { return _songInstrumentBass; } set { _songInstrumentBass = value; Store.Set("songInstrumentBass", value, self); } }

        private static string _songInstrumentBass2 = "Bass 2";
        public static string songInstrumentBass2 { get { return _songInstrumentBass2; } set { _songInstrumentBass2 = value; Store.Set("songInstrumentBass2", value, self); } }

        private static string _songInstrumentDrums = "Drums";
        public static string songInstrumentDrums { get { return _songInstrumentDrums; } set { _songInstrumentDrums = value; Store.Set("songInstrumentDrums", value, self); } }

        private static string _songInstrumentKeys = "Keys";
        public static string songInstrumentKeys { get { return _songInstrumentKeys; } set { _songInstrumentKeys = value; Store.Set("songInstrumentKeys", value, self); } }

        private static string _songInstrumentVocals = "Vocals";
        public static string songInstrumentVocals { get { return _songInstrumentVocals; } set { _songInstrumentVocals = value; Store.Set("songInstrumentVocals", value, self); } }

        private static string _songInstrumentRhythm = "Rhythm";
        public static string songInstrumentRhythm { get { return _songInstrumentRhythm; } set { _songInstrumentRhythm = value; Store.Set("songInstrumentRhythm", value, self); } }

        private static string _songInstrumentRhythm2 = "Rhythm 2";
        public static string songInstrumentRhythm2 { get { return _songInstrumentRhythm2; } set { _songInstrumentRhythm2 = value; Store.Set("songInstrumentRhythm2", value, self); } }

        private static string _songInstrumentGuitarghl = "GHL Guitar";
        public static string songInstrumentGuitarghl { get { return _songInstrumentGuitarghl; } set { _songInstrumentGuitarghl = value; Store.Set("songInstrumentGuitarghl", value, self); } }

        private static string _songInstrumentBassghl = "GHL Bass";
        public static string songInstrumentBassghl { get { return _songInstrumentBassghl; } set { _songInstrumentBassghl = value; Store.Set("songInstrumentBassghl", value, self); } }

        private static string _songInstrumentProguitar = "Pro Guitar";
        public static string songInstrumentProguitar { get { return _songInstrumentProguitar; } set { _songInstrumentProguitar = value; Store.Set("songInstrumentProguitar", value, self); } }

        private static string _songInstrumentProbass = "Pro Bass";
        public static string songInstrumentProbass { get { return _songInstrumentProbass; } set { _songInstrumentProbass = value; Store.Set("songInstrumentProbass", value, self); } }

        private static string _songRecordsList = "Records (Blue)";
        public static string songRecordsList { get { return _songRecordsList; } set { _songRecordsList = value; Store.Set("songRecordsList", value, self); } }

        private static string _songRecordsNorecords = "No Records";
        public static string songRecordsNorecords { get { return _songRecordsNorecords; } set { _songRecordsNorecords = value; Store.Set("songRecordsNorecords", value, self); } }

        private static string _songRecordsSong = "for this song";
        public static string songRecordsSong { get { return _songRecordsSong; } set { _songRecordsSong = value; Store.Set("songRecordsSong", value, self); } }

        private static string _songRecordsLoading = "Loading Records";
        public static string songRecordsLoading { get { return _songRecordsLoading; } set { _songRecordsLoading = value; Store.Set("songRecordsLoading", value, self); } }

        private static string _optionsButtonGreen = "Green";
        public static string optionsButtonGreen { get { return _optionsButtonGreen; } set { _optionsButtonGreen = value; Store.Set("optionsButtonGreen", value, self); } }

        private static string _optionsButtonRed = "Red";
        public static string optionsButtonRed { get { return _optionsButtonRed; } set { _optionsButtonRed = value; Store.Set("optionsButtonRed", value, self); } }

        private static string _optionsButtonYellow = "Yellow";
        public static string optionsButtonYellow { get { return _optionsButtonYellow; } set { _optionsButtonYellow = value; Store.Set("optionsButtonYellow", value, self); } }

        private static string _optionsButtonBlue = "Blue";
        public static string optionsButtonBlue { get { return _optionsButtonBlue; } set { _optionsButtonBlue = value; Store.Set("optionsButtonBlue", value, self); } }

        private static string _optionsButtonOrange = "Orange";
        public static string optionsButtonOrange { get { return _optionsButtonOrange; } set { _optionsButtonOrange = value; Store.Set("optionsButtonOrange", value, self); } }

        private static string _optionsButtonOpen = "Open";
        public static string optionsButtonOpen { get { return _optionsButtonOpen; } set { _optionsButtonOpen = value; Store.Set("optionsButtonOpen", value, self); } }

        private static string _optionsButtonSix = "Six";
        public static string optionsButtonSix { get { return _optionsButtonSix; } set { _optionsButtonSix = value; Store.Set("optionsButtonSix", value, self); } }

        private static string _optionsButtonStart = "Start";
        public static string optionsButtonStart { get { return _optionsButtonStart; } set { _optionsButtonStart = value; Store.Set("optionsButtonStart", value, self); } }

        private static string _optionsButtonSp = "Star Power";
        public static string optionsButtonSp { get { return _optionsButtonSp; } set { _optionsButtonSp = value; Store.Set("optionsButtonSp", value, self); } }

        private static string _optionsButtonUp = "Up";
        public static string optionsButtonUp { get { return _optionsButtonUp; } set { _optionsButtonUp = value; Store.Set("optionsButtonUp", value, self); } }

        private static string _optionsButtonDown = "Down";
        public static string optionsButtonDown { get { return _optionsButtonDown; } set { _optionsButtonDown = value; Store.Set("optionsButtonDown", value, self); } }

        private static string _optionsButtonWhammy = "Whammy";
        public static string optionsButtonWhammy { get { return _optionsButtonWhammy; } set { _optionsButtonWhammy = value; Store.Set("optionsButtonWhammy", value, self); } }

        private static string _optionsButtonAxis = "Axis";
        public static string optionsButtonAxis { get { return _optionsButtonAxis; } set { _optionsButtonAxis = value; Store.Set("optionsButtonAxis", value, self); } }

        private static string _optionsButtonLefty = "Lefty";
        public static string optionsButtonLefty { get { return _optionsButtonLefty; } set { _optionsButtonLefty = value; Store.Set("optionsButtonLefty", value, self); } }

        private static string _optionsButtonGamepadMode = "Gamepad";
        public static string optionsButtonGamepadMode { get { return _optionsButtonGamepadMode; } set { _optionsButtonGamepadMode = value; Store.Set("optionsButtonGamepadMode", value, self); } }

        private static string _optionsButtonKeyboard = "Keyboard";
        public static string optionsButtonKeyboard { get { return _optionsButtonKeyboard; } set { _optionsButtonKeyboard = value; Store.Set("optionsButtonKeyboard", value, self); } }

        private static string _optionsButtonGamepad = "Gamepad";
        public static string optionsButtonGamepad { get { return _optionsButtonGamepad; } set { _optionsButtonGamepad = value; Store.Set("optionsButtonGamepad", value, self); } }

        private static string _optionsButtonDz = "DZ";
        public static string optionsButtonDz { get { return _optionsButtonDz; } set { _optionsButtonDz = value; Store.Set("optionsButtonDz", value, self); } }

        private static string _optionsButtonPlayer = "Player {0}";
        public static string optionsButtonPlayer { get { return _optionsButtonPlayer; } set { _optionsButtonPlayer = value; Store.Set("optionsButtonPlayer", value, self); } }

        private static string _optionsButtonInstrument = "Instrument: ";
        public static string optionsButtonInstrument { get { return _optionsButtonInstrument; } set { _optionsButtonInstrument = value; Store.Set("optionsButtonInstrument", value, self); } }

        private static string _optionsButton5fret = "5 Fret";
        public static string optionsButton5fret { get { return _optionsButton5fret; } set { _optionsButton5fret = value; Store.Set("optionsButton5fret", value, self); } }

        private static string _optionsButton6fret = "6 Fret";
        public static string optionsButton6fret { get { return _optionsButton6fret; } set { _optionsButton6fret = value; Store.Set("optionsButton6fret", value, self); } }

        private static string _optionsButtonDrums = "Drums";
        public static string optionsButtonDrums { get { return _optionsButtonDrums; } set { _optionsButtonDrums = value; Store.Set("optionsButtonDrums", value, self); } }

        private static string _optionsVideo = "Video";
        public static string optionsVideo { get { return _optionsVideo; } set { _optionsVideo = value; Store.Set("optionsVideo", value, self); } }

        private static string _optionsAudio = "Audio";
        public static string optionsAudio { get { return _optionsAudio; } set { _optionsAudio = value; Store.Set("optionsAudio", value, self); } }

        private static string _optionsKeys = "Keys";
        public static string optionsKeys { get { return _optionsKeys; } set { _optionsKeys = value; Store.Set("optionsKeys", value, self); } }

        private static string _optionsGameplay = "Game";
        public static string optionsGameplay { get { return _optionsGameplay; } set { _optionsGameplay = value; Store.Set("optionsGameplay", value, self); } }

        private static string _optionsSkin = "Skin";
        public static string optionsSkin { get { return _optionsSkin; } set { _optionsSkin = value; Store.Set("optionsSkin", value, self); } }

        private static string _optionsController = "Controller";
        public static string optionsController { get { return _optionsController; } set { _optionsController = value; Store.Set("optionsController", value, self); } }

        private static string _optionsVideoUnlimited = "Unlimited";
        public static string optionsVideoUnlimited { get { return _optionsVideoUnlimited; } set { _optionsVideoUnlimited = value; Store.Set("optionsVideoUnlimited", value, self); } }

        private static string _optionsVideoFullscreen = " Fullscreen";
        public static string optionsVideoFullscreen { get { return _optionsVideoFullscreen; } set { _optionsVideoFullscreen = value; Store.Set("optionsVideoFullscreen", value, self); } }

        private static string _optionsVideoVsync = " VSync";
        public static string optionsVideoVsync { get { return _optionsVideoVsync; } set { _optionsVideoVsync = value; Store.Set("optionsVideoVsync", value, self); } }

        private static string _optionsVideoFps = "Framerate: ";
        public static string optionsVideoFps { get { return _optionsVideoFps; } set { _optionsVideoFps = value; Store.Set("optionsVideoFps", value, self); } }

        private static string _optionsVideoResolution = "Resolution: ";
        public static string optionsVideoResolution { get { return _optionsVideoResolution; } set { _optionsVideoResolution = value; Store.Set("optionsVideoResolution", value, self); } }

        private static string _optionsVideoShowfps = " Show FPS";
        public static string optionsVideoShowfps { get { return _optionsVideoShowfps; } set { _optionsVideoShowfps = value; Store.Set("optionsVideoShowfps", value, self); } }

        private static string _optionsVideoExtreme = " Extreme Performance";
        public static string optionsVideoExtreme { get { return _optionsVideoExtreme; } set { _optionsVideoExtreme = value; Store.Set("optionsVideoExtreme", value, self); } }

        private static string _optionsVideoTailQuality = "Tail Quality: {0}";
        public static string optionsVideoTailQuality { get { return _optionsVideoTailQuality; } set { _optionsVideoTailQuality = value; Store.Set("optionsVideoTailQuality", value, self); } }

        private static string _optionsVideoThreadWarning = "Is recommended to enable Single Thread if you have one core CPU";
        public static string optionsVideoThreadWarning { get { return _optionsVideoThreadWarning; } set { _optionsVideoThreadWarning = value; Store.Set("optionsVideoThreadWarning", value, self); } }

        private static string _optionsVideoSingleThread = " Single Thread";
        public static string optionsVideoSingleThread { get { return _optionsVideoSingleThread; } set { _optionsVideoSingleThread = value; Store.Set("optionsVideoSingleThread", value, self); } }

        private static string _optionsVideoDrawMenuFx = " Draw menu effects";
        public static string optionsVideoDrawMenuFx { get { return _optionsVideoDrawMenuFx; } set { _optionsVideoDrawMenuFx = value; Store.Set("optionsVideoDrawMenuFx", value, self); } }

        private static string _optionsAudioMaster = "Master Volume: ";
        public static string optionsAudioMaster { get { return _optionsAudioMaster; } set { _optionsAudioMaster = value; Store.Set("optionsAudioMaster", value, self); } }

        private static string _optionsAudioOffset = "Audio Offset: ";
        public static string optionsAudioOffset { get { return _optionsAudioOffset; } set { _optionsAudioOffset = value; Store.Set("optionsAudioOffset", value, self); } }

        private static string _optionsAudioFx = "FX Volume: ";
        public static string optionsAudioFx { get { return _optionsAudioFx; } set { _optionsAudioFx = value; Store.Set("optionsAudioFx", value, self); } }

        private static string _optionsAudioMania = "Mania Hit Volume: ";
        public static string optionsAudioMania { get { return _optionsAudioMania; } set { _optionsAudioMania = value; Store.Set("optionsAudioMania", value, self); } }

        private static string _optionsAudioMusic = "Music Volume: ";
        public static string optionsAudioMusic { get { return _optionsAudioMusic; } set { _optionsAudioMusic = value; Store.Set("optionsAudioMusic", value, self); } }

        private static string _optionsAudioPitch = " Keep Pitch";
        public static string optionsAudioPitch { get { return _optionsAudioPitch; } set { _optionsAudioPitch = value; Store.Set("optionsAudioPitch", value, self); } }

        private static string _optionsAudioFail = " Keep Pitch on fail";
        public static string optionsAudioFail { get { return _optionsAudioFail; } set { _optionsAudioFail = value; Store.Set("optionsAudioFail", value, self); } }

        private static string _optionsAudioEngine = "SFX Engine: ";
        public static string optionsAudioEngine { get { return _optionsAudioEngine; } set { _optionsAudioEngine = value; Store.Set("optionsAudioEngine", value, self); } }

        private static string _optionsAudioLagfree = "Smooth";
        public static string optionsAudioLagfree { get { return _optionsAudioLagfree; } set { _optionsAudioLagfree = value; Store.Set("optionsAudioLagfree", value, self); } }

        private static string _optionsAudioInstant = "Instant";
        public static string optionsAudioInstant { get { return _optionsAudioInstant; } set { _optionsAudioInstant = value; Store.Set("optionsAudioInstant", value, self); } }

        private static string _optionsKeysIncrease = "Increase Volume: ";
        public static string optionsKeysIncrease { get { return _optionsKeysIncrease; } set { _optionsKeysIncrease = value; Store.Set("optionsKeysIncrease", value, self); } }

        private static string _optionsKeysDecrease = "Decrease Volume: ";
        public static string optionsKeysDecrease { get { return _optionsKeysDecrease; } set { _optionsKeysDecrease = value; Store.Set("optionsKeysDecrease", value, self); } }

        private static string _optionsKeysNext = "Next Song: ";
        public static string optionsKeysNext { get { return _optionsKeysNext; } set { _optionsKeysNext = value; Store.Set("optionsKeysNext", value, self); } }

        private static string _optionsKeysPrevious = "Previous Song: ";
        public static string optionsKeysPrevious { get { return _optionsKeysPrevious; } set { _optionsKeysPrevious = value; Store.Set("optionsKeysPrevious", value, self); } }

        private static string _optionsKeysPause = "Pause Song: ";
        public static string optionsKeysPause { get { return _optionsKeysPause; } set { _optionsKeysPause = value; Store.Set("optionsKeysPause", value, self); } }

        private static string _optionsGameplayTailwave = " Tail Wave";
        public static string optionsGameplayTailwave { get { return _optionsGameplayTailwave; } set { _optionsGameplayTailwave = value; Store.Set("optionsGameplayTailwave", value, self); } }

        private static string _optionsGameplayDrawspark = " Draw Sparks";
        public static string optionsGameplayDrawspark { get { return _optionsGameplayDrawspark; } set { _optionsGameplayDrawspark = value; Store.Set("optionsGameplayDrawspark", value, self); } }

        private static string _optionsGameplayScan = "Scan Songs";
        public static string optionsGameplayScan { get { return _optionsGameplayScan; } set { _optionsGameplayScan = value; Store.Set("optionsGameplayScan", value, self); } }

        private static string _optionsGameplayLosemult = " Lose Multiplier Animation";
        public static string optionsGameplayLosemult { get { return _optionsGameplayLosemult; } set { _optionsGameplayLosemult = value; Store.Set("optionsGameplayLosemult", value, self); } }

        private static string _optionsGameplayFailanim = " Song Fail Animation";
        public static string optionsGameplayFailanim { get { return _optionsGameplayFailanim; } set { _optionsGameplayFailanim = value; Store.Set("optionsGameplayFailanim", value, self); } }

        private static string _optionsGameplayLanguage = "Language: ";
        public static string optionsGameplayLanguage { get { return _optionsGameplayLanguage; } set { _optionsGameplayLanguage = value; Store.Set("optionsGameplayLanguage", value, self); } }

        private static string _optionsGameplayHighway = " Use always GH highway";
        public static string optionsGameplayHighway { get { return _optionsGameplayHighway; } set { _optionsGameplayHighway = value; Store.Set("optionsGameplayHighway", value, self); } }

        private static string _optionsGameplayInstantChange = " Instant Song Change";
        public static string optionsGameplayInstantChange { get { return _optionsGameplayInstantChange; } set { _optionsGameplayInstantChange = value; Store.Set("optionsGameplayInstantChange", value, self); } }

        private static string _optionsGameHitwindow = " Show Hit Window";
        public static string optionsGameHitwindow { get { return _optionsGameHitwindow; } set { _optionsGameHitwindow = value; Store.Set("optionsGameHitwindow", value, self); } }

        private static string _optionsSkinCustom = "Scan Custom Content";
        public static string optionsSkinCustom { get { return _optionsSkinCustom; } set { _optionsSkinCustom = value; Store.Set("optionsSkinCustom", value, self); } }

        private static string _optionsSkinSkin = "Skin";
        public static string optionsSkinSkin { get { return _optionsSkinSkin; } set { _optionsSkinSkin = value; Store.Set("optionsSkinSkin", value, self); } }

        private static string _optionsSkinHighway = "Player {0} Highway";
        public static string optionsSkinHighway { get { return _optionsSkinHighway; } set { _optionsSkinHighway = value; Store.Set("optionsSkinHighway", value, self); } }

        private static string _optionsRestart = "	Needs to restart the game to take effect";
        public static string optionsRestart { get { return _optionsRestart; } set { _optionsRestart = value; Store.Set("optionsRestart", value, self); } }

        private static string _popupEpilepsy = "This map contains flashing images that can cause injures";
        public static string popupEpilepsy { get { return _popupEpilepsy; } set { _popupEpilepsy = value; Store.Set("popupEpilepsy", value, self); } }

        private static string _popUpInstrument = "Current instrument does not match with the selected difficulty";
        public static string popUpInstrument { get { return _popUpInstrument; } set { _popUpInstrument = value; Store.Set("popUpInstrument", value, self); } }
    }
}
