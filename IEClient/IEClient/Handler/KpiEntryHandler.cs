using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.LogUtil;
using ClearInsight;
using ClearInsight.Model;
using IEClient.Config;
using IEClientLib;

namespace IEClient.Handler
{
    public class KpiEntryHandler
    {
        private static Object fileLocker = new Object();
        private static Object dirLocker = new Object();

        private static bool unlock = true;

        private static bool deleteFileAfterRead = false;
        /// <summary>
        /// 将数据保存到本地
        /// </summary>
        /// <param name="entry"></param>
        public static void SaveLocal(KpiEntry entry)
        {
            try
            {
                if (entry != null)
                {
                    string dir = System.IO.Path.Combine("Data\\UnHandle");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (FileStream fs = new FileStream(System.IO.Path.Combine(dir, DateTime.Now.ToString("yyyy-MM-dd HH-mm-sss") + Guid.NewGuid().ToString() + ".txt"),
        FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(entry.kpi_code + ";" + entry.entry_at.ToString("yyyy-MM-dd HH:mm:ss") + ";" + entry.project_item_id.ToString() + ";" + entry.tenant_id.ToString() + ";" + entry.node_id.ToString() + ";" + entry.node_code.ToString() + ";" + entry.node_uuid.ToString() + ";" + entry.value);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 扫描本地数据并上传
        /// </summary>
        public static void ScanLocalFile() {
            List<string> files = GetAllFilesFromDirectory("Data\\UnHandle", "*.txt");
            foreach (string file in files)
            {

                if (IsFileClosed(file))
                {
                    if (unlock)
                    {
                        Process(file);
                    }

                }

            }
        }



        /// <summary>
        /// Process File
        /// </summary>
        /// <param name="fullPath"></pairam>
        private static void Process(string fullPath)
        {
            bool canMoveFile = true;
            //  string toScanDir = System.IO.Path.Combine(WPCSConfig.ScanedFileClientDir, DateTime.Today.ToString("yyyy-MM-dd"));
            string processedDir = System.IO.Path.Combine("Data\\Handled", DateTime.Today.ToString("yyyy-MM-dd"));
            try
            {
                if (IsFileClosed(fullPath))
                {

                    using (FileStream fs = new FileStream(fullPath,
                    FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string[] data = sr.ReadLine().Split(';');

                            ClearInsightAPI api = new ClearInsightAPI(BaseConfig.Server, UserSession.GetInstance().CurrentUser.token);

                            KpiEntry entry = new KpiEntry()
                            {
                                kpi_code = data[0],
                                entry_at =DateTime.Parse( data[1]),
                                project_item_id = int.Parse(data[2]),
                                tenant_id = int.Parse(data[3]),
                                node_id = int.Parse(data[4]),
                                node_code = data[5],
                                node_uuid = data[6],
                                value = float.Parse(data[7])
                            };

                            KpiEntry back = api.UploadKpiEntry(entry);


                            canMoveFile = true;

                            //sw.WriteLine(string.Join(",", codes.ToArray()) + ";" + string.Join(",", values.ToArray()) + ";" + time);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Logger.Error(e.GetType());
                canMoveFile = false;
                LogUtil.Logger.Error(e.Message);
            }
            // 是否可以访问服务 不可以访问时保持文件不处理
            if (canMoveFile)
            {
                // 是否删除文件
                if (deleteFileAfterRead)
                {
                    // 删除文件
                    if (IsFileClosed(fullPath))
                    {
                        File.Delete(fullPath);
                        LogUtil.Logger.Warn("[Delete File After Proccessed]" + fullPath);
                    }
                }
                else
                {
                    // 移动文件
                    CheckDirectory(processedDir);
                    MoveFile(fullPath, System.IO.Path.Combine(processedDir, System.IO.Path.GetFileName(fullPath)), false);
                }
            }
        }

        /// <summary>
        /// Get all files in directory
        /// </summary>
        /// <param name="direcctory"></param>
        /// <returns></returns>
        private static List<string> GetAllFilesFromDirectory(string directory, string extenstion = null)
        {
            try
            {
                if (extenstion == null)
                {
                    return Directory.GetFiles(directory.Trim()).ToList();
                }
                else
                {
                    return Directory.GetFiles(directory.Trim(), extenstion).ToList();
                }

            }
            catch (Exception e)
            {
                LogUtil.Logger.Error("[Get All File Error]" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Move file
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="autoRename"></param>
        private static string MoveFile(string sourceFileName, string destFileName, bool autoRename = true)
        {
            try
            {
                lock (fileLocker)
                {
                    if (File.Exists(sourceFileName))
                    {
                        if (autoRename)
                        {
                            destFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(destFileName),
                                DateTime.Now.ToString("HHmmsss") + "_"
                                + System.IO.Path.GetFileNameWithoutExtension(sourceFileName)
                                + "_" + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(sourceFileName));
                        }
                        if (File.Exists(destFileName))
                        {
                            throw new IOException("Target File Exists");
                        }
                        else
                        {
                            File.Move(sourceFileName, destFileName);
                            LogUtil.Logger.Info("Move File [From]" + sourceFileName + "[To]" + destFileName);
                            return destFileName;
                        }
                    }
                    else
                    {
                        throw new IOException("Source File No Exists");
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Logger.Error("Move File [From]" + sourceFileName + "[To]" + destFileName + "[ERROR]" + e.Message);
            }
            return null;
        }

        /// <summary>
        /// Check directory
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        private static bool CheckDirectory(string dirName, bool autoCreate = true)
        {
            bool result = false;
            lock (dirLocker)
            {
                if (Directory.Exists(dirName))
                {
                    result = true;
                }
                else
                {
                    if (autoCreate)
                    {
                        Directory.CreateDirectory(dirName);
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Check file is closed 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool IsFileClosed(string fileName)
        {
            try
            {
                using (File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                LogUtil.Logger.Warn(fileName + "File not close." + e.Message);
                return false;
            }
        }

    }
}
