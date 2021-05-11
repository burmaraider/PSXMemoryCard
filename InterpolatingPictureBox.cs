using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PSXMemoryCard
{
    public class InterpolatingPictureBox : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            eventArgs.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(eventArgs);
        }
    }
}
