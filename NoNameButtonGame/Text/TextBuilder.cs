using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Text
{
    class TextBuilder : GameObject
    {
        Letter[] LetterAry;
        string textstr;
        Letter.Character[] Characters;
        Color[] LColor;
        int spacing;
        int Length;
        public int Spacing { get { return spacing; } set { spacing = value; CreateLetters(); } }
        public string Text { get { return textstr; } }

        public TextBuilder(string InitText,Vector2 Position, Vector2 LSize, Color[] LColor, int Spacing) {
            this.Size = LSize;
            spacing = Spacing;
            this.Position = Position;
            if (LColor == null) {
                LColor = new Color[InitText.Length];
                for (int i = 0; i < InitText.Length; i++) {
                    LColor[i] = Color.White;
                }
            }
            if (InitText.Length != LColor.Length) {
                throw new Exception("InitText and LColor length do not match!");
            }
            ChangeText(InitText, LColor); 

        }
        public void ChangePosition(Vector2 Pos) {
            Position = Pos;
            CreateLetters();

        }
        public void ChangeColor(Color[] LColor) {
            if (textstr.Length != LColor.Length) {
                throw new Exception("InitText and LColor length do not match!");
            } else {
                this.LColor = LColor;
                CreateLetters();
            }
        }
        public void ChangeText(string Text, Color[] colors) {

            char[] strArr = Text.ToCharArray();
            Characters = ConvertFromChar(strArr);
            LColor = colors;
            textstr = Text.ToUpper();
            CreateLetters();
        }
        public void ChangeText(string Text) {
            Color[] c = new Color[Text.Length];
            for (int i = 0; i < c.Length; i++) {
                c[i] = Color.White;
            }
            ChangeText(Text, c);
        }
        private void CreateLetters() {
            if (Characters.Length != LColor.Length) {
                throw new Exception("char and color length do not match!");
            } else {
                LetterAry = new Letter[Characters.Length];
                int CurrentStrLength = 0;
                for (int i = 0; i < Characters.Length; i++) {
                    LetterAry[i] = new Letter(new Vector2(Position.X + CurrentStrLength , Position.Y), this.Size, Characters[i], LColor[i]);
                    CurrentStrLength += LetterAry[i].FrameSpace.Width * ((int)Size.X / 8) 
                        +
                        LetterAry[i].FrameSpace.X * ((int)Size.X / 8)
                        +
                        (spacing + 1) * ((int)Size.X / 8);
                }
                Length = CurrentStrLength;
            }
        }
        private Letter.Character[] ConvertFromChar(char[] strArr) {
            List<Letter.Character> Letters = new List<Letter.Character>();
            for (int i = 0; i < strArr.Length; i++) {
                switch (strArr[i]) {
                    case '0':
                        Letters.Add(Letter.Character.c0);
                        break;
                    case '1':
                        Letters.Add(Letter.Character.c1);
                        break;
                    case '2':
                        Letters.Add(Letter.Character.c2);
                        break;
                    case '3':
                        Letters.Add(Letter.Character.c3);
                        break;
                    case '4':
                        Letters.Add(Letter.Character.c4);
                        break;
                    case '5':
                        Letters.Add(Letter.Character.c5);
                        break;
                    case '6':
                        Letters.Add(Letter.Character.c6);
                        break;
                    case '7':
                        Letters.Add(Letter.Character.c7);
                        break;
                    case '8':
                        Letters.Add(Letter.Character.c8);
                        break;
                    case '9':
                        Letters.Add(Letter.Character.c9);
                        break;
                    case 'a':
                    case 'A':
                        Letters.Add(Letter.Character.cA);
                        break;
                    case 'b':
                    case 'B':
                        Letters.Add(Letter.Character.cB);
                        break;
                    case 'c':
                    case 'C':
                        Letters.Add(Letter.Character.cC);
                        break;
                    case 'd':
                    case 'D':
                        Letters.Add(Letter.Character.cD);
                        break;
                    case 'e':
                    case 'E':
                        Letters.Add(Letter.Character.cE);
                        break;
                    case 'f':
                    case 'F':
                        Letters.Add(Letter.Character.cF);
                        break;
                    case 'g':
                    case 'G':
                        Letters.Add(Letter.Character.cG);
                        break;
                    case 'h':
                    case 'H':
                        Letters.Add(Letter.Character.cH);
                        break;
                    case 'i':
                    case 'I':
                        Letters.Add(Letter.Character.cI);
                        break;
                    case 'j':
                    case 'J':
                        Letters.Add(Letter.Character.cJ);
                        break;
                    case 'k':
                    case 'K':
                        Letters.Add(Letter.Character.cK);
                        break;
                    case 'l':
                    case 'L':
                        Letters.Add(Letter.Character.cL);
                        break;
                    case 'm':
                    case 'M':
                        Letters.Add(Letter.Character.cM);
                        break;
                    case 'n':
                    case 'N':
                        Letters.Add(Letter.Character.cN);
                        break;
                    case 'o':
                    case 'O':
                        Letters.Add(Letter.Character.cO);
                        break;
                    case 'p':
                    case 'P':
                        Letters.Add(Letter.Character.cP);
                        break;
                    case 'q':
                    case 'Q':
                        Letters.Add(Letter.Character.cQ);
                        break;
                    case 'r':
                    case 'R':
                        Letters.Add(Letter.Character.cR);
                        break;
                    case 's':
                    case 'S':
                        Letters.Add(Letter.Character.cS);
                        break;
                    case 't':
                    case 'T':
                        Letters.Add(Letter.Character.cT);
                        break;
                    case 'u':
                    case 'U':
                        Letters.Add(Letter.Character.cU);
                        break;
                    case 'v':
                    case 'V':
                        Letters.Add(Letter.Character.cV);
                        break;
                    case 'w':
                    case 'W':
                        Letters.Add(Letter.Character.cW);
                        break;
                    case 'x':
                    case 'X':
                        Letters.Add(Letter.Character.cX);
                        break;
                    case 'y':
                    case 'Y':
                        Letters.Add(Letter.Character.cY);
                        break;
                    case 'z':
                    case 'Z':
                        Letters.Add(Letter.Character.cZ);
                        break;
                    case '!':
                        Letters.Add(Letter.Character.cEXCLAMATION);
                        break;
                    case '?':
                        Letters.Add(Letter.Character.cQUESTION);
                        break;
                    case '/':
                        Letters.Add(Letter.Character.cSLASH);
                        break;
                    case '-':
                        Letters.Add(Letter.Character.cMINUS);
                        break;
                    case '<':
                        Letters.Add(Letter.Character.cSMALLERAS);
                        break;
                    case '=':
                        Letters.Add(Letter.Character.cEQUAL);
                        break;
                    case '>':
                        Letters.Add(Letter.Character.cBIGGERAS);
                        break;
                    case '*':
                        Letters.Add(Letter.Character.cSTAR);
                        break;
                    case '+':
                        Letters.Add(Letter.Character.cPLUS);
                        break;
                    case '%':
                        Letters.Add(Letter.Character.cPERCENT);
                        break;
                    case '(':
                        Letters.Add(Letter.Character.cOPENBRACKET);
                        break;
                    case ')':
                        Letters.Add(Letter.Character.cCLOSEBRACKET);
                        break;
                    case ';':
                        Letters.Add(Letter.Character.cSEMICOLON);
                        break;
                    case '.':
                        Letters.Add(Letter.Character.cDOT);
                        break;
                    case ' ':
                        Letters.Add(Letter.Character.cSPACE);
                        break;
                    case '✔':
                        Letters.Add(Letter.Character.cCHECKMARK);
                        break;
                    case '❌':
                        Letters.Add(Letter.Character.cCROSSOUT);
                        break;
                    case '⬇':
                        Letters.Add(Letter.Character.CDOWN);
                        break;
                    case '⬆':
                        Letters.Add(Letter.Character.cUP);
                        break;
                    case '⬜':
                        Letters.Add(Letter.Character.cFULL);
                        break;
                    case '_':
                        Letters.Add(Letter.Character.cLINE);
                        break;
                    case ':':
                        Letters.Add(Letter.Character.cDOUBLEDOTS);
                        break;
                    case ',':
                        Letters.Add(Letter.Character.cKOMMA);
                        break;
                    case '⬅':
                        Letters.Add(Letter.Character.cLEFT);
                        break;
                    case '➡':
                        Letters.Add(Letter.Character.cRIGHT);
                        break;
                    case '\"':
                        Letters.Add(Letter.Character.cPARENTHESES);
                        break;
                    case '\\':
                        Letters.Add(Letter.Character.cBACKSLASH);
                        break;
                    default:
                        throw new Exception("Unknown Character at char \'" + strArr[i] + "\' ");
                }
            }
            return Letters.ToArray();
        }

        public override void Update(GameTime gameTime) {
            //base.Update(gt);
            for (int i = 0; i < LetterAry.Length; i++) {
                LetterAry[i].Update(gameTime);
            }
            rec = new Rectangle(Position.ToPoint(), new Point(Length + (spacing + 1) * (LetterAry.Length - 1), (int)Size.Y));
        }

        public override void Draw(SpriteBatch spriteBatch) {
            //base.Draw(sp);
            for (int i = 0; i < LetterAry.Length; i++) {
                LetterAry[i].Draw(spriteBatch);
            }
        }
    }
}
