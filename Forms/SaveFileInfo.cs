using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSXMemoryCard.Forms
{
    public partial class SaveFileInfo : Form
    {
        private MemoryCard mCard;
        private int _index;
        private int currentIconIndex;
        private Timer timer1;
        public SaveFileInfo(MemoryCard memCard, int index)
        {
            InitializeComponent();
            mCard = memCard;
            _index = index;

            LoadImage();
            LoadInfo();

            //Setup timer
            timer1 = new Timer();
            timer1.Interval = 330;
            timer1.Tick += new EventHandler(animateIcon);
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadImage()
        {
            iconDisplay.BackColor = Color.Transparent;
            iconDisplay.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            iconDisplay.Image = mCard.SaveFiles[_index].Icons[0];
        }
        private void LoadInfo()
        {
            titleLabel.Text += "   " + mCard.SaveFiles[_index].SaveFileName;
            productLabel.Text += "   " + mCard.SaveFiles[_index].ProductCode;
            identifierLabel.Text += "   " + mCard.SaveFiles[_index].Identifier;
            regionLabel.Text += "   " + mCard.SaveFiles[_index].Region;
            saveSlotLabel.Text += "   " + mCard.SaveFiles[_index].BlocksUsed.ToString();

            //Compute size

            int sizeKB = (mCard.SaveFiles[_index].BlocksUsed * 8192) / 1024;
            sizeLabel.Text += " " + Convert.ToString(sizeKB) + "KB";
        }

        private void animateIcon(object sender, EventArgs e)
        {
            iconDisplay.Image = mCard.SaveFiles[_index].Icons[currentIconIndex % mCard.SaveFiles[_index].Icons.Length];
            currentIconIndex = currentIconIndex % mCard.SaveFiles[_index].Icons.Length + 1;
        }
    }
}
