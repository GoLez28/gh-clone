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
    class MenuDraw_player : MenuItem {
        public MenuDraw_player(int p) {
            btnPriority = 1;
            renderPriority = 2;
            player = p;
        }
        bool creatingNewProfile = false;
        string newName = "";
        bool ready = false;
        bool altMenu = false;
        int select = 0;
        int select2 = 0;
        float menuPos = 0;
        public bool onOption = false;
        public bool hide = false;
        public override void SendKey(Key key) {
            if ((int)key >= (int)Key.A && (int)key <= (int)Key.Z) {
                newName += key;
            } else if ((int)key >= (int)Key.Number0 && (int)key <= (int)Key.Number9) {
                newName += (char)((int)'0' + ((int)key - (int)Key.Number0));
            } else if (key == Key.Space) {
                newName += " ";
            } else if (key == Key.BackSpace) {
                if (newName.Length > 0)
                    newName = newName.Substring(0, newName.Length - 1);
            } else if (key == Key.Enter) {
                creatingNewProfile = false;
                keyRequest = false;
                MainMenu.CreateProfile(newName);
                game.LoadProfiles();
                newName = "";
            } else if (key == Key.Escape) {
                creatingNewProfile = false;
                newName = "";
                keyRequest = false;
            }
            newName = newName.ToLower();
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
                            if (select > 11)
                                select = 11;
                        } else {
                            select2++;
                            if (select2 > 0)
                                select2 = 0;
                        }
                    }
                }
                if (btn == GuitarButtons.blue) {
                    if (!ready) {
                        game.LoadProfiles();
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
                            game.LoadProfiles();
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
                            MainMenu.playerInfos[p] = new PlayerInfo(p + 1, MainMenu.profilesPath[select - 1]);
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
                            } else if (select == 8) {
                                MainMenu.playerInfos[p].transform = !MainMenu.playerInfos[p].transform;
                            } else if (select == 9) {
                                MainMenu.playerInfos[p].autoSP = !MainMenu.playerInfos[p].autoSP;
                            } else if (select == 10) {
                                MainMenu.playerInfos[p].inputModifier += 1;
                                if (MainMenu.playerInfos[p].inputModifier > 4)
                                    MainMenu.playerInfos[p].inputModifier = 0;
                            } else if (select == 11) {
                                ready = false;
                                onOption = false;
                                MainMenu.playerInfos[p] = new PlayerInfo(p + 1);
                                if (Input.controllerIndex[p] != 2)
                                    Input.ignore = Input.controllerIndex[p];
                                Input.controllerIndex[p] = -1;
                            }
                            MainMenu.playerInfos[p].modMult = MainMenu.CalcModMult(p);
                        } else {
                            if (select2 == 0) {
                                if (Gameplay.pGameInfo[p].gameMode == GameModes.Normal)
                                    Gameplay.pGameInfo[p].gameMode = GameModes.Mania;
                                else if (Gameplay.pGameInfo[p].gameMode == GameModes.Mania)
                                    Gameplay.pGameInfo[p].gameMode = GameModes.New;
                                else if (Gameplay.pGameInfo[p].gameMode == GameModes.New)
                                    Gameplay.pGameInfo[p].gameMode = GameModes.Normal;
                            } else if (select2 == 1) {
                                if (MainMenu.playerInfos[p].instrument == Instrument.Fret5)
                                    MainMenu.playerInfos[p].instrument = Instrument.Drums;
                                else if (MainMenu.playerInfos[p].instrument == Instrument.Drums)
                                    MainMenu.playerInfos[p].instrument = Instrument.Fret5;
                            }
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
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            float textHeight = (Draw.font.Height) * scalef;
            Vector2 textScale = new Vector2(scale * scalef, scale * scalef);
            int p = player - 1;
            Vector2 vScale = new Vector2(scale * scalef, scale * scalef);
            float menuFadeOut = 1f; //temporarily
            float menuFadeOutTr = 1f; //temporarily
            if (onOption) {
                menuFadeOut = 0;
                menuPos += (0.0f - menuPos) * 0.3f;
            } else {
                menuPos += (1.0f - menuPos) * 0.3f;
            }
            float startPosX = getX(5, 0);
            float startPosY = getY(50) + +textHeight;
            float endPosX = getX(60, 0);
            float endPosY = getY(15);
            float posOff = menuPos * getY(40);
            float transparency = (float)(Math.Min(1.0, (1.0 - menuPos) * 20));
            Color colorTrasparent = GetColor(transparency, 1, 1, 1);
            float screenRatio = (float)game.height / Textures.background.Height;
            Vector2 textureScale = new Vector2(screenRatio, screenRatio);
            if (p == 0) {
                startPosY -= -posOff;
                endPosY -= -posOff;
                //Graphics.drawRect(getX(0, 0), getY(-50) - posOff, getX(-15, 3), getY(-10) - posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 0), getY(50) + posOff), Textures.menuOptioni.Xy * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
            } else if (p == 1) {
                startPosY -= -posOff;
                startPosX = getX(-60, 2);
                endPosX = getX(0, 2);
                endPosY -= -posOff;
                //Graphics.drawRect(getX(15, 3), getY(-50) - posOff, endPosX, getY(-10) - posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 2), getY(50) + posOff), Textures.menuOptioni.Xy * new Vector2(-1, 1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
            } else if (p == 2) {
                startPosY = getY(-15);
                endPosY = getY(-50);
                startPosY += -posOff;
                endPosY += -posOff;
                //Graphics.drawRect(getX(0, 0), getY(50) + posOff, getX(-15, 3), getY(10) + posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 0), getY(-50) - posOff), Textures.menuOptioni.Xy * new Vector2(1, -1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
            } else if (p == 3) {
                startPosX = getX(-60, 2);
                endPosX = getX(0, 2);
                startPosY = getY(-15);
                endPosY = getY(-50);
                startPosY += -posOff;
                endPosY += -posOff;
                //Graphics.drawRect(getX(15, 3), getY(50) + posOff, endPosX, getY(10) + posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                Graphics.Draw(Textures.menuOption, new Vector2(getX(0, 2), getY(-50) - posOff), Textures.menuOptioni.Xy * new Vector2(-1, -1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
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
                Color black = GetColor(tr * menuFadeOutTr *.9f, 0, 0, 0);
                Color transparent = GetColor(0, 0, 0, 0);
                if (p == 0) {
                    Graphics.drawPoly(getX(0, 0), getY(-50), getX(0, 0), getY(-20), getX(50, 0), getY(-20), getX(50, 0), getY(-50), black, transparent, transparent, transparent);
                    Draw.DrawString(controller, getX(5, 0), getY(45), vScale, col, Vector2.Zero);
                    if (MainMenu.playerProfileReady[p]) {
                        Draw.DrawString(MainMenu.playerInfos[p].playerName, getX(5, 0), getY(45) + textHeight, vScale, col, Vector2.Zero);
                    }
                } else if (p == 1) {
                    Graphics.drawPoly(getX(0, 2), getY(-50), getX(0, 2), getY(-20), getX(-50, 2), getY(-20), getX(-50, 2), getY(-50), black, transparent, transparent, transparent);
                    float stringWidth = Draw.GetWidthString(controller, vScale);
                    Draw.DrawString(controller, getX(-5, 2) - stringWidth, getY(45), vScale, col, Vector2.Zero);
                    if (MainMenu.playerProfileReady[p]) {
                        stringWidth = Draw.GetWidthString(MainMenu.playerInfos[p].playerName, vScale);
                        Draw.DrawString(MainMenu.playerInfos[p].playerName, getX(-5, 2) - stringWidth, getY(45) + textHeight, vScale, col, Vector2.Zero);
                    }
                } else if (p == 2) {
                    Graphics.drawPoly(getX(0, 0), getY(50), getX(0, 0), getY(20), getX(50, 0), getY(20), getX(50, 0), getY(50), black, transparent, transparent, transparent);
                    Draw.DrawString(controller, getX(5, 0), getY(-45), vScale, col, Vector2.Zero);
                    if (MainMenu.playerProfileReady[p]) {
                        Draw.DrawString(MainMenu.playerInfos[p].playerName, getX(5, 0), getY(-45) - textHeight, vScale, col, Vector2.Zero);
                    }
                } else if (p == 3) {
                    Graphics.drawPoly(getX(0, 2), getY(50), getX(0, 2), getY(20), getX(-50, 2), getY(20), getX(-50, 2), getY(50), black, transparent, transparent, transparent);
                    float stringWidth = Draw.GetWidthString(controller, vScale);
                    Draw.DrawString(controller, getX(-5, 2) - stringWidth, getY(-45), vScale, col, Vector2.Zero);
                    if (MainMenu.playerProfileReady[p]) {
                        stringWidth = Draw.GetWidthString(MainMenu.playerInfos[p].playerName, vScale);
                        Draw.DrawString(MainMenu.playerInfos[p].playerName, getX(-5, 2) - stringWidth, getY(-45) - textHeight, vScale, col, Vector2.Zero);
                    }
                }
            }
            if (menuPos < 0.95f) {
                Vector2 menuScale = vScale * 0.8f;
                float menuTextHeight = textHeight * 0.8f;
                float X = startPosX + 30;
                float Y = endPosY - menuTextHeight * 1.25f;
                string playerStr = String.Format(Language.menuModPlayer, p + 1);
                string playerName = MainMenu.playerInfos[p].playerName;
                playerName = playerName.Equals("__Guest__") ? playerStr : playerName;
                float nameLength = Draw.GetWidthString(playerName, menuScale * 2.5f);
                float namePos = endPosX - nameLength - 30;
                if (namePos < startPosX + 30)
                    namePos = startPosX + 30;
                Color lightgray = GetColor(1, .8f, .8f, .8f);
                Color gray = GetColor(1, .5f, .5f, .5f);
                Color lightgreen = GetColor(1, .55f, .95f, .55f);
                Color darkgreen = GetColor(1, 0, .4f, 0);
                Color darkred = GetColor(1, .55f, 0, 0);
                Draw.DrawString(playerName, namePos, Y, menuScale * 2.5f, GetColor(0.2f, 1, 1, 1), Vector2.Zero, 0, endPosX);
                X = startPosX;
                if (creatingNewProfile) {
                    Y = startPosY;
                    Draw.DrawString(Language.menuProfileCreateIn, X, Y, menuScale, lightgray, Vector2.Zero, 0, endPosX);
                    Y += menuTextHeight * 1.2f;
                    Draw.DrawString(newName, X, Y, menuScale, colWhite, Vector2.Zero, 0, endPosX);
                    Y += menuTextHeight * 1.2f;
                    Draw.DrawString(Language.menuProfileAccept, X, Y, menuScale, gray, Vector2.Zero, 0, endPosX);
                    Y += menuTextHeight;
                    Draw.DrawString(Language.menuProfileCancel, X, Y, menuScale, gray, Vector2.Zero, 0, endPosX);
                } else if (!ready) {
                    Y = startPosY;
                    Draw.DrawString(Language.menuProfileCreate, X, Y, menuScale, select == 0 ? lightgreen : darkgreen, Vector2.Zero, 0, endPosX);
                    for (int i = 1; i <= MainMenu.profilesName.Length; i++) {
                        Y = startPosY + menuTextHeight * i;
                        Draw.DrawString(MainMenu.profilesName[i - 1], X, Y, menuScale, select == i ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                    }
                    int ci = Input.controllerIndex[p];
                    if (ci > 0) {
                        Y += menuTextHeight * 1.2f;
                        Draw.DrawString("Btn 0: Green, Btn 1: Red", X, Y, menuScale * 0.7f, gray, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.DrawString("Btn 2: Down, Btn 3: Up", X, Y, menuScale * 0.7f, gray, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.DrawString("Btn Pressed: " + Input.lastGamePadButton, X, Y, menuScale * 0.7f, gray, Vector2.Zero, 0, endPosX);
                    } else {
                        Y += menuTextHeight * 1.2f;
                        Draw.DrawString("Number1: Accept", X, Y, menuScale * 0.7f, gray, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.DrawString("Number3: Delete", X, Y, menuScale * 0.7f, darkred, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight * 0.7f;
                        Draw.DrawString("Number4: Reload", X, Y, menuScale * 0.7f, gray, Vector2.Zero, 0, endPosX);
                    }
                } else {
                    Y = startPosY;
                    Draw.DrawString(Language.menuModMods, X, Y, menuScale, !altMenu ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                    X = (startPosX + endPosX) / 2;
                    Draw.DrawString(Language.menuModOptions, X, Y, menuScale, altMenu ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                    X = startPosX;
                    Y = startPosY + menuTextHeight * 1.5f;
                    int offset = select - 3;
                    if (offset < 0)
                        offset = 0;
                    if (offset > 5)
                        offset = 5;
                    if (!altMenu) {
                        X = endPosX + (startPosX - endPosX) / 5;
                        Draw.DrawString("x" + MainMenu.playerInfos[p].modMult.ToString("0.0"), X, Y, menuScale * 1.2f, MainMenu.playerInfos[p].modMult == 1f ? colWhite : MainMenu.playerInfos[p].modMult > 1f ? Color.PaleGreen : Color.Orange, Vector2.Zero, 0, endPosX);
                        X = startPosX;
                        Y -= menuTextHeight * offset;
                        if (offset <= 0) Draw.DrawString((select == 0 ? ">" : " ") + Language.menuModHard, X, Y, menuScale, MainMenu.playerInfos[p].HardRock ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset <= 1) Draw.DrawString((select == 1 ? ">" : " ") + Language.menuModHidden, X, Y, menuScale, MainMenu.playerInfos[p].Hidden == 1 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset <= 2) Draw.DrawString((select == 2 ? ">" : " ") + Language.menuModAuto, X, Y, menuScale, MainMenu.playerInfos[p].autoPlay ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset <= 3) Draw.DrawString((select == 3 ? ">" : " ") + Language.menuModEasy, X, Y, menuScale, MainMenu.playerInfos[p].Easy ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset <= 4) Draw.DrawString((select == 4 ? ">" : " ") + Language.menuModSpeed + ": " + Math.Round(MainMenu.playerInfos[p].gameplaySpeed * 100) + "%", X, Y, menuScale, Math.Round(MainMenu.playerInfos[p].gameplaySpeed * 100) != 100 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        Draw.DrawString((select == 5 ? ">" : " ") + String.Format(Language.menuModNotes, MainMenu.playerInfos[p].noteModifier == 0 ? Language.menuModNormal : MainMenu.playerInfos[p].noteModifier == 1 ? Language.menuModFlip : MainMenu.playerInfos[p].noteModifier == 2 ? Language.menuModShuffle : MainMenu.playerInfos[p].noteModifier == 3 ? Language.menuModRandom : "???"), X, Y, menuScale, MainMenu.playerInfos[p].noteModifier != 0 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        Draw.DrawString((select == 6 ? ">" : " ") + Language.menuModNofail, X, Y, menuScale, MainMenu.playerInfos[p].noFail ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset >= 1) Draw.DrawString((select == 7 ? ">" : " ") + Language.menuModPerformance, X, Y, menuScale, MainGame.performanceMode ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset >= 2) Draw.DrawString((select == 8 ? ">" : " ") + Language.menuModTransform, X, Y, menuScale, MainMenu.playerInfos[p].transform ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset >= 3) Draw.DrawString((select == 9 ? ">" : " ") + Language.menuModAutoSP, X, Y, menuScale, MainMenu.playerInfos[p].autoSP ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset >= 4) Draw.DrawString((select == 10 ? ">" : " ") + String.Format(Language.menuModInput, MainMenu.playerInfos[p].inputModifier == 0 ? Language.menuModInputNormal : MainMenu.playerInfos[p].inputModifier == 1 ? Language.menuModInputAllStrum : MainMenu.playerInfos[p].inputModifier == 2 ? Language.menuModInputAllTap : MainMenu.playerInfos[p].inputModifier == 3 ? Language.menuModInputStrum : MainMenu.playerInfos[p].inputModifier == 4 ? Language.menuModInputFretLess : "???"), X, Y, menuScale, MainMenu.playerInfos[p].inputModifier != 0 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                        if (offset >= 5) Draw.DrawString((select == 11 ? ">" : " ") + Language.menuModQuit, X, Y, menuScale, Color.Orange, Vector2.Zero, 0, endPosX);
                        Y += menuTextHeight;
                    } else {
                        Y += menuTextHeight;
                        Draw.DrawString((select2 == 0 ? ">" : " ") + string.Format(Language.menuOptionMode, Gameplay.pGameInfo[p].gameMode), X, Y, menuScale, colWhite, Vector2.Zero, 0, endPosX);
                    }
                }
            }
        }
    }
}
