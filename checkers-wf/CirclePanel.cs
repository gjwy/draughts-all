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
                Image test = Resources.texture4;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(playerColor);
                System.Drawing.TextureBrush myBrush2 = new System.Drawing.TextureBrush(test);
                // draw a filled elipse with the brush
                // TODO: change this to an image
                graphic.FillEllipse(myBrush2, new Rectangle(0, 0,this.Height-1, this.Width-1));
               
                // cleanup the tools
                myBrush.Dispose();
                myBrush2.Dispose();
                graphic.Dispose();
            }
        }


        

    }
}
