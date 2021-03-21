using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
    class MenuDraw_Options : MenuItem {
        public MenuDraw_Options() {
            ScanSkin();
            LoadOptions();
        }
        bool onSubOptionItem = false;
        bool optionSelected = false;
        int subOptionSelect = 0;
        int optionsSelect = 0;
        string[] highways;
        int highwaySelect;
        string[] skins;
        int skinSelect;
        int frameRate;
        int resSelect;
        int[][] resolutions;
        public override string RequestButton(GuitarButtons btn) {
            if (btn == GuitarButtons.green) {
                return "Select";
            } else if (btn == GuitarButtons.red) {
                return "Cancel";
            }
            return base.RequestButton(btn);
        }
        public override void SendKey(Key key) {
            if (optionsSelect == 2 && onSubOptionItem) {
                onSubOptionItem = false;
                keyRequest = false;
                if (subOptionSelect == 0) MainMenu.volumeUpKey = key;
                else if (subOptionSelect == 1) MainMenu.volumeDownKey = key;
                else if (subOptionSelect == 2) MainMenu.songPrevKey = key;
                else if (subOptionSelect == 3) MainMenu.songPauseResumeKey = key;
                else if (subOptionSelect == 4) MainMenu.songNextKey = key;
            }
        }
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (btn == GuitarButtons.up) {
                if (!optionSelected) {
                    optionsSelect--;
                    if (optionsSelect < 0)
                        optionsSelect = 0;
                } else {
                    if (onSubOptionItem) {
                        if (optionsSelect == 0) {
                            if (subOptionSelect == 2) {
                                frameRate += 5;
                                if (frameRate >= 1000)
                                    frameRate = 1000;
                            } else if (subOptionSelect == 3) {
                                resSelect++;
                                if (resSelect >= resolutions.Length)
                                    resSelect = resolutions.Length - 1;
                            }
                        } else if (optionsSelect == 1) {
                            if (subOptionSelect == 0) {
                                Audio.masterVolume += 0.05f;
                                Config.master = (int)Math.Round(Audio.masterVolume * 100);
                            }
                            if (subOptionSelect == 1)
                                Config.os += 5;
                            if (subOptionSelect == 2) {
                                Sound.fxVolume += 0.05f;
                                Config.fxvol = (int)Math.Round(Sound.fxVolume * 100);
                            }
                            if (subOptionSelect == 3) {
                                Sound.maniaVolume += 0.05f;
                                Config.maniavol = (int)Math.Round(Sound.maniaVolume * 100);
                            }
                            if (subOptionSelect == 4) {
                                Audio.musicVolume += 0.05f;
                                Config.musicvol = (int)Math.Round(Audio.musicVolume * 100);
                            }
                            Sound.setVolume();
                            Song.setVolume();
                        } else if (optionsSelect == 4) {
                            if (subOptionSelect == 1) {
                                skinSelect--;
                                if (skinSelect < 0)
                                    skinSelect = 0;
                            } else if (subOptionSelect >= 2 && subOptionSelect <= 5) {
                                highwaySelect--;
                                if (highwaySelect < 0)
                                    highwaySelect = 0;
                            }
                        }
                    } else {
                        subOptionSelect--;
                        if (subOptionSelect < 0)
                            subOptionSelect = 0;
                    }
                }
            } else if (btn == GuitarButtons.down) {
                if (!optionSelected) {
                    optionsSelect++;
                    if (optionsSelect > 4)
                        optionsSelect = 4;
                } else if (optionSelected) {
                    if (onSubOptionItem) {
                        if (optionsSelect == 0) {
                            if (subOptionSelect == 2) {
                                frameRate -= 5;
                                if (frameRate < 0)
                                    frameRate = 0;
                            } else if (subOptionSelect == 3) {
                                resSelect--;
                                if (resSelect < 0)
                                    resSelect = 0;
                            }
                        } else if (optionsSelect == 1) {
                            if (subOptionSelect == 0) {
                                Audio.masterVolume -= 0.05f;
                                Config.master = (int)Math.Round(Audio.masterVolume * 100);
                            }
                            if (subOptionSelect == 1)
                                Config.os -= 5;
                            if (subOptionSelect == 2) {
                                Sound.fxVolume -= 0.05f;
                                Config.fxvol = (int)Math.Round(Sound.fxVolume * 100);
                            }
                            if (subOptionSelect == 3) {
                                Sound.maniaVolume -= 0.05f;
                                Config.maniavol = (int)Math.Round(Sound.maniaVolume * 100);
                            }
                            if (subOptionSelect == 4) {
                                Audio.musicVolume -= 0.05f;
                                Config.musicvol = (int)Math.Round(Audio.musicVolume * 100);
                            }
                            Sound.setVolume();
                            Song.setVolume();
                        } else if (optionsSelect == 4) {
                            if (subOptionSelect == 1) {
                                skinSelect++;
                                if (skinSelect >= skins.Length)
                                    skinSelect = skins.Length - 1;
                            } else if (subOptionSelect >= 2 && subOptionSelect <= 5) {
                                highwaySelect++;
                                if (highwaySelect >= highways.Length)
                                    highwaySelect = highways.Length - 1;
                            }
                        }
                    } else {
                        subOptionSelect++;
                        int[] subOptionslength = new int[] { 9, 8, 5, 7, 6 };
                        if (subOptionSelect >= subOptionslength[optionsSelect])
                            subOptionSelect = subOptionslength[optionsSelect] - 1;
                    }
                }
            } else if (btn == GuitarButtons.green) {
                if (!optionSelected) {
                    optionSelected = true;
                    subOptionSelect = 0;
                    onSubOptionItem = false;
                } else if (optionSelected) {
                    if (onSubOptionItem) {
                        onSubOptionItem = false;
                        keyRequest = false;
                        saveOptionsValues();
                    } else {
                        if (optionsSelect == 0) {
                            if (subOptionSelect == 0) {
                                Config.fS = !Config.fS;
                                saveOptionsValues();
                            } else if (subOptionSelect == 1)
                                Config.vSync = !Config.vSync;
                            else if (subOptionSelect == 4)
                                Config.showFps = !Config.showFps;
                            else if (subOptionSelect == 5)
                                Config.badPC = !Config.badPC;
                            else if (subOptionSelect == 6) {
                                if (Config.tailQuality == 1)
                                    Config.tailQuality = 2;
                                else if (Config.tailQuality == 2)
                                    Config.tailQuality = 4;
                                else if (Config.tailQuality == 4)
                                    Config.tailQuality = 1;
                            } else if (subOptionSelect == 7)
                                Config.singleThread = !Config.singleThread;
                            else if (subOptionSelect == 8)
                                Config.menuFx = !Config.menuFx;
                            else if (subOptionSelect == 2 || subOptionSelect == 3)
                                onSubOptionItem = true;
                        } else if (optionsSelect == 1) {
                            if (subOptionSelect == 0 || subOptionSelect == 1 || subOptionSelect == 2 || subOptionSelect == 3 || subOptionSelect == 4)
                                onSubOptionItem = true;
                            if (subOptionSelect == 5)
                                Config.pitch = !Config.pitch;
                            if (subOptionSelect == 6)
                                Config.fpitch = !Config.fpitch;
                            if (subOptionSelect == 7)
                                Sound.ChangeEngine();
                        } else if (optionsSelect == 2) {
                            //subOptionSelect = i + 2;
                            onSubOptionItem = true;
                            keyRequest = true;
                        } else if (optionsSelect == 3) {
                            if (subOptionSelect == 0)
                                Config.wave = !Config.wave;
                            else if (subOptionSelect == 1)
                                Config.spark = !Config.spark;
                            else if (subOptionSelect == 2) {
                                //SongScan.ScanSongsThread(false);
                                SongScanner.ScanFolder();
                            } else if (subOptionSelect == 3)
                                Config.failanim = !Config.failanim;
                            else if (subOptionSelect == 4)
                                Config.fsanim = !Config.fsanim;
                            else if (subOptionSelect == 5) {
                                if (Language.language.Equals("en"))
                                    Language.language = "es";
                                else if (Language.language.Equals("es"))
                                    Language.language = "en";
                                else
                                    Language.language = "en";
                                Config.lang = Language.language;
                                Language.LoadLanguage();
                            } else if (subOptionSelect == 6)
                            Config.useghhw = !Config.useghhw;

                        } else if (optionsSelect == 4) {
                            if (subOptionSelect > 0)
                                onSubOptionItem = true;
                        }
                    }
                }
            } else if (btn == GuitarButtons.red) {
                if (!optionSelected) {
                    time = 0;
                    dying = true;
                    state = 2;
                    MenuDraw_Play item = new MenuDraw_Play(2);
                    item.state = 3;
                    MainMenu.menuItems.Add(item);
                    for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                        MenuItem item2 = MainMenu.menuItems[i];
                        if (item2 is MenuDraw_SongViewer) {
                            item2.state = 4;
                            item2.time = 0;
                        }
                    }
                    MainMenu.SaveChanges();
                } else if (optionSelected) {
                    if (onSubOptionItem) {
                        onSubOptionItem = false;
                        saveOptionsValues();
                    } else {
                        LoadOptions();
                        MainMenu.SaveChanges();
                        optionSelected = false;
                    }
                }
            } else
                press = false;
            return press;
        }
        public override void Update() {
            base.Update();
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX + posFade;
            outY = posY;
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float textHeight = (Draw.font.Height) * scalef;
            Vector2 vScale = new Vector2(scale * scalef, scale * scalef);

            Color itemSelected = GetColor(1f, 1f, 1f, .2f);
            Color itemNotSelected = GetColor(1f, 1f, 1f, 1f);
            Color notSelectedOpaque = GetColor(1f, .5f, .5f, .5f);

            float X = getX(-35); //MainMenu.getXCanvas(posX + fadeX +-35 + fadeX + posX);   ???
            float Y = getY(25);
            string[] optionsText = new string[] {
                Language.optionsVideo,
                Language.optionsAudio,
                Language.optionsKeys,
                Language.optionsGameplay,
                Language.optionsSkin
            };
            for (int i = 0; i < optionsText.Length; i++) {
                Draw.DrawString(optionsText[i], X, Y, vScale, optionsSelect == i ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
            }
            float defaultX = getX(5); //getXCanvas(posX + fadeX +5 + fadeX + posX); ???

            Y = getY(-20);
            X = defaultX + getX(-45);
            float mouseX = MainMenu.pmouseX;
            float mouseY = MainMenu.pmouseY;
            float tr = .4f;
            float textWidth = Draw.GetWidthString(Language.optionsController, vScale);
            if (onRect(mouseX, mouseY, X, -Y - textHeight * 1.1f, X + textWidth, -Y)) {
                if (MainMenu.mouseClicked) {
                    time = 0;
                    dying = true;
                    state = 1;
                    MenuDraw_Binds item = new MenuDraw_Binds();
                    item.state = 4;
                    MainMenu.menuItems.Add(item);
                    MainMenu.mouseClicked = false;
                }
                tr = .6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * tr * (tint.A / 255f));
            Draw.DrawString(Language.optionsController, X, Y, vScale, tr > 0.5f ? itemSelected : itemNotSelected, new Vector2(1, 1));

            X = defaultX;
            Y = getY(25);
            if (!optionSelected) {
                itemSelected = notSelectedOpaque;
                itemNotSelected = notSelectedOpaque;
            }
            //Various variable are this in MainMenu because it will be easy to save
            //like MainMenu.subOptionItemFrameRate or MainMenu.subOptionItemResolution or MainMenu.subOptionItemResolution[]
            if (optionsSelect == 0) {
                Draw.DrawString((Config.fS ? (char)(7) : (char)(8)) + Language.optionsVideoFullscreen, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.vSync ? (char)(7) : (char)(8)) + Language.optionsVideoVsync, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 2) {
                    Draw.DrawString(Language.optionsVideoFps +
                        " < " + (frameRate == 0 ? Language.optionsVideoUnlimited : frameRate.ToString()) + " > ", X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else
                    Draw.DrawString(Language.optionsVideoFps + (Game.Fps == 9999 ? Language.optionsVideoUnlimited : "" + Game.Fps), X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                if (Game.Fps == 9999) {
                    Y += textHeight * 0.7f;
                    Draw.DrawString(Language.optionsVideoThreadWarning, X, Y, vScale * 0.5f, itemNotSelected, Vector2.Zero);
                }
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 3) {
                    Draw.DrawString(Language.optionsVideoResolution +
                        (resSelect > 0 && resolutions.Length > 1 ? " < " : "") + resolutions[resSelect][0] + "x" + resolutions[resSelect][1] +
                        (resSelect < resolutions.Length - 1 ? " > " : "")
                        , X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else
                    Draw.DrawString(Language.optionsVideoResolution + Game.width + "x" + Game.height, X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.showFps ? (char)(7) : (char)(8)) + Language.optionsVideoShowfps, X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.badPC ? (char)(7) : (char)(8)) + Language.optionsVideoExtreme, X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(string.Format(Language.optionsVideoTailQuality, (Config.tailQuality == 1 ? "0.5x" : Config.tailQuality == 2 ? "1x" : "2x")), X, Y, vScale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight * 0.7f;
                Draw.DrawString(Language.optionsRestart, X, Y, vScale * 0.5f, itemNotSelected, Vector2.Zero);
                Y += textHeight;

                Draw.DrawString((Config.singleThread ? (char)(7) : (char)(8)) + Language.optionsVideoSingleThread, X, Y, vScale, subOptionSelect == 7 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight * 0.7f;
                Draw.DrawString(Language.optionsRestart, X, Y, vScale * 0.5f, itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.menuFx ? (char)(7) : (char)(8)) + Language.optionsVideoDrawMenuFx, X, Y, vScale, subOptionSelect == 8 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
            } else if (optionsSelect == 1) {
                if (onSubOptionItem && subOptionSelect == 0)
                    Draw.DrawString(Language.optionsAudioMaster + "< " + Math.Round(Audio.masterVolume * 100) + ">", X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionsAudioMaster + Math.Round(Audio.masterVolume * 100), X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 1)
                    Draw.DrawString(Language.optionsAudioOffset + "< " + MainGame.AudioOffset + ">", X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionsAudioOffset + MainGame.AudioOffset, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 2)
                    Draw.DrawString(Language.optionsAudioFx + "< " + Math.Round(Sound.fxVolume * 100) + ">", X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionsAudioFx + Math.Round(Sound.fxVolume * 100), X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 3)
                    Draw.DrawString(Language.optionsAudioMania + "< " + Math.Round(Sound.maniaVolume * 100) + ">", X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionsAudioMania + Math.Round(Sound.maniaVolume * 100), X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 4)
                    Draw.DrawString(Language.optionsAudioMusic + "< " + Math.Round(Audio.musicVolume * 100) + ">", X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionsAudioMusic + Math.Round(Audio.musicVolume * 100), X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.pitch ? (char)(7) : (char)(8)) + Language.optionsAudioPitch, X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.fpitch ? (char)(7) : (char)(8)) + Language.optionsAudioFail, X, Y, vScale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsAudioEngine + (Config.al ? Language.optionsAudioLagfree : Language.optionsAudioInstant), X, Y, vScale, subOptionSelect == 7 ? itemSelected : itemNotSelected, Vector2.Zero);
            } else if (optionsSelect == 2) {
                Draw.DrawString(Language.optionsKeysIncrease + MainMenu.volumeUpKey, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsKeysDecrease + MainMenu.volumeDownKey, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsKeysPrevious + MainMenu.songPrevKey, X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsKeysPause + MainMenu.songPauseResumeKey, X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsKeysNext + MainMenu.songNextKey, X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
            } else if (optionsSelect == 3) {
                Draw.DrawString((Config.wave ? (char)(7) : (char)(8)) + Language.optionsGameplayTailwave, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.spark ? (char)(7) : (char)(8)) + Language.optionsGameplayDrawspark, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsGameplayScan, X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.failanim ? (char)(7) : (char)(8)) + Language.optionsGameplayFailanim, X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.fsanim ? (char)(7) : (char)(8)) + Language.optionsGameplayFailanim, X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsGameplayLanguage + (Language.language == "en" ? "English" : Language.language == "es" ? "Español (Spanish)" : Language.language == "jp" ? "日本語 (Japanese)" : "???"), X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Config.useghhw ? (char)(7) : (char)(8)) + Language.optionsGameplayHighway, X, Y, vScale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
            } else if (optionsSelect == 4) {
                Draw.DrawString(Language.optionsSkinCustom, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionsSkinSkin, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                float plus = getX(5);
                Color colYellow = itemSelected;
                Color colWhite = itemNotSelected;
                if (onSubOptionItem && subOptionSelect == 1) {
                    for (int i = 0; i < skins.Length; i++) {
                        Draw.DrawString(skins[i], X + plus, Y, vScale * 0.8f, skinSelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionsSkinHighway, 1), X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 2) {
                    for (int i = 0; i < highways.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(highways[i]), X + plus, Y, vScale * 0.8f, highwaySelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionsSkinHighway, 2), X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 3) {
                    for (int i = 0; i < highways.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(highways[i]), X + plus, Y, vScale * 0.8f, highwaySelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionsSkinHighway, 3), X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 4) {
                    for (int i = 0; i < highways.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(highways[i]), X + plus, Y, vScale * 0.8f, highwaySelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionsSkinHighway, 4), X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 5) {
                    for (int i = 0; i < highways.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(highways[i]), X + plus, Y, vScale * 0.8f, highwaySelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
            }
        }
        public void ScanSkin() {
            string folder = "";
            folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Skins";
            string[] dirInfos;
            try {
                dirInfos = Directory.GetDirectories(folder, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            } catch { return; }
            for (int i = 0; i < dirInfos.Length; i++) {
                dirInfos[i] = dirInfos[i].Replace(folder + "\\", "");
            }
            skins = dirInfos;

            folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Highways";
            try {
                dirInfos = Directory.GetFiles(folder, "*.*");
            } catch { return; }
            for (int i = 0; i < dirInfos.Length; i++) {
                dirInfos[i] = Path.GetFileName(dirInfos[i]);
            }
            highways = dirInfos;
        }
        public void saveOptionsValues() {
            DisplayDevice di = DisplayDevice.Default;
            if (Config.fS) {
                Config.fSwidth = resolutions[resSelect][0];
                Config.fSheight = resolutions[resSelect][1];
                int w = Config.fSwidth;
                int h = Config.fSheight;
                di.ChangeResolution(di.SelectResolution(w, h, di.BitsPerPixel, di.RefreshRate));
                MainMenu.gameObj.Width = w;
                MainMenu.gameObj.Height = h;
            } else {
                DisplayDevice.Default.RestoreResolution();
            }

            Config.frameR = frameRate;
            GHtest1.Game.Fps = frameRate;
            if (frameRate == 0)
                GHtest1.Game.Fps = 9999;
            GHtest1.Game.vSync = true;
            if (!skins[skinSelect].Equals(Textures.skin)) {
                Textures.skin = skins[skinSelect];
                Config.skin = Textures.skin;
                //Textures.load();
                MainMenu.loadSkin = true;
            }
            if (subOptionSelect == 2)
                MainMenu.playerInfos[0].hw = highways[highwaySelect];
            if (subOptionSelect == 3)
                MainMenu.playerInfos[1].hw = highways[highwaySelect];
            if (subOptionSelect == 4)
                MainMenu.playerInfos[2].hw = highways[highwaySelect];
            if (subOptionSelect == 5)
                MainMenu.playerInfos[3].hw = highways[highwaySelect];
            //Textures.loadHighway();

            Textures.swpath1 = MainMenu.playerInfos[0].hw;
            Textures.swpath2 = MainMenu.playerInfos[1].hw;
            Textures.swpath3 = MainMenu.playerInfos[2].hw;
            Textures.swpath4 = MainMenu.playerInfos[3].hw;
            MainMenu.loadHw = true;
        }
        public void LoadOptions() {
            List<int[]> reslist = new List<int[]>();
            foreach (var d in DisplayDevice.Default.AvailableResolutions) {
                bool nofound = true;
                for (int i = 0; i < reslist.Count; i++) {
                    if (d.Width == reslist[i][0] && d.Height == reslist[i][1]) {
                        nofound = false;
                        break;
                    }
                }
                if (nofound) {
                    reslist.Add(new int[] { d.Width, d.Height });
                }
            }
            if (reslist.Count == 0) {
                reslist.Add(new int[] { 800, 600 });
            }
            resolutions = reslist.ToArray();
            resSelect = 0;
            for (int i = 0; i < reslist.Count; i++) {
                if (reslist[i][0] == Game.width && reslist[i][1] == Game.height) {
                    resSelect = i;
                    break;
                }
            }
            frameRate = (int)Game.Fps;
        }
    }
}
