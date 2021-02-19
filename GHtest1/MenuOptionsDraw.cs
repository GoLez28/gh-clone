using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
    class MenuDraw_options : MenuItem {
        public MenuDraw_options() { }
        float fadeX = 0;
        bool onSubOptionItem = false;
        bool optionSelected = false;
        int subOptionSelect = 0;
        int optionsSelect = 0;
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
                                MainMenu.subOptionItemFrameRate += 5;
                                if (MainMenu.subOptionItemFrameRate >= 1000)
                                    MainMenu.subOptionItemFrameRate = 1000;
                            } else if (subOptionSelect == 3) {
                                MainMenu.subOptionItemResolutionSelect--;
                                if (MainMenu.subOptionItemResolutionSelect < 0)
                                    MainMenu.subOptionItemResolutionSelect = 0;
                            }
                        } else if (optionsSelect == 1) {
                            if (subOptionSelect == 0) {
                                Audio.masterVolume += 0.05f;
                            }
                            if (subOptionSelect == 1)
                                MainGame.AudioOffset += 5;
                            if (subOptionSelect == 2) {
                                Sound.fxVolume += 0.05f;
                            }
                            if (subOptionSelect == 3) {
                                Sound.maniaVolume += 0.05f;
                            }
                            if (subOptionSelect == 4) {
                                Audio.musicVolume += 0.05f;
                            }
                            Sound.setVolume();
                            MainMenu.song.setVolume();
                        } else if (optionsSelect == 4) {
                            if (subOptionSelect == 1) {
                                MainMenu.subOptionItemSkinSelect--;
                                if (MainMenu.subOptionItemSkinSelect < 0)
                                    MainMenu.subOptionItemSkinSelect = 0;
                            } else if (subOptionSelect >= 2 && subOptionSelect <= 5) {
                                MainMenu.subOptionItemHwSelect--;
                                if (MainMenu.subOptionItemHwSelect < 0)
                                    MainMenu.subOptionItemHwSelect = 0;
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
                                MainMenu.subOptionItemFrameRate -= 5;
                                if (MainMenu.subOptionItemFrameRate < 0)
                                    MainMenu.subOptionItemFrameRate = 0;
                            } else if (subOptionSelect == 3) {
                                MainMenu.subOptionItemResolutionSelect++;
                                if (MainMenu.subOptionItemResolutionSelect >= MainMenu.subOptionItemResolution.Length)
                                    MainMenu.subOptionItemResolutionSelect = MainMenu.subOptionItemResolution.Length - 1;
                            }
                        } else if (optionsSelect == 1) {
                            if (subOptionSelect == 0) {
                                Audio.masterVolume -= 0.05f;
                            }
                            if (subOptionSelect == 1)
                                MainGame.AudioOffset -= 5;
                            if (subOptionSelect == 2) {
                                Sound.fxVolume -= 0.05f;
                            }
                            if (subOptionSelect == 3) {
                                Sound.maniaVolume -= 0.05f;
                            }
                            if (subOptionSelect == 4) {
                                Audio.musicVolume -= 0.05f;
                            }
                            Sound.setVolume();
                            MainMenu.song.setVolume();
                        } else if (optionsSelect == 4) {
                            if (subOptionSelect == 1) {
                                MainMenu.subOptionItemSkinSelect++;
                                if (MainMenu.subOptionItemSkinSelect >= MainMenu.subOptionItemSkin.Length)
                                    MainMenu.subOptionItemSkinSelect = MainMenu.subOptionItemSkin.Length - 1;
                            } else if (subOptionSelect >= 2 && subOptionSelect <= 5) {
                                MainMenu.subOptionItemHwSelect++;
                                if (MainMenu.subOptionItemHwSelect >= MainMenu.subOptionItemHw.Length)
                                    MainMenu.subOptionItemHwSelect = MainMenu.subOptionItemHw.Length - 1;
                            }
                        }
                    } else {
                        subOptionSelect++;
                        int[] subOptionslength = new int[] { 9, 8, 5, 7, 6};
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
                        MainMenu.saveOptionsValues();
                    } else {
                        if (optionsSelect == 0) {
                            if (subOptionSelect == 0) {
                                MainMenu.fullScreen = !MainMenu.fullScreen;
                                MainMenu.saveOptionsValues();
                                if (!MainMenu.fullScreen)
                                    DisplayDevice.Default.RestoreResolution();
                            } else if (subOptionSelect == 1)
                                MainMenu.vSync = !MainMenu.vSync;
                            else if (subOptionSelect == 4)
                                Draw.showFps = !Draw.showFps;
                            else if (subOptionSelect == 5)
                                MainGame.MyPCisShit = !MainGame.MyPCisShit;
                            else if (subOptionSelect == 6) {
                                if (Draw.tailSizeMult == 1)
                                    Draw.tailSizeMult = 2;
                                else if (Draw.tailSizeMult == 2)
                                    Draw.tailSizeMult = 4;
                                else if (Draw.tailSizeMult == 4)
                                    Draw.tailSizeMult = 1;
                            } else if (subOptionSelect == 7)
                                game.isSingleThreaded = !game.isSingleThreaded;
                            else if (subOptionSelect == 8)
                                MainMenu.drawMenuBackgroundFx = !MainMenu.drawMenuBackgroundFx;
                            else if (subOptionSelect == 2 || subOptionSelect == 3)
                                onSubOptionItem = true;
                        } else if (optionsSelect == 1) {
                            if (subOptionSelect == 0 || subOptionSelect == 1 || subOptionSelect == 2 || subOptionSelect == 3 || subOptionSelect == 4)
                                onSubOptionItem = true;
                            if (subOptionSelect == 5)
                                Audio.keepPitch = !Audio.keepPitch;
                            if (subOptionSelect == 6)
                                Audio.onFailPitch = !Audio.onFailPitch;
                            if (subOptionSelect == 7)
                                Sound.ChangeEngine();
                        } else if (optionsSelect == 2) {
                            //subOptionSelect = i + 2;
                            onSubOptionItem = true;
                            keyRequest = true;
                        } else if (optionsSelect == 3) {
                            if (subOptionSelect == 0)
                                Draw.tailWave = !Draw.tailWave;
                            else if (subOptionSelect == 1)
                                MainGame.drawSparks = !MainGame.drawSparks;
                            else if (subOptionSelect == 2)
                                SongScan.ScanSongsThread(false);
                            else if (subOptionSelect == 3)
                                MainGame.failanimation = !MainGame.failanimation;
                            else if (subOptionSelect == 4)
                                MainGame.songfailanimation = !MainGame.songfailanimation;
                            else if (subOptionSelect == 5) {
                                if (Language.language.Equals("en"))
                                    Language.language = "es";
                                else if (Language.language.Equals("es"))
                                    Language.language = "en";
                                else
                                    Language.language = "en";
                                Language.LoadLanguage();
                            } else if (subOptionSelect == 6)
                                MainGame.useGHhw = !MainGame.useGHhw;

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
                    MenuDraw_play item = new MenuDraw_play(2);
                    item.state = 3;
                    MainMenu.menuItems.Add(item);
                    for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                        MenuItem item2 = MainMenu.menuItems[i];
                        if (item2 is MenuDraw_SongViewer) {
                            item2.state = 4;
                                item2.time = 0 ;
                        }
                    }
                } else if (optionSelected) {
                    if (onSubOptionItem) {
                        onSubOptionItem = false;
                        MainMenu.saveOptionsValues();
                    } else {
                        MainMenu.LoadOptions();
                        MainMenu.SaveChanges();
                        optionSelected = false;
                    }
                }
            } else
                press = false;
            return press;
        }
        public override void Update() {
            if (state > 0) {
                float t = Ease.OutCirc(Ease.In((float)time, 200));
                t = state > 2 ? 1 - t : t;
                fadeX = t * (state % 2 == 0 ? -80 : 80);
                tint = Color.FromArgb((int)((1 - t) * 255), 255, 255, 255);
            }
            if (state > 0 && state < 3 && time > 400)
                died = true;
            if (state > 2 && time > 400)
                state = 0;
        }
        public override void Draw_() {
            outX = posX + fadeX;
            outY = posY;
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            float textHeight = (Draw.font.Height) * scalef;
            Vector2 vScale = new Vector2(scale * scalef, scale * scalef);

            Color itemSelected = GetColor(1f, 1f, 1f, .2f);
            Color itemNotSelected = GetColor(1f, 1f, 1f, 1f);
            Color notSelectedOpaque = GetColor(1f, .5f, .5f, .5f);

            float X = getX(-35); //MainMenu.getXCanvas(posX + fadeX +-35 + fadeX + posX);   ???
            float Y = getY(25);
            string[] optionsText = new string[] {
                Language.optionVideo,
                Language.optionAudio,
                Language.optionKeys,
                Language.optionGameplay,
                Language.optionSkin
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
            float textWidth = Draw.GetWidthString(Language.optionController, vScale);
            if (onRect(mouseX, mouseY, X, -Y - textHeight * 1.1f, X + textWidth, -Y)) {
                if (MainMenu.mouseClicked) {
                    time = 0;
                    dying = true;
                    state = 1;
                    MenuDraw_binds item = new MenuDraw_binds();
                    item.state = 4;
                    MainMenu.menuItems.Add(item);
                    MainMenu.mouseClicked = false;
                }
                tr = .6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * tr * (tint.A/255f));
            Draw.DrawString(Language.optionController, X, Y, vScale, tr > 0.5f ? itemSelected : itemNotSelected, new Vector2(1, 1));

            X = defaultX;
            Y = getY(25);
            if (!optionSelected) {
                itemSelected = notSelectedOpaque;
                itemNotSelected = notSelectedOpaque;
            }
            //Various variable are this in MainMenu because it will be easy to save
            //like MainMenu.subOptionItemFrameRate or MainMenu.subOptionItemResolution or MainMenu.subOptionItemResolution[]
            if (optionsSelect == 0) {
                Draw.DrawString((MainMenu.fullScreen ? (char)(7) : (char)(8)) + Language.optionVideoFullscreen, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((MainMenu.vSync ? (char)(7) : (char)(8)) + Language.optionVideoVSync, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 2) {
                    Draw.DrawString(Language.optionVideoFPS +
                        " < " + (MainMenu.subOptionItemFrameRate == 0 ? Language.optionVideoUnlimited : MainMenu.subOptionItemFrameRate.ToString()) + " > ", X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else
                    Draw.DrawString(Language.optionVideoFPS + (game.Fps == 9999 ? Language.optionVideoUnlimited : "" + game.Fps), X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                if (game.Fps == 9999) {
                    Y += textHeight * 0.7f;
                    Draw.DrawString(Language.optionVideoThreadWarning, X, Y, vScale * 0.5f, itemNotSelected, Vector2.Zero);
                }
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 3) {
                    Draw.DrawString(Language.optionVideoResolution +
                        (MainMenu.subOptionItemResolutionSelect > 0 && MainMenu.subOptionItemResolution.Length > 1 ? " < " : "") + MainMenu.subOptionItemResolution[MainMenu.subOptionItemResolutionSelect][0] + "x" + MainMenu.subOptionItemResolution[MainMenu.subOptionItemResolutionSelect][1] +
                        (MainMenu.subOptionItemResolutionSelect < MainMenu.subOptionItemResolution.Length - 1 ? " > " : "")
                        , X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else
                    Draw.DrawString(Language.optionVideoResolution + game.width + "x" + game.height, X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Draw.showFps ? (char)(7) : (char)(8)) + Language.optionVideoShowFPS, X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((MainGame.MyPCisShit ? (char)(7) : (char)(8)) + Language.optionVideoExtreme, X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(string.Format(Language.optionVideoTailQuality, (Draw.tailSizeMult == 1 ? "0.5x" : Draw.tailSizeMult == 2 ? "1x" : "2x")), X, Y, vScale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight * 0.7f;
                Draw.DrawString(Language.optionRestart, X, Y, vScale * 0.5f, itemNotSelected, Vector2.Zero);
                Y += textHeight;

                Draw.DrawString((game.isSingleThreaded ? (char)(7) : (char)(8)) + Language.optionVideoSingleThread, X, Y, vScale, subOptionSelect == 7 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight * 0.7f;
                Draw.DrawString(Language.optionRestart, X, Y, vScale * 0.5f, itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((MainMenu.drawMenuBackgroundFx ? (char)(7) : (char)(8)) + Language.optionVideoMenuFx, X, Y, vScale, subOptionSelect == 8 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
            } else if (optionsSelect == 1) {
                if (onSubOptionItem && subOptionSelect == 0)
                    Draw.DrawString(Language.optionAudioMaster + "< " + Math.Round(Audio.masterVolume * 100) + ">", X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionAudioMaster + Math.Round(Audio.masterVolume * 100), X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 1)
                    Draw.DrawString(Language.optionAudioOffset + "< " + MainGame.AudioOffset + ">", X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionAudioOffset + MainGame.AudioOffset, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 2)
                    Draw.DrawString(Language.optionAudioFx + "< " + Math.Round(Sound.fxVolume * 100) + ">", X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionAudioFx + Math.Round(Sound.fxVolume * 100), X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 3)
                    Draw.DrawString(Language.optionAudioMania + "< " + Math.Round(Sound.maniaVolume * 100) + ">", X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionAudioMania + Math.Round(Sound.maniaVolume * 100), X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 4)
                    Draw.DrawString(Language.optionAudioMusic + "< " + Math.Round(Audio.musicVolume * 100) + ">", X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                else
                    Draw.DrawString(Language.optionAudioMusic + Math.Round(Audio.musicVolume * 100), X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Audio.keepPitch ? (char)(7) : (char)(8)) + Language.optionAudioPitch, X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((Audio.onFailPitch ? (char)(7) : (char)(8)) + Language.optionAudioFail, X, Y, vScale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionAudioEngine + (Sound.OpenAlMode ? Language.optionAudioLagfree : Language.optionAudioInstant), X, Y, vScale, subOptionSelect == 7 ? itemSelected : itemNotSelected, Vector2.Zero);
            } else if (optionsSelect == 2) {
                Draw.DrawString(Language.optionKeysIncrease + MainMenu.volumeUpKey, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionKeysDecrease + MainMenu.volumeDownKey, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionKeysPrev + MainMenu.songPrevKey, X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionKeysPause + MainMenu.songPauseResumeKey, X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionKeysNext + MainMenu.songNextKey, X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
            } else if (optionsSelect == 3) {
                Draw.DrawString((Draw.tailWave ? (char)(7) : (char)(8)) + Language.optionGameplayTailwave, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((MainGame.drawSparks ? (char)(7) : (char)(8)) + Language.optionGameplayDrawspark, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionGameplayScan, X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((MainGame.failanimation ? (char)(7) : (char)(8)) + Language.optionGameplayFailanim, X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((MainGame.songfailanimation ? (char)(7) : (char)(8)) + Language.optionGameplayFailanim, X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionGameplayLanguage + (Language.language == "en" ? "English" : Language.language == "es" ? "Español (Spanish)" : Language.language == "jp" ? "日本語 (Japanese)" : "???"), X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString((MainGame.useGHhw ? (char)(7) : (char)(8)) + Language.optionGameplayHighway, X, Y, vScale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
            } else if (optionsSelect == 4) {
                Draw.DrawString(Language.optionSkinCustomscan, X, Y, vScale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                Draw.DrawString(Language.optionSkinSkin, X, Y, vScale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                float plus = getX(5);
                Color colYellow = itemSelected;
                Color colWhite = itemNotSelected;
                if (onSubOptionItem && subOptionSelect == 1) {
                    for (int i = 0; i < MainMenu.subOptionItemSkin.Length; i++) {
                        Draw.DrawString(MainMenu.subOptionItemSkin[i], X + plus, Y, vScale * 0.8f, MainMenu.subOptionItemSkinSelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionSkinHighway, 1), X, Y, vScale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 2) {
                    for (int i = 0; i < MainMenu.subOptionItemHw.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(MainMenu.subOptionItemHw[i]), X + plus, Y, vScale * 0.8f, MainMenu.subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionSkinHighway, 2), X, Y, vScale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 3) {
                    for (int i = 0; i < MainMenu.subOptionItemHw.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(MainMenu.subOptionItemHw[i]), X + plus, Y, vScale * 0.8f, MainMenu.subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionSkinHighway, 3), X, Y, vScale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 4) {
                    for (int i = 0; i < MainMenu.subOptionItemHw.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(MainMenu.subOptionItemHw[i]), X + plus, Y, vScale * 0.8f, MainMenu.subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
                Draw.DrawString(string.Format(Language.optionSkinHighway, 4), X, Y, vScale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                Y += textHeight;
                if (onSubOptionItem && subOptionSelect == 5) {
                    for (int i = 0; i < MainMenu.subOptionItemHw.Length; i++) {
                        Draw.DrawString(Path.GetFileNameWithoutExtension(MainMenu.subOptionItemHw[i]), X + plus, Y, vScale * 0.8f, MainMenu.subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                        Y += textHeight * 0.8f;
                    }
                }
            }
        }
    }
}
