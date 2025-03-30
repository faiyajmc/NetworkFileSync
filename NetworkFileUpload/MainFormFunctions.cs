using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace NetworkFileUpload
{
    class MainFormFunctions
    {
        public string SrcFldr;

        public string DstFldr;

        private FileSystemWatcher srcWatch;

        public List<FileData> initFiles = null;

        public List<FileData> currFiles = null;

        private bool CheckPaths()
        {
            bool validPaths = File.Exists(SrcFldr) && File.Exists(DstFldr);

            return validPaths;

        }

        public void InitializeFileTracking()
        {
            SrcFldr = Properties.Settings.Default["SrcFldr"].ToString();
            DstFldr = Properties.Settings.Default["DstFldr"].ToString();
            while (!CheckPaths())
            {
                System.Windows.Forms.MessageBox.Show("Invalid Folder Paths");
                SettingsForm();
            }

            if(initFiles == null)
            {
                SetupWatcher();
            }

            
            
        }

        public void SettingsForm()
        {
            SettingsFrm settings = new SettingsFrm();
            settings.ShowDialog();
            settings.Dispose();

            SrcFldr = Properties.Settings.Default["SrcFldr"].ToString();
            DstFldr = Properties.Settings.Default["DstFldr"].ToString();

            SetupWatcher();
        }

        private void SetupWatcher()
        {
            initFiles = InitialFileCheck(SrcFldr);
            srcWatch = new FileSystemWatcher();
            srcWatch.Path = SrcFldr;
            srcWatch.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;

            srcWatch.Created += OnCreated;
            srcWatch.Changed += OnChanged;
            srcWatch.Deleted += OnDeleted;
            srcWatch.Renamed += OnRenamed;

            // Enable the srcWatch
            srcWatch.EnableRaisingEvents = true;
        }

        public List<FileData> InitialFileCheck(string folder)
        {
            List<FileData> EditedFiles = new List<FileData>();

            foreach (FileInfo file in new DirectoryInfo(folder).GetFiles())
            {
                FileData currFile = new FileData(file.Name, GetFileHash(file.FullName));

                EditedFiles.Add(currFile);
            }

            return EditedFiles;
        }

        byte[] GetFileHash(string fileName)
        {
            //try
            //{
            //    HashAlgorithm sha1 = HashAlgorithm.Create();
            //    using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            //    {
            //        return sha1.ComputeHash(stream);
            //    }
            //}
            //catch
            //{
            //    return new byte[0];
            //}

            HashAlgorithm sha1 = HashAlgorithm.Create();
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return sha1.ComputeHash(stream);
            }
        }

        public List<FileData> CompareLists(List<FileData> oldListRaw, List<FileData> newListRaw)
        {
            Dictionary<string, byte[]> oldList = oldListRaw.ToDictionary(item => item.FileName, item => item.Hash);
            Dictionary<string, byte[]> newList = newListRaw.ToDictionary(item => item.FileName, item => item.Hash);

            // List to store changed or new files
            var modifiedFiles = new List<KeyValuePair<string, byte[]>>();

            foreach (var entry in newList)
            {
                string filename = entry.Key;
                byte[] newHash = entry.Value;

                if (!oldList.ContainsKey(filename))
                {
                    // New file, add to modified list
                    modifiedFiles.Add(entry);
                }
                else if (oldList[filename] != newHash)
                {
                    // File exists but hash changed, add to modified list
                    modifiedFiles.Add(entry);
                }
            }

            List<FileData> modFiles = new List<FileData>();

            foreach (var file in modifiedFiles)
            {
                FileData element = new FileData(file.Key, file.Value);

                modFiles.Add(element);
            }

            return modFiles;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            FileData CreatedFile = new FileData(e.Name, GetFileHash(e.FullPath));

            currFiles.Add(CreatedFile);

            UpdateList();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                // Find the corresponding FileData object
                var fileData = fileList.FirstOrDefault(f => f.FileName.Equals(e.FullPath, StringComparison.OrdinalIgnoreCase));

                if (fileData != null)
                {
                    // Compute new hash and update
                    byte[] newHash = GetFileHash(e.FullPath);
                    fileData.Hash = newHash;

                    Console.WriteLine($"File updated: {e.FullPath}, New Hash: {BitConverter.ToString(newHash)}");
                }
                else
                {
                    Console.WriteLine($"File changed but not tracked: {e.FullPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {e.FullPath}: {ex.Message}");
            }
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {

        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {

        }

        private void UpdateList()
        {

        }
    }

    class FileData
    {
        public string FileName { get; set; }
        public byte[] Hash { get; set; }

        public FileData(string fileName, byte[] hash)
        {
            FileName = fileName;
            Hash = hash;
        }

    }
}
