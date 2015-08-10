using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
/* used the idea from http://stackoverflow.com/questions/3226136/turn-a-panel-into-a-circle-in-c-sharp-visual-studio-2010 
*/


namespace checkers_wf
{
    public class CirclePanel : Panel
    {

        private Color playerColor;

        // constructor takes the player (=color)
        public CirclePanel(string playerColor)
        {
            this.playerColor = Color.FromName(playerColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            {
                
                // get the graphics obj used to paint the panel
                Graphics graphic = e.Graphics;
                // create a brush with playerColor colored paint
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(playerColor);
                // draw a filled elipse with the brush
                // TODO: change this to an image
                graphic.FillEllipse(myBrush, new Rectangle(0, 0,this.Height-1, this.Width-1));
               
                // cleanup the tools
                myBrush.Dispose();
                graphic.Dispose();
            }
        }
        

    }
}
