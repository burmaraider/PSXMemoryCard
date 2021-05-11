using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MemoryCard
{
    private byte[] _memoryCardData;
    public List<SaveFile> SaveFiles;

    private readonly int offsetToTableOfContents = 0x80;
    private readonly int saveFileBytesToRead = 8192;
    private readonly int offsetToFirstSave = 0x2000;

    public MemoryCard()
    {
        //Setup a new memory card data
        if(_memoryCardData == null)
        {
            _memoryCardData = new byte[0x20000];
        }
    }
    public MemoryCard(byte[] memoryCard)
    {
        //Setup a new memory card data
        if (_memoryCardData == null)
        {
            _memoryCardData = new byte[0x20000];
            _memoryCardData = memoryCard;

            SaveFiles = new List<SaveFile>();
            

            bool isContinuedSave = false;

            //Now lets setup save game files
            for (int i = 0; i < 15; i++)
            {
                byte[] saveStatus = new byte[1];
                byte[] completeSaveData = new byte[saveFileBytesToRead];
                byte[] nextBlock = new byte[2];
                byte[] region = new byte[2];
                byte[] productCode = new byte[10];
                byte[] identifier = new byte[8];
                Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents), saveStatus, 0, 1);
                Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 8, nextBlock, 0, nextBlock.Length);
                Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 10, region, 0, region.Length);
                Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 10 + region.Length, productCode, 0, productCode.Length);
                Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 10 + region.Length + productCode.Length, identifier, 0, identifier.Length);
                Array.Copy(_memoryCardData, offsetToFirstSave + (i * saveFileBytesToRead), completeSaveData, 0, saveFileBytesToRead);
                Console.WriteLine("Hex: {0:X}", (i * saveFileBytesToRead));
                SaveFile tempSave = new SaveFile(completeSaveData, region, productCode, identifier, nextBlock, isContinuedSave, saveStatus[0]);
                
                if (tempSave.BlocksUsed > 1)
                    isContinuedSave = true;
                else
                    isContinuedSave = false;

                SaveFiles.Add(tempSave);
            }
        }
    }
    private void ReadMemoryCardFile()
    {

    }




    public byte[] ReadMemoryCardHeaderByIndex(int index)
    {
        if (_memoryCardData == null)
            return null;

        byte[] dataArray = new byte[128];
        Array.Copy(_memoryCardData, offsetToTableOfContents + (index * 128), dataArray, 0, 128);
        return dataArray;
    }
    public byte[] ReadSaveFileByIndex(int index)
    {
        if (_memoryCardData == null)
            return null;

        byte[] dataArray = new byte[8192];
        Array.Copy(_memoryCardData, offsetToFirstSave + (index * saveFileBytesToRead), dataArray, 0, saveFileBytesToRead);
        return dataArray;
    }
    public bool ReplaceMemoryCardHeaderByIndex(int index, byte[] data)
    {
        if (_memoryCardData == null)
            return false;

        Array.Copy(data, 0, _memoryCardData, offsetToTableOfContents + (index * 128), data.Length);

        return true;
    }
    public bool ReplaceSaveFileByIndex(int index, byte[] data)
    {
        if (_memoryCardData == null)
            return false;

        Array.Copy(data, 0, _memoryCardData, offsetToFirstSave + (index * saveFileBytesToRead), data.Length);

        return true;
    }
    public SaveFile GenerateNewSave(int i)
    {
        //Setup a new memory card data
        if (_memoryCardData != null)
        {

            bool isContinuedSave = false;

            byte[] saveStatus = new byte[1];
            byte[] completeSaveData = new byte[saveFileBytesToRead];
            byte[] nextBlock = new byte[2];
            byte[] region = new byte[2];
            byte[] productCode = new byte[10];
            byte[] identifier = new byte[8];
            Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents), saveStatus, 0, 1);
            Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 8, nextBlock, 0, nextBlock.Length);
            Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 10, region, 0, region.Length);
            Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 10 + region.Length, productCode, 0, productCode.Length);
            Array.Copy(_memoryCardData, offsetToTableOfContents + (i * offsetToTableOfContents) + 10 + region.Length + productCode.Length, identifier, 0, identifier.Length);
            Array.Copy(_memoryCardData, offsetToFirstSave + (i * saveFileBytesToRead), completeSaveData, 0, saveFileBytesToRead);
            Console.WriteLine("Hex: {0:X}", (i * saveFileBytesToRead));
            SaveFile tempSave = new SaveFile(completeSaveData, region, productCode, identifier, nextBlock, isContinuedSave, saveStatus[0]);

            if (tempSave.BlocksUsed > 1)
                isContinuedSave = true;
            else
                isContinuedSave = false;

            return tempSave;
        }
        return null;
    }
    public byte CalculateXOR(byte[] inputData)
    {
        byte result = 0;
        foreach (byte item in inputData)
        {
            result ^= item;
        }
        return result;
    }
}