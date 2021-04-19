﻿using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using System.IO;

namespace Upbeat {
    class MenuDraw_Player : MenuItem {
        public MenuDraw_Player(int p) {
            btnPriority = 2;
            renderPriority = 2;
            player = p;
        }
        bool creatingNewProfile = false;
        string newName = "";
        bool ready = false;
        bool altMenu = false;
        int select = 0;
        int select2 = 0;
        float menuPos = 1;
        public bool onOption = false;
        public bool hide = false;
        public override void SendChar(char c) {
            base.SendChar(c);
            newName += c;
        }
        public override void SendKey(Key key) {
            if (key == Key.BackSpace) {
                if (newName.Length > 0)
                    newName = newName.Substring(0, newName.Length - 1);
            } else if (key == Key.Enter) {
                creatingNewProfile = false;
                keyRequest = false;
                MainMenu.CreateProfile(newName);
                Game.LoadProfiles();
                newName = "";
            } else if (key == Key.Escape) {
                creatingNewProfile = false;
                newName = "";
                keyRequest = false;
            }
        }
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            int p = player - 1;
            if (btn == GuitarButtons.start) {
                onOption = !onOption;
                if (onOption) {
                    select = 0;
                    select2 = 0;
                    altMenu = false;
                } else {
                    if (!ready) {
                        Input.ignore = Input.controllerIndex[p];
                        Input.controllerIndex[p] = -1;
                    }
                }
            } else press = false;
            if (onOption) {
                press = true;
                if (btn == GuitarButtons.blue)
                    if (ready)
                        altMenu = !altMenu;
                if (btn == GuitarButtons.up) {
                    if (!altMenu) {
                        select--;
                        if (select < 0)
                            select = 0;
                    } else {
                        select2--;
                        if (select2 < 0)
                            select2 = 0;
                    }
                }
                if (btn == GuitarButtons.down) {
                    if (!ready) {
                        select++;
                        if (select >= MainMenu.profilesPath.Length + 1)
                            select = MainMenu.profilesPath.Length;
                    } else {
                        if (!altMenu) {
                            select++;
                            if (select > 12)
                                select = 12;
                        } else {
                            select2++;
                            if (select2 > 0)
                                select2 = 0;
                        }
                    }
                }
                if (btn == GuitarButtons.blue) {
                    if (!ready) {
                        Game.LoadProfiles();
                    }
                }
                if (btn == GuitarButtons.red) {
                    onOption = false;
                    if (!ready)
                        Input.ignore = Input.controllerIndex[p];
                }
                if (btn == GuitarButtons.yellow) {
                    if (!ready) {
                        if (select == 0) {
                        } else {
                            string path = MainMenu.profilesPath[select - 1];
                            Console.WriteLine("delete: " + path);
                            /*if (!playerProfileReady[0]) playerProfileSelect[0] = 0;
                            if (!playerProfileReady[1]) playerProfileSelect[1] = 0;
                            if (!playerProfileReady[2]) playerProfileSelect[2] = 0;
                            if (!playerProfileReady[3]) playerProfileSelect[3] = 0;*/
                            if (File.Exists(path)) {
                                File.Delete(path);
                            }
                            while (File.Exists(path)) ;
                            Game.LoadProfiles();
                            select--;
                        }
                    }
                }
                if (btn == GuitarButtons.green) {
                    if (!ready) {
                        if (select == 0) {
                            //onOption = false;
                            newName = "";
                            creatingNewProfile = true;
                            keyRequest = true;
                        } else {
                            MainMenu.playerInfos[p] = new PlayerInfo(p + 1, MainMenu.profilesPath[select - 1], false);
                            Console.WriteLine("path: " + MainMenu.profilesPath[select - 1]);
                            ready = true;
                            onOption = false;
                        }
                    } else {
                        if (!altMenu) {
                            if (select == 0) {
                                MainMenu.playerInfos[p].HardRock = !MainMenu.playerInfos[p].HardRock;
                                if (MainMenu.playerInfos[p].HardRock)
                                    MainMenu.playerInfos[p].Easy = false;
                            } else if (select == 1) {
                                if (MainMenu.playerInfos[p].Hidden == 0)
                                    MainMenu.playerInfos[p].Hidden = 1;
                                else if (MainMenu.playerInfos[p].Hidden == 1)
                                    MainMenu.playerInfos[p].Hidden = 0;
                            } else if (select == 2) {
                                MainMenu.playerInfos[p].autoPlay = !MainMenu.playerInfos[p].autoPlay;
                            } else if (select == 3) {
                                MainMenu.playerInfos[p].Easy = !MainMenu.playerInfos[p].Easy;
                                if (MainMenu.playerInfos[p].Easy)
                                    MainMenu.playerInfos[p].HardRock = false;
                            } else if (select == 4) {
                                MainMenu.playerInfos[0].gameplaySpeed += 0.10f;
                                if (MainMenu.playerInfos[0].gameplaySpeed > 2.05f)
                                    MainMenu.playerInfos[0].gameplaySpeed = 0.5f;
                            } else if (select == 5) {
                                MainMenu.playerInfos[p].noteModifier += 1;
                                if (MainMenu.playerInfos[p].noteModifier > 3)
                                    MainMenu.playerInfos[p].noteModifier = 0;
                            } else if (select == 6) {
                                MainMenu.playerInfos[p].noFail = !MainMenu.playerInfos[p].noFail;
                            } else if (select == 7) {
                                MainGame.performanceMode = !MainGame.performanceMode;
                                if (MainGame.performanceMode)
                                    MainMenu.playerInfos[p].modchartMode = Modchart.None;
                                else
                                    MainMenu.playerInfos[p].modchartMode = Modchart.Full;
                            } else if (select == 8) {
                                MainMenu.playerInfos[p].modchartMode++;
                                if ((int)MainMenu.playerInfos[p].modchartMode >= 6)
                                    MainMenu.playerInfos[p].modchartMode = 0;
                                if (MainMenu.playerInfos[p].modchartMode == Modchart.None)
                                    MainGame.performanceMode = true;
                                else if (MainMenu.playerInfos[p].modchartMode == Modchart.Full)
                                    MainGame.performanceMode = false;
                            } else if (select == 9) {
                                MainMenu.playerInfos[p].transform = !MainMenu.playerInfos[p].transform;
                            } else if (select == 10) {
                                MainMenu.playerInfos[p].autoSP = !MainMenu.playerInfos[p].autoSP;
                            } else if (select == 11) {
                                MainMenu.playerInfos[p].inputModifier += 1;
                                if (MainMenu.playerInfos[p].inputModifier > 4)
                                    MainMenu.playerInfos[p].inputModifier = 0;
                            } else if (select == 12) {
                                ready = false;
                                onOption = false;
                                MainMenu.playerInfos[p] = new PlayerInfo(p + 1, "Guest", true);
                                if (Input.controllerIndex[p] != 2)
                                    Input.ignore = Input.controllerIndex[p];
                                Input.controllerIndex[p] = -1;
                            }
                            MainMenu.playerInfos[p].modMult = MainMenu.CalcModMult(p);
                        } else {
                            //if (select2 == 0) {
                            //    if (Gameplay.GameplaypGameInfo[p].gameMode == GameModes.Normal)
                            //        Gameplay.GameplaypGameInfo[p].gameMode = GameModes.Mania;
                            //    else if (Gameplay.GameplaypGameInfo[p].gameMode == GameModes.Mania)
                            //        Gameplay.GameplaypGameInfo[p].gameMode = GameModes.New;
                            //    else if (Gameplay.GameplaypGameInfo[p].gameMode == GameModes.New)
                            //        Gameplay.GameplaypGameInfo[p].gameMode = GameModes.Normal;
                            //} else if (select2 == 1) {
                            //    if (MainMenu.playerInfos[p].instrument == Instrument.Fret5)
                            //        MainMenu.playerInfos[p].instrument = Instrument.Drums;
                            //    else if (MainMenu.playerInfos[p].instrument == Instrument.Drums)
                            //        MainMenu.playerInfos[p].instrument = Instrument.Fret5;
                            //}
                        }
                    }
                }
                if (btn == GuitarButtons.red) {
                    if (!ready) {
                        Input.controllerIndex[p] = -1;
                        onOption = false;
                    }
                }
            } else press = false;
            return press;
        }
        public override void Draw_() {
            base.Draw_();
            float fade = 1f;
            Color colYellow = GetColor(fade, 1f, 1f, 0.2f);
            Color colWhite = GetColor(fade, 1f, 1f, 1f); ;
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float squish = (float)Game.width / Game.height;
            if (squish > 1)
                squish = 1;
            scale = squish;
            float textHeight = (Draw.Methods.font.Height) * scalef;
            int p = player - 1;
            int getP = p;
            Vector2 vScale = new Vector2(scalef, scalef);
            float menuFadeOutTr = 1f; //temporarily
            if (onOption) {
                menuPos += (0.0f - menuPos) * 0.3f;
            } else {
                menuPos += (1.0f - menuPos) * 0.3f;
            }
            float startPosX = getX(3f, 0);
            float startPosY = getY(2f, 0) + +textHeight;
            float endPosX = getX(58f, 0);
            float endPosY = getY(-37f, 0);
            float posOff = menuPos * getY(40);
            float transparency = (float)(Math.Min(1.0, (1.0 - menuPos) * 20));
            Color colorTrasparent = GetColor(transparency, 1, 1, 1);
            Vector2 textureScale = new Vector2(scalef * 1.8f, scalef * 1.8f);
            Vector2 alignCorner = new Vector2(1, 1);
            if (p == 0) {
                startPosY -= -posOff;
                endPosY -= -posOff;
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 0), getY(0, 0) + posOff), Textures.menuOptioni.Xy * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
            } else if (p == 1) {
                startPosY -= -posOff;
                startPosX = getX(-62, 2);
                endPosX = getX(0, 2);
                endPosY -= -posOff;
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 2), getY(0, 0) + posOff), Textures.menuOptioni.Xy * new Vector2(-1, 1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
            } else if (p == 2) {
                startPosY = getY(37, 2);
                endPosY = getY(0, 2);
                startPosY += -posOff;
                endPosY += -posOff;
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 0), getY(0, 2) - posOff), Textures.menuOptioni.Xy * new Vector2(1, -1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
            } else if (p == 3) {
                startPosX = getX(-62, 2);
                endPosX = getX(0, 2);
                startPosY = getY(37, 2);
                endPosY = getY(0, 2);
                startPosY += -posOff;
                endPosY += -posOff;
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 2), getY(0, 2) - posOff), Textures.menuOptioni.Xy * new Vector2(-1, -1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
            }
            float tr = menuPos / 1f;
            if (tr > 0.05f && !hide) {
                int controllerindex = 0;
                controllerindex = Input.controllerIndex[p];
                if (p > 1 && controllerindex == -1)
                    return;
                string controller = "";
                if (controllerindex == -2)
                    controller = Language.menuKeyboard;
                else if (controllerindex == -1)
                    controller = Language.menuPressBtn;
                else if (controllerindex > 0)
                    controller = Language.menuController;
                Color col = GetColor(tr * menuFadeOutTr, 1, 1, 1);
                Color black = GetColor(tr * menuFadeOutTr * .9f, 0, 0, 0);
                Color transparent = GetColor(0, 0, 0, 0);
                if (p == 0) {
                    Graphics.drawPoly(getX(0, 0), getY(0, 2), getX(0, 0), getY(30, 2), getX(50, 0), getY(30, 2), getX(50, 0), getY(0, 2), black, transparent, transparent, transparent);
                    Draw.Methods.DrawString(controller, getX(3, 0), getY(-3, 0), vScale, col, alignCorner);
                    if (ready) {
                        Draw.Methods.DrawString(MainMenu.playerInfos[getP].playerName, getX(3, 0), getY(-3, 0) + textHeight, vScale, col, alignCorner);
                    }
                } else if (p == 1) {
                    Graphics.drawPoly(getX(0, 2), getY(0, 2), getX(0, 2), getY(30, 2), getX(-50, 2), getY(30, 2), getX(-50, 2), getY(0, 2), black, transparent, transparent, transparent);
                    float stringWidth = Draw.Methods.GetWidthString(controller, vScale);
                    Draw.Methods.DrawString(controller, getX(-3, 2) - stringWidth, getY(-3, 0), vScale, col, alignCorner);
                    if (ready) {
                        stringWidth = Draw.Methods.GetWidthString(MainMenu.playerInfos[getP].playerName, vScale);
                        Draw.Methods.DrawString(MainMenu.playerInfos[getP].playerName, getX(-3, 2) - stringWidth, getY(-3, 0) + textHeight, vScale, col, alignCorner);
                    }
                } else if (p == 2) {
                    /*Graphics.drawPoly(getX(0, 0), getY(50), getX(0, 0), getY(20), getX(50, 0), getY(20), getX(50, 0), getY(50), black, transparent, transparent, transparent);
                    Draw.DrawString(controller, getX(5, 0), getY(-45), vScale, col, alignCorner);
                    if (MainMenu.playerProfileReady[getP]) {
                        Draw.DrawString(MainMenu.playerInfos[getP].playerName, getX(5, 0), getY(-45) - textHeight, vScale, col, alignCorner);
                    }*/
                } else if (p == 3) {
                    /*Graphics.drawPoly(getX(0, 2), getY(50), getX(0, 2), getY(20), getX(-50, 2), getY(20), getX(-50, 2), getY(50), black, transparent, transparent, transparent);
                    float stringWidth = Draw.GetWidthString(controller, vScale);
                    Draw.DrawString(controller, getX(-5, 2) - stringWidth, getY(-45), vScale, col, alignCorner);
                    if (MainMenu.playerProfileReady[getP]) {
                        stringWidth = Draw.GetWidthString(MainMenu.playerInfos[getP].playerName, vScale);
                        Draw.DrawString(MainMenu.playerInfos[getP].playerName, getX(-5, 2) - stringWidth, getY(-45) - textHeight, vScale, col, alignCorner);
                    }*/
                }
            }
            if (menuPos < 0.95f) {
                Vector2 menuScale = vScale * 0.8f;
                float menuTextHeight = textHeight * 0.8f;
                float X = startPosX + 30;
                float Y = endPosY - menuTextHeight * 3f;
                string playerStr = String.Format(Language.menuModPlayer, p + 1);
                string playerName = MainMenu.playerInfos[getP].playerName;
                playerName = MainMenu.playerInfos[getP].validInfo ? playerName : playerStr;
                float nameLength = Draw.Methods.GetWidthString(playerName, menuScale * 2.5f);
                float namePos = endPosX - nameLength - 30;
                if (namePos < startPosX + 30)
                    namePos = startPosX + 30;
                Color lightgray = GetColor(1, .8f, .8f, .8f);
                Color gray = GetColor(1, .5f, .5f, .5f);
                Color lightgreen = GetColor(1, .55f, .95f, .55f);
                Color darkgreen = GetColor(1, 0, .4f, 0);
                Color darkred = GetColor(1, .55f, 0, 0);
                Draw.Methods.DrawString(playerName, namePos, Y, menuScale * 2.5f, GetColor(0.2f, 1, 1, 1), alignCorner, 0, endPosX);
                X = startPosX;
                if (creatingNewProfile) {
                    Y = startPosY;
                    Draw.Methods.DrawString(Language.menuProfileCreateIn, X, Y, menuScale, lightgray, alignCorner, 0, endPosX);
                    Y += menuTextHeight * 1.2f;
                    Draw.Methods.DrawString(newName, X, Y, menuScale, colWhite, alignCorner, 0, endPosX);
                    Y += menuTextHeight * 1.2f;
                    Draw.Methods.DrawString(Language.menuProfileAccept, X, Y, menuScale, gray, alignCorner, 0, endPosX);
                    Y += menuTextHeight;
                    Draw.Methods.DrawString(Language.menuProfileCancel, X, Y, menuScale, gray, alignCorner, 0, endPosX);
                } else if (!ready) {
                    Y = startPosY;
                    Draw.Methods.DrawString(Language.menuProfileCreate, X, Y, menuScale, select == 0 ? lightgreen : darkgreen, alignCorner, 0, endPosX);
                    for (int i = 1; i <= MainMenu.profilesName.Length; i++) {
                        Y = startPosY + menuTextHeight * i;
                        Draw.Methods.DrawString(MainMenu.profilesName[i - 1], X, Y, menuScale, select == i ? colYellow : colWhite, alignCorner, 0, endPosX);
                    }
                    int ci = Input.controllerIndex[getP];
                    if (ci > 0) {
                        Y += menuTextHeight * 1.2f;
                        Draw.Methods.DrawString("Btn 0: Green, Btn 1: Red", X, Y, menuScale * 0.7f, gray, alignCorner, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.Methods.DrawString("Btn 2: Down, Btn 3: Up", X, Y, menuScale * 0.7f, gray, alignCorner, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.Methods.DrawString("Btn Pressed: " + Input.lastGamePadButton, X, Y, menuScale * 0.7f, gray, alignCorner, 0, endPosX);
                    } else {
                        Y += menuTextHeight * 1.2f;
                        Draw.Methods.DrawString("Enter: Accept", X, Y, menuScale * 0.7f, gray, alignCorner, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.Methods.DrawString("Delete: Delete", X, Y, menuScale * 0.7f, darkred, alignCorner, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.Methods.DrawString("Insert: Reload", X, Y, menuScale * 0.7f, gray, alignCorner, 0, endPosX);
                    }
                } else {
                    Y = startPosY;
                    Draw.Methods.DrawString(Language.menuModMods, X, Y, menuScale, !altMenu ? colYellow : colWhite, alignCorner, 0, endPosX);
                    X = (startPosX + endPosX) / 2;
                    Draw.Methods.DrawString(Language.menuModOptions, X, Y, menuScale, altMenu ? colYellow : colWhite, alignCorner, 0, endPosX);
                    X = startPosX;
                    Y = startPosY + menuTextHeight * 1.5f;
                    int offset = select - 3;
                    if (offset < 0)
                        offset = 0;
                    if (offset > 6)
                        offset = 6;
                    if (!altMenu) {
                        X = endPosX + (startPosX - endPosX) / 5;
                        Draw.Methods.DrawString("x" + MainMenu.playerInfos[getP].modMult.ToString("0.0"), X, Y, menuScale * 1.2f, MainMenu.playerInfos[getP].modMult == 1f ? colWhite : MainMenu.playerInfos[getP].modMult > 1f ? Color.PaleGreen : Color.Orange, alignCorner, 0, endPosX);
                        X = startPosX;
                        Y -= menuTextHeight * offset;
                        SetParams(menuScale*0.9f, alignCorner, colYellow, colWhite, endPosX, X, getP);
                        PlayerInfo player = MainMenu.playerInfos[getP];
                        if (offset <= 0) DrawBool(select, 0, Language.menuModHard, player.HardRock, Y);
                        Y += menuTextHeight;
                        if (offset <= 1) DrawBool(select, 1, Language.menuModHidden, player.Hidden == 1, Y);
                        Y += menuTextHeight;
                        if (offset <= 2) DrawBool(select, 2, Language.menuModAuto, player.autoPlay, Y);
                        Y += menuTextHeight;
                        if (offset <= 3) DrawBool(select, 3, Language.menuModEasy, player.Easy, Y);
                        Y += menuTextHeight;
                        if (offset <= 4) DrawBool(select, 4, Language.menuModSpeed + ": " + Math.Round(player.gameplaySpeed * 100) + "%", Math.Round(player.gameplaySpeed * 100) != 100, Y);
                        Y += menuTextHeight;
                        if (offset <= 5) DrawList(select, 5, Language.menuModNotes, player.noteModifier != 0, Y, player.noteModifier, new string[] {
                            Language.menuModNotesNormal,
                            Language.menuModNotesFlip,
                            Language.menuModNotesShuffle,
                            Language.menuModNotesRandom
                        });
                        Y += menuTextHeight;
                        DrawBool(select, 6, Language.menuModNofail, MainMenu.playerInfos[getP].noFail, Y);
                        Y += menuTextHeight;
                        if (offset >= 1) DrawBool(select, 7, Language.menuModPerformance, MainGame.performanceMode, Y);
                        Y += menuTextHeight;
                        if (offset >= 2) DrawList(select, 8, "Modchart: {0}", player.modchartMode != 0, Y, (int)player.modchartMode, new string[] {
                            //Full, WoHighway, InfoTargets, Info, Targets, None
                            "Full",
                            "Without highway",
                            "Info and Targets",
                            "Info",
                            "Targets",
                            "None"
                        });
                        Y += menuTextHeight;
                        if (offset >= 3) DrawBool(select, 9, Language.menuModTransform, player.transform, Y);
                        Y += menuTextHeight;
                        if (offset >= 4) DrawBool(select, 10, Language.menuModAutoSP, player.autoSP, Y);
                        Y += menuTextHeight;
                        if (offset >= 5) DrawList(select, 11, Language.menuModInput, player.inputModifier != 0, Y, player.inputModifier, new string[] {
                            Language.menuModInNormal,
                            Language.menuModInAllstrum,
                            Language.menuModInAlltap,
                            Language.menuModInStrum,
                            Language.menuModInFretless
                        });
                        Y += menuTextHeight;
                        if (offset >= 6) Draw.Methods.DrawString((select == 12 ? ">" : " ") + Language.menuModQuit, X, Y, menuScale, Color.Orange, alignCorner, 0, endPosX);
                    } else {
                        Y += menuTextHeight;
                        Draw.Methods.DrawString((select2 == 0 ? ">" : " ") + string.Format(Language.menuOptionMode, Gameplay.Methods.pGameInfo[getP].gameMode), X, Y, menuScale, colWhite, alignCorner, 0, endPosX);
                    }
                }
            }
        }
        Vector2 optionScale = Vector2.One;
        Vector2 optionAlign = Vector2.Zero;
        Color optionColorSelect = Color.Yellow;
        Color optionColorNormal = Color.White;
        float optionEndX = 0;
        float optionX = 0;
        void SetParams(Vector2 scale, Vector2 align, Color selected, Color normal, float end, float X, float player) {
            optionScale = scale;
            optionAlign = align;
            optionColorSelect = selected;
            optionColorNormal = normal;
            optionEndX = end;
            optionX = X;
        }
        void DrawBool(int select, int index, string text, bool input, float Y) {
            Draw.Methods.DrawString((select == index ? ">" : " ") + text, optionX, Y, optionScale, input ? optionColorSelect : optionColorNormal, optionAlign, 0, optionEndX);
        }
        void DrawList(int select, int index, string text, bool input, float Y, int inputSelect, string[] list) {
            string result = "???";
            if (inputSelect < list.Length)
                result = list[inputSelect];
            Draw.Methods.DrawString((select == index ? ">" : " ") + String.Format(text, result), optionX, Y, optionScale, input ? optionColorSelect : optionColorNormal, optionAlign, 0, optionEndX);
        }
    }
}
