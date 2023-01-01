using StorageOccupationStatistics;
using System.Diagnostics;

namespace StorageStatisticsTest {
    [TestClass]
    public class FileTreeTest {
        //[TestMethod]
        //public void TestModelsFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\_Models");
        //    Assert.AreEqual(fit.GetFileCount(), 0);
        //    Assert.AreEqual(fit.GetFolderCount(), 0);
        //    Assert.AreEqual(fit.GetTotalSize(), 0);
        //}
        //[TestMethod]
        //public void TestComboKeyFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\ComboKey");
        //    Assert.AreEqual(fit.GetFileCount(), 1);
        //    Assert.AreEqual(fit.GetFolderCount(), 0);
        //    Assert.AreEqual(fit.GetTotalSize(), 193);
        //}
        //[TestMethod]
        //public void TestDataFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\Data");
        //    Assert.AreEqual(fit.GetFileCount(), 2482401);
        //    Assert.AreEqual(fit.GetFolderCount(), 13328);
        //    Assert.AreEqual(fit.GetTotalSize(), 30466578504);
        //}
        //[TestMethod]
        //public void TestIntelFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\Intel");
        //    Assert.AreEqual(fit.GetFileCount(), 4);
        //    Assert.AreEqual(fit.GetFolderCount(), 3);
        //    Assert.AreEqual(fit.GetTotalSize(), 934108);
        //}
        //[TestMethod]
        //public void TestPerfFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\PerfLogs");
        //    Assert.AreEqual(fit.GetFileCount(), 0);
        //    Assert.AreEqual(fit.GetFolderCount(), 0);
        //    Assert.AreEqual(fit.GetTotalSize(), 0);
        //}
        //[TestMethod]
        //public void TestProgFilesFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\Program Files");
        //    Assert.AreEqual(fit.GetTotalSize(), 43280829644);
        //    Assert.AreEqual(fit.GetFolderCount(), 21699);
        //    Assert.AreEqual(fit.GetFileCount(), 198583);
        //}
        //[TestMethod]
        //public void TestProgFiles86Folder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\Program Files (x86)");
        //    Assert.AreEqual(fit.GetTotalSize(), 129191703382);
        //    Assert.AreEqual(fit.GetFolderCount(), 17414);
        //    Assert.AreEqual(fit.GetFileCount(), 167742);
        //}


        #region progdata innerfolders
//        [TestMethod]
//        public void TestProgDataAdobeFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Adobe");
//            Assert.AreEqual(fit.GetTotalSize(), 994829432);
//            Assert.AreEqual(fit.GetFileCount(), 5296);
//            Assert.AreEqual(fit.GetFolderCount(), 487);
//        }

//        [TestMethod]
//        public void TestProgDataAnaconda3Folder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Anaconda3");
//            Assert.AreEqual(fit.GetTotalSize(), 30179536261);
//            Assert.AreEqual(fit.GetFileCount(), 235962);
//            Assert.AreEqual(fit.GetFolderCount(), 27514);
//        }

//        [TestMethod]
//        public void TestProgDataBrotherFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Brother");
//            Assert.AreEqual(fit.GetTotalSize(), 44967);
//            Assert.AreEqual(fit.GetFileCount(), 8);
//            Assert.AreEqual(fit.GetFolderCount(), 5);
//        }

////        C:\ProgramData\ntuser.pol
////400 0
////Files : 1
////Folders : 0
////        C:\ProgramData\Application Data
////0 0
////Files : 0
////Folders : 1
////        C:\ProgramData\Desktop
////0 0
////Files : 0
////Folders : 1
////        C:\ProgramData\Documents
////0 0
////Files : 0
////Folders : 1
////C:\ProgramData\Start Menu
////0 0
////Files : 0
////Folders : 1
////C:\ProgramData\Templates
////0 0
////Files : 0
////Folders : 1
//        [TestMethod]
//        public void TestProgDataElectronic_ArtsFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Electronic Arts");
//            Assert.AreEqual(fit.GetTotalSize(), 593);
//            Assert.AreEqual(fit.GetFileCount(), 1);
//            Assert.AreEqual(fit.GetFolderCount(), 2);
//        }

//        [TestMethod]
//        public void TestProgDataHewlett_PackardFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Hewlett-Packard");
//            Assert.AreEqual(fit.GetTotalSize(), 0);
//            Assert.AreEqual(fit.GetFileCount(), 1);
//            Assert.AreEqual(fit.GetFolderCount(), 1);
//        }

//        [TestMethod]
//        public void TestProgDataHPFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\HP");
//            Assert.AreEqual(fit.GetTotalSize(), 568);
//            Assert.AreEqual(fit.GetFileCount(), 1);
//            Assert.AreEqual(fit.GetFolderCount(), 3);
//        }

//        [TestMethod]
//        public void TestProgDataIntelFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Intel");
//            Assert.AreEqual(fit.GetTotalSize(), 176241681);
//            Assert.AreEqual(fit.GetFileCount(), 32);
//            Assert.AreEqual(fit.GetFolderCount(), 9);
//        }

//        [TestMethod]
//        public void TestProgDataFolderjupyter() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\jupyter");
//            Assert.AreEqual(fit.GetTotalSize(), 0);
//            Assert.AreEqual(fit.GetFileCount(), 0);
//            Assert.AreEqual(fit.GetFolderCount(), 5);
//        }

//        [TestMethod]
//        public void TestProgDataMicrosoftFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Microsoft");
//            Assert.AreEqual(fit.GetTotalSize(), 14034722315);
//            Assert.AreEqual(fit.GetFileCount(), 6509);
//            Assert.AreEqual(fit.GetFolderCount(), 2182);
//        }

//        [TestMethod]
//        public void TestProgDataMicrosoftOneDriveFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Microsoft OneDrive");
//            Assert.AreEqual(fit.GetTotalSize(), 25);
//            Assert.AreEqual(fit.GetFileCount(), 1);
//            Assert.AreEqual(fit.GetFolderCount(), 1);
//        }
//        [TestMethod]
//        public void TestProgDataMVSFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Microsoft Visual Studio");
//            Assert.AreEqual(fit.GetTotalSize(), 2118);
//            Assert.AreEqual(fit.GetFileCount(), 4);
//            Assert.AreEqual(fit.GetFolderCount(), 0);
//        }
//        [TestMethod]
//        public void TestProgDataMozillaFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Mozilla");
//            Assert.AreEqual(fit.GetTotalSize(), 0);
//            Assert.AreEqual(fit.GetFileCount(), 1);
//            Assert.AreEqual(fit.GetFolderCount(), 0);
//        }
//        [TestMethod]
//        public void TestProgDataMozilla2Folder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Mozilla-1de4eec8-1241-4177-a864-e594e8d1fb38");
//            Assert.AreEqual(fit.GetTotalSize(), 34418883);
//            Assert.AreEqual(fit.GetFileCount(), 263);
//            Assert.AreEqual(fit.GetFolderCount(), 12);
//        }
//        [TestMethod]
//        public void TestProgDataNVIDIAFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\NVIDIA");
//            Assert.AreEqual(fit.GetTotalSize(), 261110903);
//            Assert.AreEqual(fit.GetFileCount(), 338);
//            Assert.AreEqual(fit.GetFolderCount(), 36);
//        }
//        [TestMethod]
//        public void TestProgDataNVIDIACorporationFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\NVIDIA Corporation");
//            Assert.AreEqual(fit.GetTotalSize(), 774382042);
//            Assert.AreEqual(fit.GetFileCount(), 1192);
//            Assert.AreEqual(fit.GetFolderCount(), 99);
//        }
//        [TestMethod]
//        public void TestProgDataFolderOracle() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Oracle");
//            Assert.AreEqual(fit.GetTotalSize(), 82552059);
//            Assert.AreEqual(fit.GetFileCount(), 3);
//            Assert.AreEqual(fit.GetFolderCount(), 3);
//        }

//        [TestMethod]
//        public void TestProgDataOriginFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Origin");
//            Assert.AreEqual(fit.GetTotalSize(), 376277025);
//            Assert.AreEqual(fit.GetFileCount(), 424);
//            Assert.AreEqual(fit.GetFolderCount(), 33);
//        }

//        [TestMethod]
//        public void TestProgDataPackageCacheFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Package Cache");
//            Assert.AreEqual(fit.GetTotalSize(), 521259401);
//            Assert.AreEqual(fit.GetFileCount(), 317);
//            Assert.AreEqual(fit.GetFolderCount(), 194);
//        }

//        [TestMethod]
//        public void TestProgDataPackagesFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Packages");
//            Assert.AreEqual(fit.GetTotalSize(), 237568);
//            Assert.AreEqual(fit.GetFileCount(), 36);
//            Assert.AreEqual(fit.GetFolderCount(), 114);
//        }

//        [TestMethod]
//        public void TestProgDataPreSonusFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\PreSonus");
//            Assert.AreEqual(fit.GetTotalSize(), 236968);
//            Assert.AreEqual(fit.GetFileCount(), 107);
//            Assert.AreEqual(fit.GetFolderCount(), 19);
//        }

//        [TestMethod]
//        public void TestProgDataregedFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\regid.1991-06.com.microsoft");
//            Assert.AreEqual(fit.GetTotalSize(), 4253);
//            Assert.AreEqual(fit.GetFileCount(), 4);
//            Assert.AreEqual(fit.GetFolderCount(), 0);
//        }

//        [TestMethod]
//        public void TestProgDataSoftwareDistributionFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\SoftwareDistribution");
//            Assert.AreEqual(fit.GetTotalSize(), 0);
//            Assert.AreEqual(fit.GetFileCount(), 0);
//            Assert.AreEqual(fit.GetFolderCount(), 0);
//        }
//        [TestMethod]
//        public void TestProgDatasshFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\ssh");
//            Assert.AreEqual(fit.GetTotalSize(), 0);
//            Assert.AreEqual(fit.GetFileCount(), 0);
//            Assert.AreEqual(fit.GetFolderCount(), 0);
//        }
//        [TestMethod]
//        public void TestProgDataUnityFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Unity");
//            Assert.AreEqual(fit.GetTotalSize(), 2634);
//            Assert.AreEqual(fit.GetFileCount(), 1);
//            Assert.AreEqual(fit.GetFolderCount(), 0);
//        }
//        [TestMethod]
//        public void TestProgDataUSOPrivateFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\USOPrivate");
//            Assert.AreEqual(fit.GetTotalSize(), 12238848);
//            Assert.AreEqual(fit.GetFileCount(), 1);
//            Assert.AreEqual(fit.GetFolderCount(), 1);
//        }
//        [TestMethod]
//        public void TestProgDataUSOSharedFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\USOShared");
//            Assert.AreEqual(fit.GetTotalSize(), 119226368);
//            Assert.AreEqual(fit.GetFileCount(), 1878);
//            Assert.AreEqual(fit.GetFolderCount(), 3);
//        }
//        [TestMethod]
//        public void TestProgDatawinappcertFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\Windows App Certification Kit");
//            Assert.AreEqual(fit.GetTotalSize(), 1835887);
//            Assert.AreEqual(fit.GetFileCount(), 18);
//            Assert.AreEqual(fit.GetFolderCount(), 15);
//        }
//        [TestMethod]
//        public void TestProgDataWindowsHolographicDevicesFolder() {
//            FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\WindowsHolographicDevices");
//            Assert.AreEqual(fit.GetTotalSize(), 0);
//            Assert.AreEqual(fit.GetFileCount(), 0);
//            Assert.AreEqual(fit.GetFolderCount(), 1);
//        }

        #endregion








        //[TestMethod]
        //public void TestProgDataFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\ProgramData\\");
        //    fit.PrintRootOneLayerInfo();
        //    Debug.WriteLine(fit.GetTotalSize()- 47569157599);
        //    Assert.AreEqual(fit.GetTotalSize(), 47569157599);
        //    Assert.AreEqual(fit.GetFileCount(), 252399);
        //}
        //[TestMethod]
        //public void TestUsersFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\Users");
        //    Assert.AreEqual(fit.GetTotalSize(), 195813341062);
        //    Assert.AreEqual(fit.GetFolderCount(), 46152);
        //    Assert.AreEqual(fit.GetFileCount(), 1037611);
        //}
        //[TestMethod]
        //public void TestWindowsFolder() {
        //    FileInfoTree fit = new FileInfoTree("C:\\Windows");
        //    Assert.AreEqual(fit.GetTotalSize(), 30197839476);
        //    Assert.AreEqual(fit.GetFolderCount(), 88595);
        //    Assert.AreEqual(fit.GetFileCount(), 224676);
        //}
    }
}