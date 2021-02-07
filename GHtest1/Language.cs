using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GHtest1 {
    class Language {
        public static string menuPlay = "";
        public static string menuEditor = "";
        public static string menuOption = "";
        public static string menuExit = "";

        public static string menuLclPlay = "";
        public static string menuOnlPlay = "";

        public static string menuPressBtn = "";
        public static string menuKeyboard = "";
        public static string menuController = "";
        public static string menuPlaying = "";
        public static string menuBlueTo = "";
        public static string menuScanning = "";
        public static string menuCalcDiffs = "";
        public static string menuCaching = "";
        public static string menuPlayerHelp = "";
        public static string menuModPlayer = "";
        public static string menuModMods = "";
        public static string menuModOptions = "";
        public static string menuModHard = "";
        public static string menuModEasy = "";
        public static string menuModSpeed = "";
        public static string menuModHidden = "";
        public static string menuModAuto = "";
        public static string menuModNotes = "";
        public static string menuModNormal = "";
        public static string menuModFlip = "";
        public static string menuModShuffle = "";
        public static string menuModRandom = "";
        public static string menuModInput = "";
        public static string menuModInputNormal = "";
        public static string menuModInputAllStrum = "";
        public static string menuModInputAllTap = "";
        public static string menuModInputStrum = "";
        public static string menuModInputFretLess = "";
        public static string menuModNofail = "";
        public static string menuModPerformance = "";
        public static string menuModTransform = "";
        public static string menuModAutoSP = "";

        public static string menuModQuit = "";

        public static string menuOptionMode = "";

        public static string menuProfileCreateIn = "";
        public static string menuProfileCreate = "";
        public static string menuProfileCancel = "";
        public static string menuProfileAccept = "";

        public static string menuBtnsMain = "";
        public static string menuBtnsSong = "";
        public static string menuBtnsOptions = "";
        public static string menuBtnsDiff = "";
        public static string menuBtnsRecords = "";
        public static string menuVolume = "";

        public static string gameScore = "";
        public static string gamePause = "";
        public static string gamePausePlayer = "";
        public static string gamePauseResume = "";
        public static string gamePauseRestart = "";
        public static string gamePauseOptions = "";
        public static string gamePauseExit = "";
        public static string gameFail = "";
        public static string gameFailRestart = "";
        public static string gameFailExit = "";
        public static string gameFailSave = "";

        public static string songSortBy = "";
        public static string songSortName = "";
        public static string songSortAlbum = "";
        public static string songSortArtist = "";
        public static string songSortPath = "";
        public static string songSortYear = "";
        public static string songSortGenre = "";
        public static string songSortLength = "";
        public static string songSortCharter = "";
        public static string songSortDiff = "";
        public static string songSortbyInstrument = "";
        public static string songSortinsOn = "";
        public static string songSortinsOff = "";
        public static string songCount = "";

        public static string songDiffList = "";
        public static string songDiffEasy = "";
        public static string songDiffMedium = "";
        public static string songDiffHard = "";
        public static string songDiffExpert = "";

        public static string instrumentGuitar = "";
        public static string instrument2Guitar = "";
        public static string instrumentBass = "";
        public static string instrument2Bass = "";
        public static string instrumentDrums = "";
        public static string instrumentKeys = "";
        public static string instrumentVocals = "";
        public static string instrumentRhythm = "";
        public static string instrument2Rhythm = "";
        public static string instrumentGuitarGHL = "";
        public static string instrumentBassGHL = "";
        public static string instrumentProGuitar = "";
        public static string instrumentProBass = "";

        public static string recordsList = "";
        public static string recordsNorec = "";
        public static string recordsSong = "";
        public static string recordsLoading = "";

        public static string optionButtonGreen = "";
        public static string optionButtonRed = "";
        public static string optionButtonYellow = "";
        public static string optionButtonBlue = "";
        public static string optionButtonOrange = "";
        public static string optionButtonOpen = "";
        public static string optionButtonSix = "";
        public static string optionButtonStart = "";
        public static string optionButtonSp = "";
        public static string optionButtonUp = "";
        public static string optionButtonDown = "";
        public static string optionButtonWhammy = "";
        public static string optionButtonAxis = "";
        public static string optionButtonKeyboard = "";
        public static string optionButtonGamepad = "";
        public static string optionButtonDz = "";
        public static string optionButtonLefty = "";
        public static string optionButtonGpMode = "";
        public static string optionButtonPlayer = "";
        public static string optionButtonInstrument = "";
        public static string optionButton5Fret = "";
        public static string optionButton6Fret = "";
        public static string optionButtonDrums = "";

        public static string optionVideo = "";
        public static string optionAudio = "";
        public static string optionKeys = "";
        public static string optionGameplay = "";
        public static string optionSkin = "";
        public static string optionController = "";

        public static string optionVideoUnlimited = "";
        public static string optionVideoFullscreen = "";
        public static string optionVideoVSync = "";
        public static string optionVideoFPS = "";
        public static string optionVideoResolution = "";
        public static string optionVideoShowFPS = "";
        public static string optionVideoExtreme = "";
        public static string optionVideoTailQuality = "";
        public static string optionVideoThreadWarning = "";
        public static string optionVideoSingleThread = "";
        public static string optionVideoMenuFx = "";

        public static string optionAudioMaster = "";
        public static string optionAudioOffset = "";
        public static string optionAudioFx = "";
        public static string optionAudioMania = "";
        public static string optionAudioMusic = "";
        public static string optionAudioPitch = "";
        public static string optionAudioFail = "";
        public static string optionAudioEngine = "";
        public static string optionAudioInstant = "";
        public static string optionAudioLagfree = "";

        public static string optionKeysIncrease = "";
        public static string optionKeysDecrease = "";
        public static string optionKeysNext = "";
        public static string optionKeysPrev = "";
        public static string optionKeysPause = "";

        public static string optionGameplayTailwave = "";
        public static string optionGameplayDrawspark = "";
        public static string optionGameplayScan = "";
        public static string optionGameplayLosemult = "";
        public static string optionGameplayFailanim = "";
        public static string optionGameplayLanguage = "";
        public static string optionGameplayHighway = "";

        public static string optionRestart = "";

        public static string popupEpilepsy = "";
        public static string popupInstrument = "";

        public static string optionSkinCustomscan = "";
        public static string optionSkinSkin = "";
        public static string optionSkinHighway = "";
        public static string stadisticFps = "";


        public static string language = "en";
        public static void LoadLanguage() { LoadLanguage(language); }
        public static void LoadLanguage(string lang) {
            string[] lines = new string[] { };
            lines = File.ReadAllLines("Content/Languages/" + lang + ".txt", Encoding.UTF8);
            for (int i = 0; i < lines.Length; i++) {
                string[] split = lines[i].Split('=');
                split[0] = split[0].Trim();
                if (split.Length != 2)
                    continue;
                if (split[0].Equals("Menu.play")) menuPlay = split[1];
                else if (split[0].Equals("Menu.editor")) menuEditor = split[1];
                else if (split[0].Equals("Menu.options")) menuOption = split[1];
                else if (split[0].Equals("Menu.exit")) menuExit = split[1];
                else if (split[0].Equals("Menu.localPlay")) menuLclPlay = split[1];
                else if (split[0].Equals("Menu.onlinePlay")) menuOnlPlay = split[1];
                else if (split[0].Equals("Menu.pressBtn")) menuPressBtn = split[1];
                else if (split[0].Equals("Menu.keyboard")) menuKeyboard = split[1];
                else if (split[0].Equals("Menu.controller")) menuController = split[1];
                else if (split[0].Equals("Menu.playing")) menuPlaying = split[1];
                else if (split[0].Equals("Menu.blueTo")) menuBlueTo = split[1];
                else if (split[0].Equals("Menu.scan")) menuScanning = split[1];
                else if (split[0].Equals("Menu.calcDiff")) menuCalcDiffs = split[1];
                else if (split[0].Equals("Menu.cache")) menuCaching = split[1];
                else if (split[0].Equals("Menu.playerHelp")) menuPlayerHelp = split[1];
                else if (split[0].Equals("Menu.Mod.player")) menuModPlayer = split[1];
                else if (split[0].Equals("Menu.Mod.mods")) menuModMods = split[1];
                else if (split[0].Equals("Menu.Mod.options")) menuModOptions = split[1];
                else if (split[0].Equals("Menu.Mod.hard")) menuModHard = split[1];
                else if (split[0].Equals("Menu.Mod.easy")) menuModEasy = split[1];
                else if (split[0].Equals("Menu.Mod.speed")) menuModSpeed = split[1];
                else if (split[0].Equals("Menu.Mod.hidden")) menuModHidden = split[1];
                else if (split[0].Equals("Menu.Mod.auto")) menuModAuto = split[1];
                else if (split[0].Equals("Menu.Mod.notes")) menuModNotes = split[1];
                else if (split[0].Equals("Menu.Mod.input")) menuModInput = split[1];
                else if (split[0].Equals("Menu.Mod.inNormal")) menuModInputNormal = split[1];
                else if (split[0].Equals("Menu.Mod.inAllstrum")) menuModInputAllStrum = split[1];
                else if (split[0].Equals("Menu.Mod.inAlltap")) menuModInputAllTap = split[1];
                else if (split[0].Equals("Menu.Mod.inStrum")) menuModInputStrum = split[1];
                else if (split[0].Equals("Menu.Mod.inFretless")) menuModInputFretLess = split[1];
                else if (split[0].Equals("Menu.Mod.notesNormal")) menuModNormal = split[1];
                else if (split[0].Equals("Menu.Mod.notesFlip")) menuModFlip = split[1];
                else if (split[0].Equals("Menu.Mod.notesShuffle")) menuModShuffle = split[1];
                else if (split[0].Equals("Menu.Mod.notesRandom")) menuModRandom = split[1];
                else if (split[0].Equals("Menu.Mod.nofail")) menuModNofail = split[1];
                else if (split[0].Equals("Menu.Mod.performance")) menuModPerformance = split[1];
                else if (split[0].Equals("Menu.Mod.transform")) menuModTransform = split[1];
                else if (split[0].Equals("Menu.Mod.autoSP")) menuModAutoSP = split[1];

                else if (split[0].Equals("Menu.Mod.quit")) menuModQuit = split[1];

                else if (split[0].Equals("Menu.Option.mode")) menuOptionMode = split[1];

                else if (split[0].Equals("Menu.Profile.createIn")) menuProfileCreateIn = split[1];
                else if (split[0].Equals("Menu.Profile.create")) menuProfileCreate = split[1];
                else if (split[0].Equals("Menu.Profile.cancel")) menuProfileCancel = split[1];
                else if (split[0].Equals("Menu.Profile.accept")) menuProfileAccept = split[1];

                else if (split[0].Equals("Menu.Btns.main")) menuBtnsMain = split[1];
                else if (split[0].Equals("Menu.Btns.song")) menuBtnsSong = split[1];
                else if (split[0].Equals("Menu.Btns.options")) menuBtnsOptions = split[1];
                else if (split[0].Equals("Menu.Btns.diff")) menuBtnsDiff = split[1];
                else if (split[0].Equals("Menu.Btns.records")) menuBtnsRecords = split[1];
                else if (split[0].Equals("Menu.volume")) menuVolume = split[1];

                else if (split[0].Equals("Game.score")) gameScore = split[1];
                else if (split[0].Equals("Game.Pause")) gamePause = split[1];
                else if (split[0].Equals("Game.Pause.player")) gamePausePlayer = split[1];
                else if (split[0].Equals("Game.Pause.resume")) gamePauseResume = split[1];
                else if (split[0].Equals("Game.Pause.options")) gamePauseOptions = split[1];
                else if (split[0].Equals("Game.Pause.restart")) gamePauseRestart = split[1];
                else if (split[0].Equals("Game.Pause.exit")) gamePauseExit = split[1];
                else if (split[0].Equals("Game.Fail")) gameFail = split[1];
                else if (split[0].Equals("Game.Fail.restart")) gameFailRestart = split[1];
                else if (split[0].Equals("Game.Fail.exit")) gameFailExit = split[1];
                else if (split[0].Equals("Game.Fail.save")) gameFailSave = split[1];

                else if (split[0].Equals("Game.FPS")) stadisticFps = split[1];

                else if (split[0].Equals("Song.Sort.by")) songSortBy = split[1];
                else if (split[0].Equals("Song.Sort.name")) songSortName = split[1];
                else if (split[0].Equals("Song.Sort.artist")) songSortArtist = split[1];
                else if (split[0].Equals("Song.Sort.album")) songSortAlbum = split[1];
                else if (split[0].Equals("Song.Sort.year")) songSortYear = split[1];
                else if (split[0].Equals("Song.Sort.length")) songSortLength = split[1];
                else if (split[0].Equals("Song.Sort.path")) songSortPath = split[1];
                else if (split[0].Equals("Song.Sort.genre")) songSortGenre = split[1];
                else if (split[0].Equals("Song.Sort.charter")) songSortCharter = split[1];
                else if (split[0].Equals("Song.Sort.diff")) songSortDiff = split[1];
                else if (split[0].Equals("Song.Sort.byInstrument")) songSortbyInstrument = split[1];
                else if (split[0].Equals("Song.Sort.insOn")) songSortinsOn = split[1];
                else if (split[0].Equals("Song.Sort.insOff")) songSortinsOff = split[1];
                else if (split[0].Equals("Song.count")) songCount = split[1];

                else if (split[0].Equals("Song.Difficulty.list")) songDiffList = split[1];
                else if (split[0].Equals("Song.Difficulty.easy")) songDiffEasy = split[1];
                else if (split[0].Equals("Song.Difficulty.medium")) songDiffMedium = split[1];
                else if (split[0].Equals("Song.Difficulty.hard")) songDiffHard = split[1];
                else if (split[0].Equals("Song.Difficulty.expert")) songDiffExpert = split[1];

                else if (split[0].Equals("Song.Instrument.guitar")) instrumentGuitar = split[1];
                else if (split[0].Equals("Song.Instrument.guitar2")) instrument2Guitar = split[1];
                else if (split[0].Equals("Song.Instrument.bass")) instrumentBass = split[1];
                else if (split[0].Equals("Song.Instrument.bass2")) instrument2Bass = split[1];
                else if (split[0].Equals("Song.Instrument.drums")) instrumentDrums = split[1];
                else if (split[0].Equals("Song.Instrument.keys")) instrumentKeys = split[1];
                else if (split[0].Equals("Song.Instrument.vocals")) instrumentVocals = split[1];
                else if (split[0].Equals("Song.Instrument.rhythm")) instrumentRhythm = split[1];
                else if (split[0].Equals("Song.Instrument.rhythm2")) instrument2Rhythm = split[1];
                else if (split[0].Equals("Song.Instrument.guitarghl")) instrumentGuitarGHL = split[1];
                else if (split[0].Equals("Song.Instrument.bassghl")) instrumentBassGHL = split[1];
                else if (split[0].Equals("Song.Instrument.proguitar")) instrumentProGuitar = split[1];
                else if (split[0].Equals("Song.Instrument.probass")) instrumentProBass = split[1];

                else if (split[0].Equals("Song.Records.list")) recordsList = split[1];
                else if (split[0].Equals("Song.Records.norecords")) recordsNorec = split[1];
                else if (split[0].Equals("Song.Records.song")) recordsSong = split[1];
                else if (split[0].Equals("Song.Records.loading")) recordsLoading = split[1];

                else if (split[0].Equals("Options.Button.green")) optionButtonGreen = split[1];
                else if (split[0].Equals("Options.Button.red")) optionButtonRed = split[1];
                else if (split[0].Equals("Options.Button.yellow")) optionButtonYellow = split[1];
                else if (split[0].Equals("Options.Button.blue")) optionButtonBlue = split[1];
                else if (split[0].Equals("Options.Button.orange")) optionButtonOrange = split[1];
                else if (split[0].Equals("Options.Button.open")) optionButtonOpen = split[1];
                else if (split[0].Equals("Options.Button.six")) optionButtonSix = split[1];
                else if (split[0].Equals("Options.Button.start")) optionButtonStart = split[1];
                else if (split[0].Equals("Options.Button.sp")) optionButtonSp = split[1];
                else if (split[0].Equals("Options.Button.up")) optionButtonUp = split[1];
                else if (split[0].Equals("Options.Button.down")) optionButtonDown = split[1];
                else if (split[0].Equals("Options.Button.whammy")) optionButtonWhammy = split[1];
                else if (split[0].Equals("Options.Button.axis")) optionButtonAxis = split[1];
                else if (split[0].Equals("Options.Button.lefty")) optionButtonLefty = split[1];
                else if (split[0].Equals("Options.Button.gamepadMode")) optionButtonGpMode = split[1];
                else if (split[0].Equals("Options.Button.keyboard")) optionButtonKeyboard = split[1];
                else if (split[0].Equals("Options.Button.gamepad")) optionButtonGamepad = split[1];
                else if (split[0].Equals("Options.Button.dz")) optionButtonDz = split[1];
                else if (split[0].Equals("Options.Button.player")) optionButtonPlayer = split[1];

                else if (split[0].Equals("Options.Button.instrument")) optionButtonInstrument = split[1];
                else if (split[0].Equals("Options.Button.5fret")) optionButton5Fret = split[1];
                else if (split[0].Equals("Options.Button.6fret")) optionButton6Fret = split[1];
                else if (split[0].Equals("Options.Button.drums")) optionButtonDrums = split[1];

                else if (split[0].Equals("Options.video")) optionVideo = split[1];
                else if (split[0].Equals("Options.audio")) optionAudio = split[1];
                else if (split[0].Equals("Options.keys")) optionKeys = split[1];
                else if (split[0].Equals("Options.gameplay")) optionGameplay = split[1];
                else if (split[0].Equals("Options.skin")) optionSkin = split[1];
                else if (split[0].Equals("Options.controller")) optionController = split[1];
                else if (split[0].Equals("Options.Video.unlimited")) optionVideoUnlimited = split[1];
                else if (split[0].Equals("Options.Video.fullscreen")) optionVideoFullscreen = split[1];
                else if (split[0].Equals("Options.Video.vsync")) optionVideoVSync = split[1];
                else if (split[0].Equals("Options.Video.fps")) optionVideoFPS = split[1];
                else if (split[0].Equals("Options.Video.resolution")) optionVideoResolution = split[1];
                else if (split[0].Equals("Options.Video.showfps")) optionVideoShowFPS = split[1];
                else if (split[0].Equals("Options.Video.extreme")) optionVideoExtreme = split[1];
                else if (split[0].Equals("Options.Video.tailQuality")) optionVideoTailQuality = split[1];
                else if (split[0].Equals("Options.Video.threadWarning")) optionVideoThreadWarning = split[1];
                else if (split[0].Equals("Options.Video.singleThread")) optionVideoSingleThread = split[1];
                else if (split[0].Equals("Options.Video.drawMenuFx")) optionVideoMenuFx = split[1];
                else if (split[0].Equals("Options.Audio.master")) optionAudioMaster = split[1];
                else if (split[0].Equals("Options.Audio.offset")) optionAudioOffset = split[1];
                else if (split[0].Equals("Options.Audio.fx")) optionAudioFx = split[1];
                else if (split[0].Equals("Options.Audio.mania")) optionAudioMania = split[1];
                else if (split[0].Equals("Options.Audio.music")) optionAudioMusic = split[1];
                else if (split[0].Equals("Options.Audio.pitch")) optionAudioPitch = split[1];
                else if (split[0].Equals("Options.Audio.fail")) optionAudioFail = split[1];
                else if (split[0].Equals("Options.Audio.engine")) optionAudioEngine = split[1];
                else if (split[0].Equals("Options.Audio.lagfree")) optionAudioLagfree = split[1];
                else if (split[0].Equals("Options.Audio.instant")) optionAudioInstant = split[1];
                //Options.Keys.increase
                else if (split[0].Equals("Options.Keys.increase")) optionKeysIncrease = split[1];
                else if (split[0].Equals("Options.Keys.decrease")) optionKeysDecrease = split[1];
                else if (split[0].Equals("Options.Keys.next")) optionKeysNext = split[1];
                else if (split[0].Equals("Options.Keys.previous")) optionKeysPrev = split[1];
                else if (split[0].Equals("Options.Keys.pause")) optionKeysPause = split[1];
                else if (split[0].Equals("Options.Gameplay.tailwave")) optionGameplayTailwave = split[1];
                else if (split[0].Equals("Options.Gameplay.drawspark")) optionGameplayDrawspark = split[1];
                else if (split[0].Equals("Options.Gameplay.scan")) optionGameplayScan = split[1];
                else if (split[0].Equals("Options.Gameplay.losemult")) optionGameplayLosemult = split[1];
                else if (split[0].Equals("Options.Gameplay.failanim")) optionGameplayFailanim = split[1];
                else if (split[0].Equals("Options.Gameplay.language")) optionGameplayLanguage = split[1];
                else if (split[0].Equals("Options.Gameplay.highway")) optionGameplayHighway = split[1];
                else if (split[0].Equals("Options.Skin.custom")) optionSkinCustomscan = split[1];
                else if (split[0].Equals("Options.Skin.skin")) optionSkinSkin = split[1];
                else if (split[0].Equals("Options.Skin.highway")) optionSkinHighway = split[1];

                else if (split[0].Equals("Options.restart")) optionRestart = split[1];

                else if (split[0].Equals("PopUp.epilepsy")) popupEpilepsy = split[1];
                else if (split[0].Equals("PopUp.instrument")) popupInstrument = split[1];

                //else if (split[0].Equals("")) { var = split[1]; }
            }
            MainMenu.changeText();
        }
    }
}
