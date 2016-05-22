//using System;
//using System.IO;
//using System.Reflection;
//using SevenZip;

//namespace KCVDB.LocalAnalyze.IO
//{
//    public static class SevenZipDll
//    {
//        public static void Initialize()
//        {
//            if (IntPtr.Size == 4)
//            {
//                SevenZipBase.SetLibraryPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x86", "7z.dll"));
//            }
//            else if (IntPtr.Size == 8)
//            {
//                SevenZipBase.SetLibraryPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x64", "7z.dll"));
//            }
//            else
//            {
//                throw new PlatformNotSupportedException();
//            }
//        }
//    }
//}
