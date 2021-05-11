using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSXMemoryCard
{
    public partial class Form1 : Form
    {
        public List<MemoryCard> memoryCardsList;

        //We will use these to copy data around
        private byte[] MemoryCardHeaderData;
        private byte[] SaveFileData;
        private ListView SelectedListView;
        private int SelectedListViewItem;
        private int PreviousSelectedListViewItem;
        private int PreviouslySelectedMemoryCard;

        public Form1()
        {
            InitializeComponent();

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Browse for PSX Memory Card File";
            openFileDialog.Filter = "PSX Memory Card|*.mcr";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //file opened
                byte[] MemoryCardFile = File.ReadAllBytes(openFileDialog.FileName);

                //Lets go ahead and make a new memory card
                if (memoryCardsList == null)
                    memoryCardsList = new List<MemoryCard>();
                memoryCardsList.Add(new MemoryCard(MemoryCardFile));

                //Set our tabbed list title
                if (tabControl1.TabCount <= 1 && memoryCardsList.Count <= 1)
                {
                    //this is our only tab, lets change the name
                    tabControl1.TabPages[tabControl1.TabCount - 1].Text = openFileDialog.SafeFileName;
                    infoStatusLabel.Text = openFileDialog.FileName;
                }
                else
                {
                    tabControl1.TabPages.Add(openFileDialog.SafeFileName);
                    tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                    infoStatusLabel.Text = openFileDialog.FileName;
                    //Add new listview...
                    CreateNewList(memoryCardsList[memoryCardsList.Count - 1]);
                    return;
                }

                
            }

            int index = 0;
            var imageList = new ImageList();
            foreach (SaveFile item in memoryCardsList[tabControl1.SelectedIndex].SaveFiles)
            {
                if (item.SaveFileName != null)
                {
                    if (item.IconFrames > 0)
                        imageList.Images.Add(index.ToString(), item.Icons[0]);
                    else
                        imageList.Images.Add(index.ToString(), new Bitmap(16, 16));

                    ListViewItem tempItem = new ListViewItem
                    {
                        ImageIndex = index,
                        Text = item.SaveFileName
                    };

                    //Handle linked slots
                    if(item.SaveFileName == "Linked Slot")
                        tempItem.ImageIndex = index - 1;

                    listView1.Items.Add(tempItem);
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(item.Region);
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(item.ProductCode);
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(item.Identifier);
                }
                else
                {
                    ListViewItem tempItem = new ListViewItem
                    {
                        Text = "Free Slot"
                    };
                    listView1.Items.Add(tempItem);
                }
                index++;
            }
            var picstoadd = 15 - imageList.Images.Count;
            if (imageList.Images.Count < 15)
            {
                for (int i = 0; i < picstoadd; i++)
                {
                    imageList.Images.Add(index.ToString(), new Bitmap(16, 16));
                }
            }

            listView1.SmallImageList = imageList;
            listView1.LargeImageList = imageList;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var senderList = (ListView)sender;
            var clickedItem = senderList.HitTest(e.Location).Item;

            if (clickedItem.SubItems[1].Text == "")
                return;

            Forms.SaveFileInfo saveFileInfoWindow = new Forms.SaveFileInfo(memoryCardsList[0], clickedItem.Index);
            saveFileInfoWindow.ShowDialog();
        }

        private void CreateNewList(MemoryCard mCard)
        {
            ListView tempList = new ListView
            {
                Size = new Size(495, 286),
                Location = new Point(6,6),
                View = View.Details
            };

            tempList.Columns.Add("Title", 247);
            tempList.Columns.Add("Region", 75);
            tempList.Columns.Add("Product Code", 84);
            tempList.Columns.Add("Identifier", 85);

            int index = 0;
            var imageList = new ImageList();
            foreach (SaveFile item in memoryCardsList[tabControl1.SelectedIndex].SaveFiles)
            {
                if (item.SaveFileName != null)
                {
                    if (item.IconFrames > 0)
                        imageList.Images.Add(index.ToString(), item.Icons[0]);
                    else
                        imageList.Images.Add(index.ToString(), new Bitmap(16, 16));

                    ListViewItem tempItem = new ListViewItem
                    {
                        ImageIndex = index,
                        Text = item.SaveFileName
                    };

                    //Handle linked slots
                    if (item.SaveFileName == "Linked Slot")
                        tempItem.ImageIndex = index - 1;

                    tempList.Items.Add(tempItem);
                    tempList.Items[tempList.Items.Count - 1].SubItems.Add(item.Region);
                    tempList.Items[tempList.Items.Count - 1].SubItems.Add(item.ProductCode);
                    tempList.Items[tempList.Items.Count - 1].SubItems.Add(item.Identifier);
                }
                else
                {
                    ListViewItem tempItem = new ListViewItem
                    {
                        Text = "Free Slot"
                    };

                    tempList.Items.Add(tempItem);
                }
                //Check if this data was deleted or not...
                if (shouldBeDisabled(item.SaveFileStatus))
                    tempList.Items[tempList.Items.Count - 1].ForeColor = Color.Gray;

                index++;
            }
            var picstoadd = 15 - imageList.Images.Count;
            if (imageList.Images.Count < 15)
            {
                for (int i = 0; i < picstoadd; i++)
                {
                    imageList.Images.Add(index.ToString(), new Bitmap(16, 16));
                }
            }

            tempList.SmallImageList = imageList;
            tempList.LargeImageList = imageList;
            tempList.MouseDoubleClick += new MouseEventHandler(listView_DoubleClick);
            tempList.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
            tempList.ContextMenuStrip = rightClickMenu;
            //tempList.ContextMenu = rightClickMenu;

            tabControl1.SelectedTab.Controls.Add(tempList);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

            SelectedListView = sender as ListView;
            if (SelectedListView.SelectedItems.Count <= 0)
                return;
            SelectedListViewItem = SelectedListView.SelectedItems[0].Index;
        }

        private void listView_DoubleClick(object sender, MouseEventArgs e)
        {
            var senderList = (ListView)sender;
            var clickedItem = senderList.HitTest(e.Location).Item;

            if (clickedItem.SubItems[1].Text == "")
                return;

            Forms.SaveFileInfo saveFileInfoWindow = new Forms.SaveFileInfo(memoryCardsList[tabControl1.SelectedIndex], clickedItem.Index);
            saveFileInfoWindow.ShowDialog();
        }

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var senderList = (ListView)sender;

            
            
            //var clickedItem = senderList.HitTest(e.Location).Item;

            //if (clickedItem.SubItems[1].Text == "")
                //return;

           // Forms.SaveFileInfo saveFileInfoWindow = new Forms.SaveFileInfo(memoryCardsList[tabControl1.SelectedIndex], clickedItem.Index);
            //saveFileInfoWindow.ShowDialog();
        }

        private void copySaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Console.WriteLine("Selected Item: {0} and memory card id {1}", SelectedListViewItem, tabControl1.SelectedIndex);

            MemoryCardHeaderData =  memoryCardsList[tabControl1.SelectedIndex].ReadMemoryCardHeaderByIndex(SelectedListViewItem);
            SaveFileData = memoryCardsList[tabControl1.SelectedIndex].ReadSaveFileByIndex(SelectedListViewItem);
            PreviousSelectedListViewItem = SelectedListView.Items[SelectedListViewItem].Index;
            PreviouslySelectedMemoryCard = tabControl1.SelectedIndex;
        }

        private void pasteSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedListView.Items[SelectedListViewItem].SubItems[0].Text == "Free Slot")
                DoPasteData();
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to overwrite this block?", "Confirm Overwrite Data", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    DoPasteData();
            }
        }

        private void DoPasteData()
        {
            memoryCardsList[tabControl1.SelectedIndex].ReplaceMemoryCardHeaderByIndex(SelectedListViewItem, MemoryCardHeaderData);
            memoryCardsList[tabControl1.SelectedIndex].ReplaceSaveFileByIndex(SelectedListViewItem, SaveFileData);
            memoryCardsList[tabControl1.SelectedIndex].SaveFiles[SelectedListViewItem] = memoryCardsList[tabControl1.SelectedIndex].GenerateNewSave(SelectedListViewItem);

            var test = memoryCardsList[tabControl1.SelectedIndex].SaveFiles[SelectedListViewItem];

            //Add the data to the listview
            SelectedListView.Items[SelectedListViewItem].SubItems[0].Text = test.SaveFileName;
            
            //Check if this save has a name
           // if(memoryCardsList[tabControl1.SelectedIndex].SaveFiles[SelectedListViewItem].)

            if(SelectedListView.Items[SelectedListViewItem].SubItems.Count > 1)
            {
                SelectedListView.Items[SelectedListViewItem].SubItems.Clear();
                SelectedListView.Items[SelectedListViewItem].SubItems[0].Text = test.SaveFileName;
            }
            SelectedListView.Items[SelectedListViewItem].SubItems.Add(test.Region);
            SelectedListView.Items[SelectedListViewItem].SubItems.Add(test.ProductCode);
            SelectedListView.Items[SelectedListViewItem].SubItems.Add(test.Identifier);

            //Set image index...

            if(PreviouslySelectedMemoryCard == tabControl1.SelectedIndex)
                SelectedListView.Items[SelectedListViewItem].ImageIndex = SelectedListView.Items[PreviousSelectedListViewItem].ImageIndex;

            else //We are on a different memory card, we need to replace the image index
            {
                var test2 = SelectedListView.SmallImageList.Images[SelectedListViewItem];
                if(memoryCardsList[tabControl1.SelectedIndex].SaveFiles[SelectedListViewItem].IconFrames > 0)
                    SelectedListView.SmallImageList.Images[SelectedListViewItem] = memoryCardsList[tabControl1.SelectedIndex].SaveFiles[SelectedListViewItem].Icons[0];
                SelectedListView.Items[SelectedListViewItem].ImageIndex = SelectedListViewItem;
            }

            //Check if this data was deleted or not...
            if (shouldBeDisabled(memoryCardsList[tabControl1.SelectedIndex].SaveFiles[SelectedListViewItem].SaveFileStatus))
                SelectedListView.Items[SelectedListViewItem].ForeColor = Color.Gray;

            //Remove buffer Data
            MemoryCardHeaderData = null;
            SaveFileData = null;
        }

        private bool shouldBeDisabled(SaveFile.SaveStatus status)
        {
            if (status == SaveFile.SaveStatus.InitialBlockDeleted ||
                status == SaveFile.SaveStatus.MiddleBlockDeleted ||
                status == SaveFile.SaveStatus.EndBlockDeleted)
                return true;
            return false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedListView = sender as ListView;
            if (SelectedListView.SelectedItems.Count <= 0)
                return;
            SelectedListViewItem = SelectedListView.SelectedItems[0].Index;
        }

        private void rightClickMenu_Opened(object sender, EventArgs e)
        {

        }

        private void rightClickMenu_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void rightClickMenu_VisibleChanged(object sender, EventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;

            if (SelectedListView.Items[SelectedListViewItem].Text == "Free Slot")
            {
                menu.Items[0].Enabled = false;
                menu.Items[2].Enabled = false;
                if(MemoryCardHeaderData == null)
                    menu.Items[3].Enabled = false;
                else
                    menu.Items[3].Enabled = true;
                menu.Items[5].Enabled = false;
                menu.Items[6].Enabled = false;
                menu.Items[10].Enabled = false;
            }
            else
            {
                menu.Items[0].Enabled = true;
                menu.Items[2].Enabled = true;
                if (MemoryCardHeaderData == null)
                    menu.Items[3].Enabled = false;
                else
                    menu.Items[3].Enabled = true;
                menu.Items[5].Enabled = true;
                menu.Items[6].Enabled = true;
                menu.Items[10].Enabled = true;
            }
        }
    }
}
