using System;
using System.Collections.Generic;
using System.Drawing;

public class SaveFile
{
    private charConverter SJIS = new charConverter();
    private enum IconFrameCount
    {
        one = 17,
        two = 18,
        three = 19
    };

    public enum SaveStatus
    {
        InitialBlock = 81,
        MiddleBlock = 82,
        EndBlock = 83,
        InitialBlockDeleted = InitialBlock + 80,
        MiddleBlockDeleted = MiddleBlock + 80,
        EndBlockDeleted = EndBlock + 80,
        Formatted = 160
    };

    //Icon data
    private byte[] _saveFile;
    private IconFrameCount _iconFrameCount;
    private int _blocksUsed;
    private string _saveTitle;
    private string _productCode;
    private string _identifier;
    private string _region;
    private int _nextBlock;
    private SaveStatus _status;
    Color[] iconPalette = new Color[16];
    Bitmap[] iconBitmap = new Bitmap[3];

    public SaveFile(byte[] saveFileData, byte[] region,  byte[] productCode, byte[] identifier, byte[] nextBlock, bool isContinuedSave, byte status)
    {
        _saveFile = new byte[8192];
        _saveFile = saveFileData;
        _saveTitle = LoadSaveFileName();
        _blocksUsed = LoadBlocksUsed();
        _iconFrameCount = LoadIconsUsed();

        _region = SetRegion(region);
        _productCode = SetProductCode(productCode);
        _identifier = SetIdentifier(identifier);

        _nextBlock = SetNextBlock(nextBlock);

        _status = SetStatus(status);

        LoadPalette();
        LoadIcons(IconFrames);

        //Handle multi blocks
        if (isContinuedSave)
        {
            _saveTitle = "Linked Slot";
        }
    }

    private SaveStatus SetStatus(byte status)
    {
        SaveStatus saveStatus = (SaveStatus)status;
        return saveStatus;
    }

    private int SetNextBlock(byte[] nextBlock)
    {
        int result = BitConverter.ToInt16(nextBlock, 0);

        if (result == 0xFFFF)
            return 0;
        else
            return result;
    }
    private string SetRegion(byte[] region)
    {
       string temp = System.Text.Encoding.ASCII.GetString(region);

        if (temp == "BI") return "JAP";
        if (temp == "BA") return "USA";
        if (temp == "BE") return "EUR";
        return null;
    }
    private string SetProductCode(byte[] productCode)
    {
        return System.Text.Encoding.ASCII.GetString(productCode);
    }
    private string SetIdentifier(byte[] identifier)
    {
        return System.Text.Encoding.ASCII.GetString(identifier);
    }

    public SaveStatus SaveFileStatus
    {
        get { return _status; }
    }
    public string SaveFileName
    {
        get { return _saveTitle; }
    }
    public Bitmap[] Icons
    {
        get { return iconBitmap; }
    }
    public int BlocksUsed

    {
        get { return _blocksUsed; }
    }
    public string Region
    {
        get{ return _region; }
    }
    public string ProductCode
    {
        get { return _productCode; }
    }
    public string Identifier
    {
        get { return _identifier; }
    }
    public int IconFrames
    {
        get 
        {
            if (_iconFrameCount == IconFrameCount.one) return 1;
            if (_iconFrameCount == IconFrameCount.two) return 2;
            if (_iconFrameCount == IconFrameCount.three) return 3;
            return 0;
        }
    }

    private IconFrameCount LoadIconsUsed()
    {
        return (IconFrameCount)Convert.ToInt32(_saveFile[2]);
    }
    private int LoadBlocksUsed()
    {
        return Convert.ToInt32(_saveFile[3]);
    }
    private string LoadSaveFileName()
    {
        byte[] saveFileName = new byte[64];
        Array.Copy(_saveFile, 0x4, saveFileName, 0, 64);
        return SJIS.convertSJIStoASCII(saveFileName);
    }
    private void LoadPalette()
    {
        int redChannel = 0;
        int greenChannel = 0;
        int blueChannel = 0;
        int colorCounter = 0;

        //Clear existing data
        iconPalette = new Color[16];

        //Reset color counter
        colorCounter = 0;

        //Fetch two bytes at a time
        for (int byteCount = 0; byteCount < 32; byteCount += 2)
        {
            redChannel = (_saveFile[byteCount + 0x60] & 0x1F) << 3;
            greenChannel = ((_saveFile[byteCount + 0x60 + 1] & 0x3) << 6) | ((_saveFile[byteCount + 0x60] & 0xE0) >> 2);
            blueChannel = ((_saveFile[byteCount + 0x60 + 1] & 0x7C) << 1);

            //Get the color value
            iconPalette[colorCounter] = Color.FromArgb(redChannel, greenChannel, blueChannel);
            colorCounter++;
        }
    }
    private void LoadIcons(int iconCount)
    {
        int byteCount = 0;

        //Clear existing data
        iconBitmap = new Bitmap[iconCount];

        //Each save has 3 icons (some are data but those will not be shown)
        for (int iconNumber = 1; iconNumber < iconCount+1; iconNumber++)
        {
            iconBitmap[iconNumber-1] = new Bitmap(16, 16);
            byteCount = 0;

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x += 2)
                {
                    iconBitmap[iconNumber-1].SetPixel(x, y, iconPalette[_saveFile[byteCount + (0x80 * iconNumber)] & 0xF]);
                    iconBitmap[iconNumber-1].SetPixel(x + 1, y, iconPalette[_saveFile[byteCount + (0x80 * iconNumber)] >> 4]);
                    byteCount++;
                }
            }

            //Handle Transparency...
            iconBitmap[iconNumber - 1].MakeTransparent(Color.Black);
        }
    }

}