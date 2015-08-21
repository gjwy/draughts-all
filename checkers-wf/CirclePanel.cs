using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using checkers_wf.Properties;
/* used the idea from http://stackoverflow.com/questions/3226136/turn-a-panel-into-a-circle-in-c-sharp-visual-studio-2010 
*/


namespace checkers_wf
{
    public class CirclePanel : Panel
    {

        private Color playerColor;
        private Image pieceTexture;

        // constructor takes the player (=color)
        public CirclePanel(string playerColor)
        {
            this.playerColor = Color.FromName(playerColor);
            // will need to add a method to change the texture to the king when it is upgraded
            this.pieceTexture = (playerColor == "red") ? Resources.draughts_man_red : Resources.draughts_man_white;
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            {
                
                // get the graphics obj used to paint the panel
                Graphics graphic = e.Graphics;
                // create a brush with playerColor colored paint
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(playerColor);
                
                System.Drawing.TextureBrush myBrush2 = new System.Drawing.TextureBrush(pieceTexture); // the bounding rectangle of the texture image
                // scale down the texture from 256,256
                myBrush2.Transform = new System.Drawing.Drawing2D.Matrix(50.0f / 256.0f, 0.0f, 0.0f, 50.0f / 256.0f, 0.0f, 0.0f);
                // draw a filled elipse with the brush
                // TODO: change this to an image
                graphic.FillEllipse(myBrush2, new Rectangle(0, 0,this.Height-1, this.Width-1)); // the bounding rectangle of the piecePanel
               
                // cleanup the tools
                myBrush.Dispose();
                myBrush2.Dispose();
                graphic.Dispose();
            }
        }


        

    }
}
