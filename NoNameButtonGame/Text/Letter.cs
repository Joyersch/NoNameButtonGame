using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Text
{
    class Letter : GameObject
    {
        //Character character; /*unused*/
        public Letter(Vector2 Position, Vector2 Size, Character Character, Color CColor) {
            this.Position = Position;
            this.Size = Size;
            this.Texture = Globals.Content.Load<Texture2D>("font");
            DrawColor = CColor;
            FrameSize = new Vector2(8, 8);
            ChangeCharacter(Character);
            rec = new Rectangle((Position + FrameSpace.Location.ToVector2()).ToPoint(), (Size + FrameSpace.Size.ToVector2()).ToPoint());
        }
        public Rectangle FrameSpace;
        public void ChangeColor(Color Ccolor) {
            DrawColor = Ccolor;
        }
        public void ChangeCharacter(Character Cchar) {
            // character = Cchar; /*unused*/
            FrameSpace = GenerateRec(Cchar);
            ImageLocation = new Rectangle(new Point((int)Cchar % 5 * 8, (int)Cchar / 5 * 8 ), (FrameSize).ToPoint());
        }

        public override void Draw(SpriteBatch sp) {
            base.Draw(sp);
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            
        }
        private Rectangle GenerateRec(Character c) {
            switch (c) {
                case Character.c0:
                case Character.c1:
                case Character.c2:
                case Character.c4:
                case Character.c5:
                case Character.c6:
                case Character.c7:
                case Character.c8:
                case Character.c9:
                case Character.cB:
                case Character.cD:
                case Character.cE:
                case Character.cF:
                case Character.cG:
                case Character.cH:
                case Character.cN:
                case Character.cO:
                case Character.cP:
                case Character.cQ:
                case Character.cR:
                case Character.cS:
                case Character.cT:
                case Character.cU:
                case Character.cV:
                case Character.cX:
                case Character.cY:
                case Character.cZ:
                case Character.cQUESTION:
                    return new Rectangle(1, 0, 5, 7);
                case Character.cA:
                    return new Rectangle(1, 0, 6, 7);
                case Character.c3:
                case Character.cC:
                case Character.cK:
                case Character.cL:
                    return new Rectangle(1, 0, 4, 7);
                case Character.cI:
                case Character.cEXCLAMATION:
                    return new Rectangle(1, 0, 1, 7);
                case Character.cDOT:
                    return new Rectangle(0, 0, 0, 7);
                case Character.cJ:
                case Character.cOPENBRACKET:
                case Character.cCLOSEBRACKET:
                case Character.cSEMICOLON:
                    return new Rectangle(1, 0, 2, 7);
                case Character.cM:
                    return new Rectangle(2, 0, 4, 7);
                case Character.cW:
                    return new Rectangle(0, 0, 7, 7);
                case Character.cCROSSOUT:
                    return new Rectangle(0, 0, 8, 8);
                case Character.cSLASH:
                    return new Rectangle(2, 0, 4 ,8);
                case Character.cMINUS:
                    return new Rectangle(2, 0, 4, 1);
                case Character.cSMALLERAS:
                case Character.cBIGGERAS:
                    return new Rectangle(2, 0, 3, 5);
                case Character.cEQUAL:
                    return new Rectangle(1, 0, 6, 3);
                case Character.cSTAR:
                case Character.cPLUS:
                    return new Rectangle(1, 0, 5, 5);
                case Character.CDOWN:
                case Character.cUP:
                    return new Rectangle(0, 2, 8, 4);
                default:
                    return new Rectangle(0, 0, 8, 8);

            }
        }

        public enum Character
        {
            c0,
            c1,
            c2,
            c3,
            c4,
            c5,
            c6,
            c7,
            c8,
            c9,
            cA,
            cB,
            cC,
            cD,
            cE,
            cF,
            cG,
            cH,
            cI,
            cJ,
            cK,
            cL,
            cM,
            cN,
            cO,
            cP,
            cQ,
            cR,
            cS,
            cT,
            cU,
            cV,
            cW,
            cX,
            cY,
            cZ,
            cEXCLAMATION,
            cQUESTION,
            cSLASH,
            cMINUS,
            cSMALLERAS,
            cEQUAL,
            cBIGGERAS,
            cSTAR,
            cPLUS,
            cPERCENT,
            cOPENBRACKET,
            cCLOSEBRACKET,
            cSEMICOLON,
            cDOT,
            cSPACE,
            cCHECKMARK,
            cCROSSOUT,
            CDOWN,
            cUP,
            cFULL,
            cLINE,
            cDOUBLEDOTS,
            cKOMMA,
            cLEFT,
            cRIGHT,
            cPARENTHESES,
            cBACKSLASH,
        }
    }
}
