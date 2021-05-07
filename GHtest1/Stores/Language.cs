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
        public static string languageName { get { return _languageName; } set { _languageName = value; Store.Set("languageName", value, self); } }
        private static string _languageName = "English";

        public static string menuPlay { get { return _menuPlay; } set { _menuPlay = value; Store.Set("menuPlay", value, self); } }
        private static string _menuPlay = "Play";

        public static string menuEditor { get { return _menuEditor; } set { _menuEditor = value; Store.Set("menuEditor", value, self); } }
        private static string _menuEditor = "Editor";

        public static string menuOptions { get { return _menuOptions; } set { _menuOptions = value; Store.Set("menuOptions", value, self); } }
        private static string _menuOptions = "Options";

        public static string menuExit { get { return _menuExit; } set { _menuExit = value; Store.Set("menuExit", value, self); } }
        private static string _menuExit = "Exit";

        public static string menuLocalPlay { get { return _menuLocalPlay; } set { _menuLocalPlay = value; Store.Set("menuLocalPlay", value, self); } }
        private static string _menuLocalPlay = "Local Play";

        public static string menuCoop { get { return _menuCoop; } set { _menuCoop = value; Store.Set("menuCoop", value, self); } }
        private static string _menuCoop = "Coop";

        public static string menuPractice { get { return _menuPractice; } set { _menuPractice = value; Store.Set("menuPractice", value, self); } }
        private static string _menuPractice = "Practice";

        public static string menuOnlinePlay { get { return _menuOnlinePlay; } set { _menuOnlinePlay = value; Store.Set("menuOnlinePlay", value, self); } }
        private static string _menuOnlinePlay = "Online Play";

        public static string menuPressBtn { get { return _menuPressBtn; } set { _menuPressBtn = value; Store.Set("menuPressBtn", value, self); } }
        private static string _menuPressBtn = "Press Button";

        public static string menuKeyboard { get { return _menuKeyboard; } set { _menuKeyboard = value; Store.Set("menuKeyboard", value, self); } }
        private static string _menuKeyboard = "Keyboard";

        public static string menuController { get { return _menuController; } set { _menuController = value; Store.Set("menuController", value, self); } }
        private static string _menuController = "Controller";

        public static string menuScan { get { return _menuScan; } set { _menuScan = value; Store.Set("menuScan", value, self); } }
        private static string _menuScan = "Scanning";

        public static string menuTitleListening { get { return _menuTitleListening; } set { _menuTitleListening = value; Store.Set("menuTitleListening", value, self); } }
        private static string _menuTitleListening = "Listening: ";

        public static string menuTitlePlaying { get { return _menuTitlePlaying; } set { _menuTitlePlaying = value; Store.Set("menuTitlePlaying", value, self); } }
        private static string _menuTitlePlaying = "Playing: ";

        public static string menuScanCalcDiff { get { return _menuCalcDiff; } set { _menuCalcDiff = value; Store.Set("menuCalcDiff", value, self); } }
        private static string _menuCalcDiff = "Calculating Difficulties";

        public static string menuScanCache { get { return _menuCache; } set { _menuCache = value; Store.Set("menuCache", value, self); } }
        private static string _menuCache = "Caching";

        public static string menuScanRead { get { return _menuScanRead; } set { _menuScanRead = value; Store.Set("menuScanRead", value, self); } }
        private static string _menuScanRead = "Loading: ";

        public static string menuScanDuplicate { get { return _menuScanDuplicate; } set { _menuScanDuplicate = value; Store.Set("menuScanDuplicate", value, self); } }
        private static string _menuScanDuplicate = "Searching for duplicates";

        public static string menuPlayerHelp { get { return _menuPlayerHelp; } set { _menuPlayerHelp = value; Store.Set("menuPlayerHelp", value, self); } }
        private static string _menuPlayerHelp = "Vol+ {0}  Vol- {1}     Prev {2}  Pause {3}  Next {4}";

        public static string menuNightcoreMode { get { return _menuNightcoreMode; } set { _menuNightcoreMode = value; Store.Set("menuNightcoreMode", value, self); } }
        private static string _menuNightcoreMode = "Nightcore mode: {0}";

        public static string menuNightcoreModeEnabled { get { return _menuNightcoreModeEnabled; } set { _menuNightcoreModeEnabled = value; Store.Set("menuNightcoreModeEnabled", value, self); } }
        private static string _menuNightcoreModeEnabled = "Enabled";

        public static string menuNightcoreModeDisabled { get { return _menuNightcoreModeDisabled; } set { _menuNightcoreModeDisabled = value; Store.Set("menuNightcoreModeDisabled", value, self); } }
        private static string _menuNightcoreModeDisabled = "Disabled";

        public static string menuWarningAddSong { get { return _menuWarningAddSong; } set { _menuWarningAddSong = value; Store.Set("menuWarningAddSong", value, self); } }
        private static string _menuWarningAddSong = "Cannot add song while scanning";

        public static string menuWarningMicOn { get { return _menuWarningMicOn; } set { _menuWarningMicOn = value; Store.Set("menuWarningMicOn", value, self); } }
        private static string _menuWarningMicOn = "Microphone enabled";

        public static string menuWarningMicOff { get { return _menuWarningMicOff; } set { _menuWarningMicOff = value; Store.Set("menuWarningMicOff", value, self); } }
        private static string _menuWarningMicOff = "Microphone disabled";

        public static string menuWarningAlreadyScanning { get { return _menuWarningAlreadyScanning; } set { _menuWarningAlreadyScanning = value; Store.Set("menuWarningAlreadyScanning", value, self); } }
        private static string _menuWarningAlreadyScanning = "Scanning already in process";

        public static string menuWarningStartedScan { get { return _menuWarningStartedScan; } set { _menuWarningStartedScan = value; Store.Set("menuWarningStartedScan", value, self); } }
        private static string _menuWarningStartedScan = "Started Scanning";

        public static string menuWarningScanFinish { get { return _menuWarningScanFinish; } set { _menuWarningScanFinish = value; Store.Set("menuWarningScanFinish", value, self); } }
        private static string _menuWarningScanFinish = "Finished scanning songs";

        public static string menuWarning2Instruments { get { return _menuWarning2Instruments; } set { _menuWarning2Instruments = value; Store.Set("menuWarning2Instruments", value, self); } }
        private static string _menuWarning2Instruments = "Cannot play normal with 2 instruments¿try Coop instead";

        public static string menuWarningMorePlayers { get { return _menuWarningMorePlayers; } set { _menuWarningMorePlayers = value; Store.Set("menuWarningMorePlayers", value, self); } }
        private static string _menuWarningMorePlayers = "Need to have more than 1 player";

        public static string menuModPlayer { get { return _menuModPlayer; } set { _menuModPlayer = value; Store.Set("menuModPlayer", value, self); } }
        private static string _menuModPlayer = "Player {0}";

        public static string menuModMods { get { return _menuModMods; } set { _menuModMods = value; Store.Set("menuModMods", value, self); } }
        private static string _menuModMods = "Mods";

        public static string menuModOptions { get { return _menuModOptions; } set { _menuModOptions = value; Store.Set("menuModOptions", value, self); } }
        private static string _menuModOptions = "Options";

        public static string menuModHard { get { return _menuModHard; } set { _menuModHard = value; Store.Set("menuModHard", value, self); } }
        private static string _menuModHard = "Hard";

        public static string menuModEasy { get { return _menuModEasy; } set { _menuModEasy = value; Store.Set("menuModEasy", value, self); } }
        private static string _menuModEasy = "Easy";

        public static string menuModSpeed { get { return _menuModSpeed; } set { _menuModSpeed = value; Store.Set("menuModSpeed", value, self); } }
        private static string _menuModSpeed = "Speed";

        public static string menuModHidden { get { return _menuModHidden; } set { _menuModHidden = value; Store.Set("menuModHidden", value, self); } }
        private static string _menuModHidden = "Hidden";

        public static string menuModAuto { get { return _menuModAuto; } set { _menuModAuto = value; Store.Set("menuModAuto", value, self); } }
        private static string _menuModAuto = "Auto";

        public static string menuModNotes { get { return _menuModNotes; } set { _menuModNotes = value; Store.Set("menuModNotes", value, self); } }
        private static string _menuModNotes = "Notes: {0}";

        public static string menuModInput { get { return _menuModInput; } set { _menuModInput = value; Store.Set("menuModInput", value, self); } }
        private static string _menuModInput = "Input: {0}";

        public static string menuModInNormal { get { return _menuModInNormal; } set { _menuModInNormal = value; Store.Set("menuModInNormal", value, self); } }
        private static string _menuModInNormal = "Normal";

        public static string menuModInAllstrum { get { return _menuModInAllstrum; } set { _menuModInAllstrum = value; Store.Set("menuModInAllstrum", value, self); } }
        private static string _menuModInAllstrum = "All Strum";

        public static string menuModInAlltap { get { return _menuModInAlltap; } set { _menuModInAlltap = value; Store.Set("menuModInAlltap", value, self); } }
        private static string _menuModInAlltap = "All Tap";

        public static string menuModInStrum { get { return _menuModInStrum; } set { _menuModInStrum = value; Store.Set("menuModInStrum", value, self); } }
        private static string _menuModInStrum = "Strum Only";

        public static string menuModInFretless { get { return _menuModInFretless; } set { _menuModInFretless = value; Store.Set("menuModInFretless", value, self); } }
        private static string _menuModInFretless = "Fretless";

        public static string menuModNotesNormal { get { return _menuModNotesNormal; } set { _menuModNotesNormal = value; Store.Set("menuModNotesNormal", value, self); } }
        private static string _menuModNotesNormal = "Normal";

        public static string menuModNotesFlip { get { return _menuModNotesFlip; } set { _menuModNotesFlip = value; Store.Set("menuModNotesFlip", value, self); } }
        private static string _menuModNotesFlip = "Flip";

        public static string menuModNotesShuffle { get { return _menuModNotesShuffle; } set { _menuModNotesShuffle = value; Store.Set("menuModNotesShuffle", value, self); } }
        private static string _menuModNotesShuffle = "Shuffle";

        public static string menuModNotesRandom { get { return _menuModNotesRandom; } set { _menuModNotesRandom = value; Store.Set("menuModNotesRandom", value, self); } }
        private static string _menuModNotesRandom = "Total Random";

        public static string menuModNofail { get { return _menuModNofail; } set { _menuModNofail = value; Store.Set("menuModNofail", value, self); } }
        private static string _menuModNofail = "No Fail";

        public static string menuModPerformance { get { return _menuModPerformance; } set { _menuModPerformance = value; Store.Set("menuModPerformance", value, self); } }
        private static string _menuModPerformance = "Performance Mode";

        public static string menuModModchart { get { return _menuModModchart; } set { _menuModModchart = value; Store.Set("menuModModchart", value, self); } }
        private static string _menuModModchart = "Modchart: {0}";

        public static string menuModModchartFull { get { return _menuModModchartFull; } set { _menuModModchartFull = value; Store.Set("menuModModchartFull", value, self); } }
        private static string _menuModModchartFull = "Full";

        public static string menuModTransform { get { return _menuModTransform; } set { _menuModTransform = value; Store.Set("menuModTransform", value, self); } }
        private static string _menuModTransform = "Transform";

        public static string menuModModchartWoHw { get { return _menuModModchartWoHw; } set { _menuModModchartWoHw = value; Store.Set("menuModModchartWoHw", value, self); } }
        private static string _menuModModchartWoHw = "Without highway";

        public static string menuModModchartInfoTar { get { return _menuModModchartInfoTar; } set { _menuModModchartInfoTar = value; Store.Set("menuModModchartInfoTar", value, self); } }
        private static string _menuModModchartInfoTar = "Info and Targets";

        public static string menuModModchartInfo { get { return _menuModModchartInfo; } set { _menuModModchartInfo = value; Store.Set("menuModModchartInfo", value, self); } }
        private static string _menuModModchartInfo = "Info";

        public static string menuModModchartTargets { get { return _menuModModchartTargets; } set { _menuModModchartTargets = value; Store.Set("menuModModchartTargets", value, self); } }
        private static string _menuModModchartTargets = "Targets";

        public static string menuModModchartNone { get { return _menuModModchartNone; } set { _menuModModchartNone = value; Store.Set("menuModModchartNone", value, self); } }
        private static string _menuModModchartNone = "None";

        public static string menuModAutoSP { get { return _menuModAutoSP; } set { _menuModAutoSP = value; Store.Set("menuModAutoSP", value, self); } }
        private static string _menuModAutoSP = "Auto SP";

        public static string menuModQuit { get { return _menuModQuit; } set { _menuModQuit = value; Store.Set("menuModQuit", value, self); } }
        private static string _menuModQuit = "Quit Profile";

        public static string menuOptionMode { get { return _menuOptionMode; } set { _menuOptionMode = value; Store.Set("menuOptionMode", value, self); } }
        private static string _menuOptionMode = "Game Mode: {0}";

        public static string menuProfileCreateIn { get { return _menuProfileCreateIn; } set { _menuProfileCreateIn = value; Store.Set("menuProfileCreateIn", value, self); } }
        private static string _menuProfileCreateIn = "Create Profile";

        public static string menuProfileCreate { get { return _menuProfileCreate; } set { _menuProfileCreate = value; Store.Set("menuProfileCreate", value, self); } }
        private static string _menuProfileCreate = "Create Profile";

        public static string menuProfileCancel { get { return _menuProfileCancel; } set { _menuProfileCancel = value; Store.Set("menuProfileCancel", value, self); } }
        private static string _menuProfileCancel = "Escape to Cancel";

        public static string menuProfileAccept { get { return _menuProfileAccept; } set { _menuProfileAccept = value; Store.Set("menuProfileAccept", value, self); } }
        private static string _menuProfileAccept = "Enter to Accept";

        public static string menuProfileBtns1 { get { return _menuProfileBtns1; } set { _menuProfileBtns1 = value; Store.Set("menuProfileBtns1", value, self); } }
        private static string _menuProfileBtns1 = "Btn 0: Green, Btn 1: Red";

        public static string menuProfileBtns2 { get { return _menuProfileBtns2; } set { _menuProfileBtns2 = value; Store.Set("menuProfileBtns2", value, self); } }
        private static string _menuProfileBtns2 = "Btn 2: Down, Btn 3: Up";

        public static string menuProfileBtnsP { get { return _menuProfileBtnsP; } set { _menuProfileBtnsP = value; Store.Set("menuProfileBtnsP", value, self); } }
        private static string _menuProfileBtnsP = "Btn Pressed: {0}";

        public static string menuProfileKeyAccept { get { return _menuProfileKeyAccept; } set { _menuProfileKeyAccept = value; Store.Set("menuProfileKeyAccept", value, self); } }
        private static string _menuProfileKeyAccept = "Enter: Accept";

        public static string menuProfileKeyDelete { get { return _menuProfileKeyDelete; } set { _menuProfileKeyDelete = value; Store.Set("menuProfileKeyDelete", value, self); } }
        private static string _menuProfileKeyDelete = "Delete: Delete";

        public static string menuProfileKeyReload { get { return _menuProfileKeyReload; } set { _menuProfileKeyReload = value; Store.Set("menuProfileKeyReload", value, self); } }
        private static string _menuProfileKeyReload = "Insert: Reload";

        public static string menuScoreChart { get { return _menuScoreChart; } set { _menuScoreChart = value; Store.Set("menuScoreChart", value, self); } }
        private static string _menuScoreChart = "Charted by: {0}";

        public static string menuScoreStreak { get { return _menuScoreStreak; } set { _menuScoreStreak = value; Store.Set("menuScoreStreak", value, self); } }
        private static string _menuScoreStreak = "Streak: {0}";

        public static string menuScoreAccuracy { get { return _menuScoreAccuracy; } set { _menuScoreAccuracy = value; Store.Set("menuScoreAccuracy", value, self); } }
        private static string _menuScoreAccuracy = "Accuracy: {0}";

        public static string menuScoreHits { get { return _menuScoreHits; } set { _menuScoreHits = value; Store.Set("menuScoreHits", value, self); } }
        private static string _menuScoreHits = "Hits: {0}";

        public static string menuScoreMisses { get { return _menuScoreMisses; } set { _menuScoreMisses = value; Store.Set("menuScoreMisses", value, self); } }
        private static string _menuScoreMisses = "Misses: {0}";

        public static string menuScoreModsNone { get { return _menuScoreModsNone; } set { _menuScoreModsNone = value; Store.Set("menuScoreModsNone", value, self); } }
        private static string _menuScoreModsNone = "None";

        public static string menuScoreMods { get { return _menuScoreMods; } set { _menuScoreMods = value; Store.Set("menuScoreMods", value, self); } }
        private static string _menuScoreMods = "Mods: {0}";

        public static string menuScoreGamepad { get { return _menuScoreGamepad; } set { _menuScoreGamepad = value; Store.Set("menuScoreGamepad", value, self); } }
        private static string _menuScoreGamepad = "Gamepad mode: {0}";

        public static string menuScoreGamepadOn { get { return _menuScoreGamepadOn; } set { _menuScoreGamepadOn = value; Store.Set("menuScoreGamepadOn", value, self); } }
        private static string _menuScoreGamepadOn = "On";

        public static string menuScoreGamepadOff { get { return _menuScoreGamepadOff; } set { _menuScoreGamepadOff = value; Store.Set("menuScoreGamepadOff", value, self); } }
        private static string _menuScoreGamepadOff = "Off";

        public static string menuBtnsSelect { get { return _menuBtnsSelect; } set { _menuBtnsSelect = value; Store.Set("menuBtnsSelect", value, self); } }
        private static string _menuBtnsSelect = "Select";

        public static string menuBtnsCancel { get { return _menuBtnsCancel; } set { _menuBtnsCancel = value; Store.Set("menuBtnsCancel", value, self); } }
        private static string _menuBtnsCancel = "Cancel";

        public static string menuBtnsPlay { get { return _menuBtnsPlay; } set { _menuBtnsPlay = value; Store.Set("menuBtnsPlay", value, self); } }
        private static string _menuBtnsPlay = "Play";

        public static string menuBtnsLeaderboard { get { return _menuBtnsLeaderboard; } set { _menuBtnsLeaderboard = value; Store.Set("menuBtnsLeaderboard", value, self); } }
        private static string _menuBtnsLeaderboard = "Leaderboard";

        public static string menuBtnsSearch { get { return _menuBtnsSearch; } set { _menuBtnsSearch = value; Store.Set("menuBtnsSearch", value, self); } }
        private static string _menuBtnsSearch = "Search";

        public static string menuBtnsSort { get { return _menuBtnsSort; } set { _menuBtnsSort = value; Store.Set("menuBtnsSort", value, self); } }
        private static string _menuBtnsSort = "Sort";

        public static string menuBtnsPrevSong { get { return _menuBtnsPrevSong; } set { _menuBtnsPrevSong = value; Store.Set("menuBtnsPrevSong", value, self); } }
        private static string _menuBtnsPrevSong = "Previous Song";

        public static string menuBtnsPauseSong { get { return _menuBtnsPauseSong; } set { _menuBtnsPauseSong = value; Store.Set("menuBtnsPauseSong", value, self); } }
        private static string _menuBtnsPauseSong = "Pause Song";

        public static string menuBtnsNextSong { get { return _menuBtnsNextSong; } set { _menuBtnsNextSong = value; Store.Set("menuBtnsNextSong", value, self); } }
        private static string _menuBtnsNextSong = "Next Song";

        public static string menuBtnsSongplayer { get { return _menuBtnsSongplayer; } set { _menuBtnsSongplayer = value; Store.Set("menuBtnsSongplayer", value, self); } }
        private static string _menuBtnsSongplayer = "Song Player";

        public static string menuBtnsReturnTimer { get { return _menuBtnsReturnTimer; } set { _menuBtnsReturnTimer = value; Store.Set("menuBtnsReturnTimer", value, self); } }
        private static string _menuBtnsReturnTimer = "Return ({0})";

        public static string menuBtnsReturn { get { return _menuBtnsReturn; } set { _menuBtnsReturn = value; Store.Set("menuBtnsReturn", value, self); } }
        private static string _menuBtnsReturn = "Return";

        public static string menuBtnsView { get { return _menuBtnsView; } set { _menuBtnsView = value; Store.Set("menuBtnsView", value, self); } }
        private static string _menuBtnsView = "View";

        public static string menuBtnsChangeLb { get { return _menuBtnsChangeLb; } set { _menuBtnsChangeLb = value; Store.Set("menuBtnsChangeLb", value, self); } }
        private static string _menuBtnsChangeLb = "Change Leaderboard";

        public static string menuBtnsContinue { get { return _menuBtnsContinue; } set { _menuBtnsContinue = value; Store.Set("menuBtnsContinue", value, self); } }
        private static string _menuBtnsContinue = "Continue";

        public static string menuBtnsReplay { get { return _menuBtnsReplay; } set { _menuBtnsReplay = value; Store.Set("menuBtnsReplay", value, self); } }
        private static string _menuBtnsReplay = "Replay";

        public static string menuBtnsPractice { get { return _menuBtnsPractice; } set { _menuBtnsPractice = value; Store.Set("menuBtnsPractice", value, self); } }
        private static string _menuBtnsPractice = "Practice";

        public static string menuBtnsInfo { get { return _menuBtnsInfo; } set { _menuBtnsInfo = value; Store.Set("menuBtnsInfo", value, self); } }
        private static string _menuBtnsInfo = "Info";

        public static string menuVolume { get { return _menuVolume; } set { _menuVolume = value; Store.Set("menuVolume", value, self); } }
        private static string _menuVolume = "Master";

        public static string gameScore { get { return _gameScore; } set { _gameScore = value; Store.Set("gameScore", value, self); } }
        private static string _gameScore = "Score: {0}";

        public static string gamePause { get { return _gamePause; } set { _gamePause = value; Store.Set("gamePause", value, self); } }
        private static string _gamePause = "PAUSE";

        public static string gamePausePlayer { get { return _gamePausePlayer; } set { _gamePausePlayer = value; Store.Set("gamePausePlayer", value, self); } }
        private static string _gamePausePlayer = "Player {0}";

        public static string gamePauseResume { get { return _gamePauseResume; } set { _gamePauseResume = value; Store.Set("gamePauseResume", value, self); } }
        private static string _gamePauseResume = "Resume";

        public static string gamePauseOptions { get { return _gamePauseOptions; } set { _gamePauseOptions = value; Store.Set("gamePauseOptions", value, self); } }
        private static string _gamePauseOptions = "Options";

        public static string gamePauseRestart { get { return _gamePauseRestart; } set { _gamePauseRestart = value; Store.Set("gamePauseRestart", value, self); } }
        private static string _gamePauseRestart = "Restart";

        public static string gamePauseExit { get { return _gamePauseExit; } set { _gamePauseExit = value; Store.Set("gamePauseExit", value, self); } }
        private static string _gamePauseExit = "Quit";

        public static string gameFail { get { return _gameFail; } set { _gameFail = value; Store.Set("gameFail", value, self); } }
        private static string _gameFail = "Song Failed";

        public static string gameFailRestart { get { return _gameFailRestart; } set { _gameFailRestart = value; Store.Set("gameFailRestart", value, self); } }
        private static string _gameFailRestart = "Retry";

        public static string gameFailExit { get { return _gameFailExit; } set { _gameFailExit = value; Store.Set("gameFailExit", value, self); } }
        private static string _gameFailExit = "Quit";

        public static string gameFailSave { get { return _gameFailSave; } set { _gameFailSave = value; Store.Set("gameFailSave", value, self); } }
        private static string _gameFailSave = "Save Play";

        public static string gameFPS { get { return _gameFPS; } set { _gameFPS = value; Store.Set("gameFPS", value, self); } }
        private static string _gameFPS = " FPS";

        public static string songSortBy { get { return _songSortBy; } set { _songSortBy = value; Store.Set("songSortBy", value, self); } }
        private static string _songSortBy = "Sorting by: ";

        public static string songSortName { get { return _songSortName; } set { _songSortName = value; Store.Set("songSortName", value, self); } }
        private static string _songSortName = "Name";

        public static string songSortArtist { get { return _songSortArtist; } set { _songSortArtist = value; Store.Set("songSortArtist", value, self); } }
        private static string _songSortArtist = "Artist";

        public static string songSortAlbum { get { return _songSortAlbum; } set { _songSortAlbum = value; Store.Set("songSortAlbum", value, self); } }
        private static string _songSortAlbum = "Album";

        public static string songSortYear { get { return _songSortYear; } set { _songSortYear = value; Store.Set("songSortYear", value, self); } }
        private static string _songSortYear = "Year";

        public static string songSortLength { get { return _songSortLength; } set { _songSortLength = value; Store.Set("songSortLength", value, self); } }
        private static string _songSortLength = "Length";

        public static string songSortPath { get { return _songSortPath; } set { _songSortPath = value; Store.Set("songSortPath", value, self); } }
        private static string _songSortPath = "Path";

        public static string songSortGenre { get { return _songSortGenre; } set { _songSortGenre = value; Store.Set("songSortGenre", value, self); } }
        private static string _songSortGenre = "Genre";

        public static string songSortCharter { get { return _songSortCharter; } set { _songSortCharter = value; Store.Set("songSortCharter", value, self); } }
        private static string _songSortCharter = "Charter";

        public static string songSortDiff { get { return _songSortDiff; } set { _songSortDiff = value; Store.Set("songSortDiff", value, self); } }
        private static string _songSortDiff = "Difficulty";

        public static string songCount { get { return _songCount; } set { _songCount = value; Store.Set("songCount", value, self); } }
        private static string _songCount = "Songs: {0}";

        public static string songNoteCount { get { return _songNoteCount; } set { _songNoteCount = value; Store.Set("songNoteCount", value, self); } }
        private static string _songNoteCount = "Notes: {0}";

        public static string songNoteGetting { get { return _songNoteGetting; } set { _songNoteGetting = value; Store.Set("songNoteGetting", value, self); } }
        private static string _songNoteGetting = "Getting";

        public static string songCurrentSearch { get { return _songCurrentSearch; } set { _songCurrentSearch = value; Store.Set("songCurrentSearch", value, self); } }
        private static string _songCurrentSearch = "Search: {0}";

        public static string songToSearch { get { return _songToSearch; } set { _songToSearch = value; Store.Set("songToSearch", value, self); } }
        private static string _songToSearch = "Search: ";

        public static string songDifficultyList { get { return _songDifficultyList; } set { _songDifficultyList = value; Store.Set("songDifficultyList", value, self); } }
        private static string _songDifficultyList = "Difficulty: ";

        public static string songDifficultyEasy { get { return _songDifficultyEasy; } set { _songDifficultyEasy = value; Store.Set("songDifficultyEasy", value, self); } }
        private static string _songDifficultyEasy = "Easy";

        public static string songDifficultyMedium { get { return _songDifficultyMedium; } set { _songDifficultyMedium = value; Store.Set("songDifficultyMedium", value, self); } }
        private static string _songDifficultyMedium = "Medium";

        public static string songDifficultyHard { get { return _songDifficultyHard; } set { _songDifficultyHard = value; Store.Set("songDifficultyHard", value, self); } }
        private static string _songDifficultyHard = "Hard";

        public static string songDifficultyExpert { get { return _songDifficultyExpert; } set { _songDifficultyExpert = value; Store.Set("songDifficultyExpert", value, self); } }
        private static string _songDifficultyExpert = "Expert";

        public static string songInstrumentGuitar { get { return _songInstrumentGuitar; } set { _songInstrumentGuitar = value; Store.Set("songInstrumentGuitar", value, self); } }
        private static string _songInstrumentGuitar = "Lead";

        public static string songInstrumentGuitar2 { get { return _songInstrumentGuitar2; } set { _songInstrumentGuitar2 = value; Store.Set("songInstrumentGuitar2", value, self); } }
        private static string _songInstrumentGuitar2 = "Guitar 2";

        public static string songInstrumentBass { get { return _songInstrumentBass; } set { _songInstrumentBass = value; Store.Set("songInstrumentBass", value, self); } }
        private static string _songInstrumentBass = "Bass";

        public static string songInstrumentBass2 { get { return _songInstrumentBass2; } set { _songInstrumentBass2 = value; Store.Set("songInstrumentBass2", value, self); } }
        private static string _songInstrumentBass2 = "Bass 2";

        public static string songInstrumentDrums { get { return _songInstrumentDrums; } set { _songInstrumentDrums = value; Store.Set("songInstrumentDrums", value, self); } }
        private static string _songInstrumentDrums = "Drums";

        public static string songInstrumentDrums5 { get { return _songInstrumentDrums5; } set { _songInstrumentDrums5 = value; Store.Set("songInstrumentDrums5", value, self); } }
        private static string _songInstrumentDrums5 = "Drums 5";

        public static string songInstrumentDrums5Pro { get { return _songInstrumentDrums5Pro; } set { _songInstrumentDrums5Pro = value; Store.Set("songInstrumentDrums5Pro", value, self); } }
        private static string _songInstrumentDrums5Pro = "Pro Drums 5";

        public static string songInstrumentDrumsPro { get { return _songInstrumentDrumsPro; } set { _songInstrumentDrumsPro = value; Store.Set("songInstrumentDrumsPro", value, self); } }
        private static string _songInstrumentDrumsPro = "Pro Drums";

        public static string songInstrumentKeys { get { return _songInstrumentKeys; } set { _songInstrumentKeys = value; Store.Set("songInstrumentKeys", value, self); } }
        private static string _songInstrumentKeys = "Keys";

        public static string songInstrumentVocals { get { return _songInstrumentVocals; } set { _songInstrumentVocals = value; Store.Set("songInstrumentVocals", value, self); } }
        private static string _songInstrumentVocals = "Vocals";

        public static string songInstrumentVocalsHarm1 { get { return _songInstrumentVocalsHarm1; } set { _songInstrumentVocalsHarm1 = value; Store.Set("songInstrumentVocalsHarm1", value, self); } }
        private static string _songInstrumentVocalsHarm1 = "Vocals Harms 1";

        public static string songInstrumentVocalsHarm2 { get { return _songInstrumentVocalsHarm2; } set { _songInstrumentVocalsHarm2 = value; Store.Set("songInstrumentVocalsHarm2", value, self); } }
        private static string _songInstrumentVocalsHarm2 = "Vocals Harms 2";

        public static string songInstrumentVocalsHarm3 { get { return _songInstrumentVocalsHarm3; } set { _songInstrumentVocalsHarm3 = value; Store.Set("songInstrumentVocalsHarm3", value, self); } }
        private static string _songInstrumentVocalsHarm3 = "Vocals Harms 3";

        public static string songInstrumentRhythm { get { return _songInstrumentRhythm; } set { _songInstrumentRhythm = value; Store.Set("songInstrumentRhythm", value, self); } }
        private static string _songInstrumentRhythm = "Rhythm";

        public static string songInstrumentRhythm2 { get { return _songInstrumentRhythm2; } set { _songInstrumentRhythm2 = value; Store.Set("songInstrumentRhythm2", value, self); } }
        private static string _songInstrumentRhythm2 = "Rhythm 2";

        public static string songInstrumentGuitarghl { get { return _songInstrumentGuitarghl; } set { _songInstrumentGuitarghl = value; Store.Set("songInstrumentGuitarghl", value, self); } }
        private static string _songInstrumentGuitarghl = "GHL Guitar";

        public static string songInstrumentBassghl { get { return _songInstrumentBassghl; } set { _songInstrumentBassghl = value; Store.Set("songInstrumentBassghl", value, self); } }
        private static string _songInstrumentBassghl = "GHL Bass";

        public static string songInstrumentGuitarPro { get { return _songInstrumentProguitar; } set { _songInstrumentProguitar = value; Store.Set("songInstrumentProguitar", value, self); } }
        private static string _songInstrumentProguitar = "Pro Guitar";

        public static string songInstrumentGuitarPro2 { get { return _songInstrumentGuitarPro2; } set { _songInstrumentGuitarPro2 = value; Store.Set("songInstrumentGuitarPro2", value, self); } }
        private static string _songInstrumentGuitarPro2 = "Pro Guitar 2";

        public static string songInstrumentGuitarPro22 { get { return _songInstrumentGuitarPro22; } set { _songInstrumentGuitarPro22 = value; Store.Set("songInstrumentGuitarPro22", value, self); } }
        private static string _songInstrumentGuitarPro22 = "Pro Guitar 22";

        public static string songInstrumentBassPro { get { return _songInstrumentProbass; } set { _songInstrumentProbass = value; Store.Set("songInstrumentProbass", value, self); } }
        private static string _songInstrumentProbass = "Pro Bass";

        public static string songInstrumentBassPro2 { get { return _songInstrumentBassPro2; } set { _songInstrumentBassPro2 = value; Store.Set("songInstrumentBassPro2", value, self); } }
        private static string _songInstrumentBassPro2 = "Pro Bass 2";

        public static string songInstrumentBassPro22 { get { return _songInstrumentBassPro22; } set { _songInstrumentBassPro22 = value; Store.Set("songInstrumentBassPro22", value, self); } }
        private static string _songInstrumentBassPro22 = "Pro Bass 22";

        public static string songDifficultyEmpty { get { return _songDifficultyEmpty; } set { _songDifficultyEmpty = value; Store.Set("songDifficultyEmpty", value, self); } }
        private static string _songDifficultyEmpty = "No Difficulties";

        public static string songRecordsNorecords { get { return _songRecordsNorecords; } set { _songRecordsNorecords = value; Store.Set("songRecordsNorecords", value, self); } }
        private static string _songRecordsNorecords = "No Records";

        public static string songRecordsLoading { get { return _songRecordsLoading; } set { _songRecordsLoading = value; Store.Set("songRecordsLoading", value, self); } }
        private static string _songRecordsLoading = "Loading Records";

        public static string songRecordsShow { get { return _songRecordsShow; } set { _songRecordsShow = value; Store.Set("songRecordsShow", value, self); } }
        private static string _songRecordsShow = "Showing: {0}";

        public static string songRecordsShowLocal { get { return _songRecordsShowLocal; } set { _songRecordsShowLocal = value; Store.Set("songRecordsShowLocal", value, self); } }
        private static string _songRecordsShowLocal = "Local";

        public static string songRecordsShowOnline { get { return _songRecordsShowOnline; } set { _songRecordsShowOnline = value; Store.Set("songRecordsShowOnline", value, self); } }
        private static string _songRecordsShowOnline = "Online";

        public static string songRecordsShowFriends { get { return _songRecordsShowFriends; } set { _songRecordsShowFriends = value; Store.Set("songRecordsShowFriends", value, self); } }
        private static string _songRecordsShowFriends = "Friends";

        public static string songRecordsNoDiffs { get { return _songRecordsNoDiffs; } set { _songRecordsNoDiffs = value; Store.Set("songRecordsNoDiffs", value, self); } }
        private static string _songRecordsNoDiffs = "No Records for this difficulty";

        public static string optionsButtonGreen { get { return _optionsButtonGreen; } set { _optionsButtonGreen = value; Store.Set("optionsButtonGreen", value, self); } }
        private static string _optionsButtonGreen = "Green";

        public static string optionsButtonRed { get { return _optionsButtonRed; } set { _optionsButtonRed = value; Store.Set("optionsButtonRed", value, self); } }
        private static string _optionsButtonRed = "Red";

        public static string optionsButtonYellow { get { return _optionsButtonYellow; } set { _optionsButtonYellow = value; Store.Set("optionsButtonYellow", value, self); } }
        private static string _optionsButtonYellow = "Yellow";

        public static string optionsButtonBlue { get { return _optionsButtonBlue; } set { _optionsButtonBlue = value; Store.Set("optionsButtonBlue", value, self); } }
        private static string _optionsButtonBlue = "Blue";

        public static string optionsButtonOrange { get { return _optionsButtonOrange; } set { _optionsButtonOrange = value; Store.Set("optionsButtonOrange", value, self); } }
        private static string _optionsButtonOrange = "Orange";

        public static string optionsButtonOpen { get { return _optionsButtonOpen; } set { _optionsButtonOpen = value; Store.Set("optionsButtonOpen", value, self); } }
        private static string _optionsButtonOpen = "Open";

        public static string optionsButtonSix { get { return _optionsButtonSix; } set { _optionsButtonSix = value; Store.Set("optionsButtonSix", value, self); } }
        private static string _optionsButtonSix = "Six";

        public static string optionsButtonStart { get { return _optionsButtonStart; } set { _optionsButtonStart = value; Store.Set("optionsButtonStart", value, self); } }
        private static string _optionsButtonStart = "Start";

        public static string optionsButtonSp { get { return _optionsButtonSp; } set { _optionsButtonSp = value; Store.Set("optionsButtonSp", value, self); } }
        private static string _optionsButtonSp = "Star Power";

        public static string optionsButtonUp { get { return _optionsButtonUp; } set { _optionsButtonUp = value; Store.Set("optionsButtonUp", value, self); } }
        private static string _optionsButtonUp = "Up";

        public static string optionsButtonDown { get { return _optionsButtonDown; } set { _optionsButtonDown = value; Store.Set("optionsButtonDown", value, self); } }
        private static string _optionsButtonDown = "Down";

        public static string optionsButtonWhammy { get { return _optionsButtonWhammy; } set { _optionsButtonWhammy = value; Store.Set("optionsButtonWhammy", value, self); } }
        private static string _optionsButtonWhammy = "Whammy";

        public static string optionsButtonAxis { get { return _optionsButtonAxis; } set { _optionsButtonAxis = value; Store.Set("optionsButtonAxis", value, self); } }
        private static string _optionsButtonAxis = "Axis";

        public static string optionsButtonLefty { get { return _optionsButtonLefty; } set { _optionsButtonLefty = value; Store.Set("optionsButtonLefty", value, self); } }
        private static string _optionsButtonLefty = "Lefty";

        public static string optionsButtonKeyboard { get { return _optionsButtonKeyboard; } set { _optionsButtonKeyboard = value; Store.Set("optionsButtonKeyboard", value, self); } }
        private static string _optionsButtonKeyboard = "Keyboard";

        public static string optionsButtonGamepad { get { return _optionsButtonGamepad; } set { _optionsButtonGamepad = value; Store.Set("optionsButtonGamepad", value, self); } }
        private static string _optionsButtonGamepad = "Gamepad";

        public static string optionsButtonDz { get { return _optionsButtonDz; } set { _optionsButtonDz = value; Store.Set("optionsButtonDz", value, self); } }
        private static string _optionsButtonDz = "DZ";

        public static string optionsButtonPlayer { get { return _optionsButtonPlayer; } set { _optionsButtonPlayer = value; Store.Set("optionsButtonPlayer", value, self); } }
        private static string _optionsButtonPlayer = "Player {0}";

        public static string optionsButtonInstrument { get { return _optionsButtonInstrument; } set { _optionsButtonInstrument = value; Store.Set("optionsButtonInstrument", value, self); } }
        private static string _optionsButtonInstrument = "Instrument: ";

        public static string optionsButton5fret { get { return _optionsButton5fret; } set { _optionsButton5fret = value; Store.Set("optionsButton5fret", value, self); } }
        private static string _optionsButton5fret = "5 Fret";

        public static string optionsButton6fret { get { return _optionsButton6fret; } set { _optionsButton6fret = value; Store.Set("optionsButton6fret", value, self); } }
        private static string _optionsButton6fret = "6 Fret";

        public static string optionsButtonDrums { get { return _optionsButtonDrums; } set { _optionsButtonDrums = value; Store.Set("optionsButtonDrums", value, self); } }
        private static string _optionsButtonDrums = "Drums";

        public static string optionsButtonVocals { get { return _optionsButtonVocals; } set { _optionsButtonVocals = value; Store.Set("optionsButtonVocals", value, self); } }
        private static string _optionsButtonVocals = "Vocals";

        public static string optionsButtonPadBtn { get { return _optionsButtonPadBtn; } set { _optionsButtonPadBtn = value; Store.Set("optionsButtonPadBtn", value, self); } }
        private static string _optionsButtonPadBtn = "Button {0}";

        public static string optionsButtonPadPad { get { return _optionsButtonPadPad; } set { _optionsButtonPadPad = value; Store.Set("optionsButtonPadPad", value, self); } }
        private static string _optionsButtonPadPad = "Pad {0}";

        public static string optionsButtonPadAxis { get { return _optionsButtonPadAxis; } set { _optionsButtonPadAxis = value; Store.Set("optionsButtonPadAxis", value, self); } }
        private static string _optionsButtonPadAxis = "Axis {0}";

        public static string optionsButtonPadUnknown { get { return _optionsButtonPadUnknown; } set { _optionsButtonPadUnknown = value; Store.Set("optionsButtonPadUnknown", value, self); } }
        private static string _optionsButtonPadUnknown = "Unknown";

        public static string optionsVideo { get { return _optionsVideo; } set { _optionsVideo = value; Store.Set("optionsVideo", value, self); } }
        private static string _optionsVideo = "Video";

        public static string optionsAudio { get { return _optionsAudio; } set { _optionsAudio = value; Store.Set("optionsAudio", value, self); } }
        private static string _optionsAudio = "Audio";

        public static string optionsKeys { get { return _optionsKeys; } set { _optionsKeys = value; Store.Set("optionsKeys", value, self); } }
        private static string _optionsKeys = "Keys";

        public static string optionsGameplay { get { return _optionsGameplay; } set { _optionsGameplay = value; Store.Set("optionsGameplay", value, self); } }
        private static string _optionsGameplay = "Game";

        public static string optionsSkin { get { return _optionsSkin; } set { _optionsSkin = value; Store.Set("optionsSkin", value, self); } }
        private static string _optionsSkin = "Skin";

        public static string optionsController { get { return _optionsController; } set { _optionsController = value; Store.Set("optionsController", value, self); } }
        private static string _optionsController = "Controller";

        public static string optionsVideoUnlimited { get { return _optionsVideoUnlimited; } set { _optionsVideoUnlimited = value; Store.Set("optionsVideoUnlimited", value, self); } }
        private static string _optionsVideoUnlimited = "Unlimited";

        public static string optionsVideoFullscreen { get { return _optionsVideoFullscreen; } set { _optionsVideoFullscreen = value; Store.Set("optionsVideoFullscreen", value, self); } }
        private static string _optionsVideoFullscreen = " Fullscreen";

        public static string optionsVideoVsync { get { return _optionsVideoVsync; } set { _optionsVideoVsync = value; Store.Set("optionsVideoVsync", value, self); } }
        private static string _optionsVideoVsync = " VSync";

        public static string optionsVideoFps { get { return _optionsVideoFps; } set { _optionsVideoFps = value; Store.Set("optionsVideoFps", value, self); } }
        private static string _optionsVideoFps = "Framerate: ";

        public static string optionsVideoResolution { get { return _optionsVideoResolution; } set { _optionsVideoResolution = value; Store.Set("optionsVideoResolution", value, self); } }
        private static string _optionsVideoResolution = "Resolution: ";

        public static string optionsVideoShowfps { get { return _optionsVideoShowfps; } set { _optionsVideoShowfps = value; Store.Set("optionsVideoShowfps", value, self); } }
        private static string _optionsVideoShowfps = " Show FPS";

        public static string optionsVideoExtreme { get { return _optionsVideoExtreme; } set { _optionsVideoExtreme = value; Store.Set("optionsVideoExtreme", value, self); } }
        private static string _optionsVideoExtreme = " Extreme Performance";

        public static string optionsVideoTailQuality { get { return _optionsVideoTailQuality; } set { _optionsVideoTailQuality = value; Store.Set("optionsVideoTailQuality", value, self); } }
        private static string _optionsVideoTailQuality = "Tail Quality: {0}";

        public static string optionsVideoThreadWarning { get { return _optionsVideoThreadWarning; } set { _optionsVideoThreadWarning = value; Store.Set("optionsVideoThreadWarning", value, self); } }
        private static string _optionsVideoThreadWarning = "Is recommended to enable Single Thread if you have one core CPU";

        public static string optionsVideoSingleThread { get { return _optionsVideoSingleThread; } set { _optionsVideoSingleThread = value; Store.Set("optionsVideoSingleThread", value, self); } }
        private static string _optionsVideoSingleThread = " Single Thread";

        public static string optionsVideoDrawMenuFx { get { return _optionsVideoDrawMenuFx; } set { _optionsVideoDrawMenuFx = value; Store.Set("optionsVideoDrawMenuFx", value, self); } }
        private static string _optionsVideoDrawMenuFx = " Draw menu effects";

        public static string optionsAudioMaster { get { return _optionsAudioMaster; } set { _optionsAudioMaster = value; Store.Set("optionsAudioMaster", value, self); } }
        private static string _optionsAudioMaster = "Master Volume: ";

        public static string optionsAudioOffset { get { return _optionsAudioOffset; } set { _optionsAudioOffset = value; Store.Set("optionsAudioOffset", value, self); } }
        private static string _optionsAudioOffset = "Audio Offset: ";

        public static string optionsAudioFx { get { return _optionsAudioFx; } set { _optionsAudioFx = value; Store.Set("optionsAudioFx", value, self); } }
        private static string _optionsAudioFx = "FX Volume: ";

        public static string optionsAudioMania { get { return _optionsAudioMania; } set { _optionsAudioMania = value; Store.Set("optionsAudioMania", value, self); } }
        private static string _optionsAudioMania = "Mania Hit Volume: ";

        public static string optionsAudioMusic { get { return _optionsAudioMusic; } set { _optionsAudioMusic = value; Store.Set("optionsAudioMusic", value, self); } }
        private static string _optionsAudioMusic = "Music Volume: ";

        public static string optionsAudioPitch { get { return _optionsAudioPitch; } set { _optionsAudioPitch = value; Store.Set("optionsAudioPitch", value, self); } }
        private static string _optionsAudioPitch = " Keep Pitch";

        public static string optionsAudioFail { get { return _optionsAudioFail; } set { _optionsAudioFail = value; Store.Set("optionsAudioFail", value, self); } }
        private static string _optionsAudioFail = " Keep Pitch on fail";

        public static string optionsAudioEngine { get { return _optionsAudioEngine; } set { _optionsAudioEngine = value; Store.Set("optionsAudioEngine", value, self); } }
        private static string _optionsAudioEngine = "SFX Engine: ";

        public static string optionsAudioLagfree { get { return _optionsAudioLagfree; } set { _optionsAudioLagfree = value; Store.Set("optionsAudioLagfree", value, self); } }
        private static string _optionsAudioLagfree = "Smooth";

        public static string optionsAudioInstant { get { return _optionsAudioInstant; } set { _optionsAudioInstant = value; Store.Set("optionsAudioInstant", value, self); } }
        private static string _optionsAudioInstant = "Instant";

        public static string optionsKeysIncrease { get { return _optionsKeysIncrease; } set { _optionsKeysIncrease = value; Store.Set("optionsKeysIncrease", value, self); } }
        private static string _optionsKeysIncrease = "Increase Volume: ";

        public static string optionsKeysDecrease { get { return _optionsKeysDecrease; } set { _optionsKeysDecrease = value; Store.Set("optionsKeysDecrease", value, self); } }
        private static string _optionsKeysDecrease = "Decrease Volume: ";

        public static string optionsKeysNext { get { return _optionsKeysNext; } set { _optionsKeysNext = value; Store.Set("optionsKeysNext", value, self); } }
        private static string _optionsKeysNext = "Next Song: ";

        public static string optionsKeysPrevious { get { return _optionsKeysPrevious; } set { _optionsKeysPrevious = value; Store.Set("optionsKeysPrevious", value, self); } }
        private static string _optionsKeysPrevious = "Previous Song: ";

        public static string optionsKeysPause { get { return _optionsKeysPause; } set { _optionsKeysPause = value; Store.Set("optionsKeysPause", value, self); } }
        private static string _optionsKeysPause = "Pause Song: ";

        public static string optionsGameplayTailwave { get { return _optionsGameplayTailwave; } set { _optionsGameplayTailwave = value; Store.Set("optionsGameplayTailwave", value, self); } }
        private static string _optionsGameplayTailwave = " Tail Wave";

        public static string optionsGameplayDrawspark { get { return _optionsGameplayDrawspark; } set { _optionsGameplayDrawspark = value; Store.Set("optionsGameplayDrawspark", value, self); } }
        private static string _optionsGameplayDrawspark = " Draw Sparks";

        public static string optionsGameplayScan { get { return _optionsGameplayScan; } set { _optionsGameplayScan = value; Store.Set("optionsGameplayScan", value, self); } }
        private static string _optionsGameplayScan = "Scan Songs";

        public static string optionsGameplayLosemult { get { return _optionsGameplayLosemult; } set { _optionsGameplayLosemult = value; Store.Set("optionsGameplayLosemult", value, self); } }
        private static string _optionsGameplayLosemult = " Lose Multiplier Animation";

        public static string optionsGameplayFailanim { get { return _optionsGameplayFailanim; } set { _optionsGameplayFailanim = value; Store.Set("optionsGameplayFailanim", value, self); } }
        private static string _optionsGameplayFailanim = " Song Fail Animation";

        public static string optionsGameplayLanguage { get { return _optionsGameplayLanguage; } set { _optionsGameplayLanguage = value; Store.Set("optionsGameplayLanguage", value, self); } }
        private static string _optionsGameplayLanguage = "Language: ";

        public static string optionsGameplayHighway { get { return _optionsGameplayHighway; } set { _optionsGameplayHighway = value; Store.Set("optionsGameplayHighway", value, self); } }
        private static string _optionsGameplayHighway = " Use always GH highway";

        public static string optionsGameplayInstantChange { get { return _optionsGameplayInstantChange; } set { _optionsGameplayInstantChange = value; Store.Set("optionsGameplayInstantChange", value, self); } }
        private static string _optionsGameplayInstantChange = " Instant Song Change";

        public static string optionsGameHitwindow { get { return _optionsGameHitwindow; } set { _optionsGameHitwindow = value; Store.Set("optionsGameHitwindow", value, self); } }
        private static string _optionsGameHitwindow = " Show Hit Window";

        public static string optionsGameVideoFlip { get { return _optionsGameVideoFlip; } set { _optionsGameVideoFlip = value; Store.Set("optionsGameVideoFlip", value, self); } }
        private static string _optionsGameVideoFlip = " Flip video in Lefty mode";

        public static string optionsGameDiffsort { get { return _optionsGameDiffsort; } set { _optionsGameDiffsort = value; Store.Set("optionsGameDiffsort", value, self); } }
        private static string _optionsGameDiffsort = "Difficulty sorting: {0}";

        public static string optionsGameDiffsortEverything { get { return _optionsGameDiffsortEverything; } set { _optionsGameDiffsortEverything = value; Store.Set("optionsGameDiffsortEverything", value, self); } }
        private static string _optionsGameDiffsortEverything = "Everything";

        public static string optionsGameDiffsortSelected { get { return _optionsGameDiffsortSelected; } set { _optionsGameDiffsortSelected = value; Store.Set("optionsGameDiffsortSelected", value, self); } }
        private static string _optionsGameDiffsortSelected = "Selected";

        public static string optionsGameDiffsortStrict { get { return _optionsGameDiffsortStrict; } set { _optionsGameDiffsortStrict = value; Store.Set("optionsGameDiffsortStrict", value, self); } }
        private static string _optionsGameDiffsortStrict = "Strict";

        public static string optionsSkinCustom { get { return _optionsSkinCustom; } set { _optionsSkinCustom = value; Store.Set("optionsSkinCustom", value, self); } }
        private static string _optionsSkinCustom = "Scan Custom Content";

        public static string optionsSkinSkin { get { return _optionsSkinSkin; } set { _optionsSkinSkin = value; Store.Set("optionsSkinSkin", value, self); } }
        private static string _optionsSkinSkin = "Skin";

        public static string optionsSkinDefault { get { return _optionsSkinDefault; } set { _optionsSkinDefault = value; Store.Set("optionsSkinDefault", value, self); } }
        private static string _optionsSkinDefault = "Default";

        public static string optionsSkinHighway { get { return _optionsSkinHighway; } set { _optionsSkinHighway = value; Store.Set("optionsSkinHighway", value, self); } }
        private static string _optionsSkinHighway = "Player {0} Highway";

        public static string optionsRestart { get { return _optionsRestart; } set { _optionsRestart = value; Store.Set("optionsRestart", value, self); } }
        private static string _optionsRestart = "	Needs to restart the game to take effect";

        public static string popupEpilepsy { get { return _popupEpilepsy; } set { _popupEpilepsy = value; Store.Set("popupEpilepsy", value, self); } }
        private static string _popupEpilepsy = "This map contains flashing images that can cause injures";

        public static string popUpInstrument { get { return _popUpInstrument; } set { _popUpInstrument = value; Store.Set("popUpInstrument", value, self); } }
        private static string _popUpInstrument = "Current instrument does not match with the selected difficulty";

        public static string practiceStart { get { return _practiceStart; } set { _practiceStart = value; Store.Set("practiceStart", value, self); } }
        private static string _practiceStart = "Start";

        public static string practiceEnd { get { return _practiceEnd; } set { _practiceEnd = value; Store.Set("practiceEnd", value, self); } }
        private static string _practiceEnd = "End";

        public static string practiceBtns1 { get { return _practiceBtns1; } set { _practiceBtns1 = value; Store.Set("practiceBtns1", value, self); } }
        private static string _practiceBtns1 = "{0}  Set start {1}  Set end {2} Snap {3} {4}  Remove {5} Exit";

        public static string practiceBtns2 { get { return _practiceBtns2; } set { _practiceBtns2 = value; Store.Set("practiceBtns2", value, self); } }
        private static string _practiceBtns2 = "{0} Speed {1}  {2} {3}  Start";

        public static string practiceBtnsPlay { get { return _practiceBtnsPlay; } set { _practiceBtnsPlay = value; Store.Set("practiceBtnsPlay", value, self); } }
        private static string _practiceBtnsPlay = "Speed {0}   {1}  Stop {2}  Reset";

    }
}