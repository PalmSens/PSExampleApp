using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using PalmSens.Data;
using PalmSens.DataFiles;

namespace PalmSens.Core.iOS.Utils
{
    //TODO: This is only rudimentary code (this will be finished when the SDK for iOS will be created). See same class of the PalmSens.PSAndroid.Core.csproj for implementation exmaples.

    public class LoadSaveHelperFunctions
    {
        /// <summary>
        /// Use as filter for load method file dialog
        /// </summary>
        public const string LoadMethodDialogFilter =
            "All method files (*.psmethod;*.pms;*.pmt)|*.psmethod;*.pms;*.pmt|Scan method file (OLD) (*.pms)|*.pms|Time method file (OLD) (*.pmt)|*.pmt";

        public static string AddDataError;

        /// <summary>
        /// The default dialog path for loading files
        /// </summary>
        public static string PrevLoadDialogPath = String.Empty;

        /// <summary>
        /// The default dialog path for saving files
        /// </summary>
        public static string PrevSaveDialogPath = String.Empty;

        /// <summary>
        /// The path of the current file to use for relative paths
        /// </summary>
        public static string CurrentPath;

        /// <summary>
        /// The back ground worker
        /// </summary>
        private static BackgroundWorker _backGroundWorker = new BackgroundWorker();

        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string GetExecutingAssemblyNameAndVersion()
        {
            if (Assembly.GetEntryAssembly() == null) //FIXME: find a way to get entry assembly reliably in android
                return "Unknown";

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            string version = fvi.ProductVersion;
            return $"{fvi.ProductName} {version}";
        }

        #region Save and Load Files

        /// <summary>
        /// Loads the method. Allowed extensions: .psmethod, .pst and .pss
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="isCorrosion">if set to <c>true</c> [is corrosion]. This is only required for old style method files with extension .pmt and .pms.</param>
        /// <returns></returns>
        public static Method LoadMethod(string filepath, bool isCorrosion = false)
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                Method method;
                if (filepath.EndsWith(MethodFile2.FileExtension))
                    method = MethodFile2.FromStream(sr);
                else
                    method = MethodFile.FromStream(sr, filepath, isCorrosion).Method;

                method.MethodFilename = Path.GetFullPath(filepath);
                return method;
            }
        }

        public static void SaveMethod(Method method, string filepath)
        {
            using (var sw = new StreamWriter(filepath, false, Encoding.Unicode))
            {
                MethodFile2.Save(method, sw.BaseStream, filepath, true, GetExecutingAssemblyNameAndVersion());
            }
        }

        public static void SaveMethod(Method method, string filepath, string versionSDK) //Added for matlab SDK
        {
            using (var sw = new StreamWriter(filepath, false, Encoding.Unicode))
            {
                MethodFile2.Save(method, sw.BaseStream, filepath, true, versionSDK);
            }
        }

        public static SessionManager LoadSessionFile(string filepath)
        {
            SessionManager sm = new SessionManager();
            using (StreamReader sr = new StreamReader(filepath))
                sm.Load(sr.BaseStream, filepath);

            sm.MethodForEditor.MethodFilename = new FileInfo(filepath).Name;
            return sm;
        }

        public static void SaveSessionFile(string filepath, SessionManager sessionManager)
        {
            sessionManager.MethodForEditor.MethodFilename = new FileInfo(filepath).Name;
            using (StreamWriter sw = new StreamWriter(filepath, false, Encoding.Unicode))
                sessionManager.Save(sw.BaseStream, filepath);
            PrevSaveDialogPath = Path.GetDirectoryName(filepath);
        }

        #endregion
    }
}