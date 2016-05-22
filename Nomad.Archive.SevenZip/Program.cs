using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.COM;
using Nomad.Archive.SevenZip;

namespace SevenZip
{
  class Program
  {
    private static void ShowHelp()
    {
      Console.WriteLine("SevenZip");
      Console.WriteLine("SevenZip l {ArchiveName}");
      Console.WriteLine("SevenZip e {ArchiveName} {FileNumber}");
      Console.WriteLine("SevenZip p {ArchiveName} {FilePath FilePath ...}");
    }

    static void Main(string[] args)
    {
      if (args.Length < 2)
      {
        ShowHelp();
        return;
      }

      try
      {
        switch (args[0])
        {
          case "l":
            ListOrExtract(args[1], false, 0xFFFFFFFF);
            break;
          case "e":
            uint FileNumber;

            if ((args.Length < 3) || !uint.TryParse(args[2], out FileNumber))
            {
              ShowHelp();
              return;
            }

            ListOrExtract(args[1], true, FileNumber);
            break;
          case "p":
            if (args.Length < 3)
            {
              ShowHelp();
              return;
            }

            List<string> FilePathList = new List<string>();
            for (int I = 2; I < args.Length; I++)
              FilePathList.Add(args[I]);

            Pack(args[1], FilePathList);
            break;
          default:
            ShowHelp();
            return;
        }

      }
      catch (Exception e)
      {
        Console.Write("Error: ");
        Console.WriteLine(e.Message);
      }
    }

    private static void ListOrExtract(string archiveName, bool extract, uint fileNumber)
    {
      using (SevenZipFormat Format = new SevenZipFormat(SevenZipDllPath))
      {
        IInArchive Archive = Format.CreateInArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.Zip));
        if (Archive == null)
        {
          ShowHelp();
          return;
        }

        try
        {
          using (InStreamWrapper ArchiveStream = new InStreamWrapper(File.OpenRead(archiveName)))
          {
            IArchiveOpenCallback OpenCallback = new ArchiveOpenCallback();

            // 32k CheckPos is not enough for some 7z archive formats
            ulong CheckPos = 128 * 1024;
            if (Archive.Open(ArchiveStream, ref CheckPos, OpenCallback) != 0)
              ShowHelp();

            Console.Write("Archive: ");
            Console.WriteLine(archiveName);

            if (extract)
            {
              PropVariant Name = new PropVariant();
              Archive.GetProperty(fileNumber, ItemPropId.kpidPath, ref Name);
              string FileName = (string)Name.GetObject();

              Console.Write("Extracting: ");
              Console.Write(FileName);
              Console.Write(' ');

              Archive.Extract(new uint[] { fileNumber }, 1, 0, new ArchiveExtractCallback(fileNumber, FileName));
            }
            else
            {
              Console.WriteLine("List:");
              uint Count = Archive.GetNumberOfItems();
              for (uint I = 0; I < Count; I++)
              {
                PropVariant Name = new PropVariant();
                Archive.GetProperty(I, ItemPropId.kpidPath, ref Name);
                Console.Write(I);
                Console.Write(' ');
                Console.WriteLine(Name.GetObject());
              }
            }
          }
        }
        finally
        {
          Marshal.ReleaseComObject(Archive);
        }
      }
    }

    private static void Pack(string archiveName, ICollection<string> filePathList)
    {
      using (SevenZipFormat Format = new SevenZipFormat(SevenZipDllPath))
      {
        IOutArchive Archive = Format.CreateOutArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.Zip));
        if (Archive == null)
        {
          ShowHelp();
          return;
        }

        List<FileInfo> FileList = new List<FileInfo>(filePathList.Count);
        foreach (string NextFilePath in filePathList)
          FileList.Add(new FileInfo(NextFilePath));

        try
        {
          using (OutStreamWrapper ArchiveStream = new OutStreamWrapper(File.OpenWrite(archiveName)))
          {
            Console.Write("Archive: ");
            Console.WriteLine(archiveName);

            Archive.UpdateItems(ArchiveStream, FileList.Count, new ArchiveUpdateCallback(FileList));
          }
        }
        finally
        {
          Marshal.ReleaseComObject(Archive);
        }
      }
    }

    private static string SevenZipDllPath
    {
      get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "7z.dll"); }
    }

    // Some 7z formats require this empty callback (some not)
    private class ArchiveOpenCallback : IArchiveOpenCallback
    {
      public void SetTotal(IntPtr files, IntPtr bytes)
      {
      }

      public void SetCompleted(IntPtr files, IntPtr bytes)
      {
      }
    }

    private class ArchiveExtractCallback : IProgress, IArchiveExtractCallback
    {
      private uint FileNumber;
      private string FileName;
      private OutStreamWrapper FileStream;

      public ArchiveExtractCallback(uint fileNumber, string fileName)
      {
        this.FileNumber = fileNumber;
        this.FileName = fileName;
      }

      #region IProgress Members

      public void SetTotal(ulong total)
      {
      }

      public void SetCompleted(ref ulong completeValue)
      {
      }

      #endregion

      #region IArchiveExtractCallback Members

      public int GetStream(uint index, out ISequentialOutStream outStream, AskMode askExtractMode)
      {
        if ((index == FileNumber) && (askExtractMode == AskMode.kExtract))
        {
          string FileDir = Path.GetDirectoryName(FileName);
          if (!string.IsNullOrEmpty(FileDir))
            Directory.CreateDirectory(FileDir);
          FileStream = new OutStreamWrapper(File.Create(FileName));

          outStream = FileStream;
        }
        else
          outStream = null;

        return 0;
      }

      public void PrepareOperation(AskMode askExtractMode)
      {
      }

      public void SetOperationResult(OperationResult resultEOperationResult)
      {
        FileStream.Dispose();
        Console.WriteLine(resultEOperationResult);
      }

      #endregion
    }

    private class ArchiveUpdateCallback : IProgress, IArchiveUpdateCallback
    {
      private IList<FileInfo> FileList;
      private Stream CurrentSourceStream;

      public ArchiveUpdateCallback(IList<FileInfo> list)
      {
        FileList = list;
      }

      #region IProgress Members

      public void SetTotal(ulong total)
      {
      }

      public void SetCompleted(ref ulong completeValue)
      {
      }

      #endregion

      #region IArchiveUpdateCallback Members

      public void GetUpdateItemInfo(int index, out int newData, out int newProperties, out uint indexInArchive)
      {
        newData = 1;
        newProperties = 1;
        indexInArchive = 0xFFFFFFFF;
      }

      private void GetTimeProperty(DateTime time, IntPtr value)
      {
        Marshal.GetNativeVariantForObject(time.ToFileTime(), value);
        Marshal.WriteInt16(value, (short)VarEnum.VT_FILETIME);
      }

      public void GetProperty(int index, ItemPropId propID, IntPtr value)
      {
        FileInfo Source = FileList[index];
        switch (propID)
        {
          case ItemPropId.kpidPath:
            Marshal.GetNativeVariantForObject(Path.GetFileName(Source.FullName), value);
            break;
          case ItemPropId.kpidIsFolder:
          case ItemPropId.kpidIsAnti:
            Marshal.GetNativeVariantForObject(false, value);
            break;
          //case ItemPropId.kpidAttributes:
          //  Marshal.WriteInt16(value, (short)VarEnum.VT_EMPTY);
          //  break;
          case ItemPropId.kpidCreationTime:
            GetTimeProperty(Source.CreationTime, value);
            break;
          case ItemPropId.kpidLastAccessTime:
            GetTimeProperty(Source.LastAccessTime, value);
            break;
          case ItemPropId.kpidLastWriteTime:
            GetTimeProperty(Source.LastWriteTime, value);
            break;
          case ItemPropId.kpidSize:
            Marshal.GetNativeVariantForObject((ulong)Source.Length, value);
            break;
          default:
            Marshal.WriteInt16(value, (short)VarEnum.VT_EMPTY);
            break;
        }
      }

      public void GetStream(int index, out ISequentialInStream inStream)
      {
        FileInfo Source = FileList[index];

        Console.Write("Packing: ");
        Console.Write(Path.GetFileName(Source.FullName));
        Console.Write(' ');

        CurrentSourceStream = Source.OpenRead();
        inStream = new InStreamTimedWrapper(CurrentSourceStream);
      }

      public void SetOperationResult(int operationResult)
      {
        CurrentSourceStream.Close();
        Console.WriteLine("Ok");
      }

      #endregion
    }
  }
}
