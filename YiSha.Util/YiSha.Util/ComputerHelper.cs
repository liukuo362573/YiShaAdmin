using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YiSha.Util
{
    public class ComputerHelper
    {
        //定义内存的信息结构
        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }
        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        public static ComputerInfo GetComputerInfo()
        {
            ComputerInfo computerInfo = new ComputerInfo();

            try
            {
                //MEMORY_INFO Memory_Info = new MEMORY_INFO();
                //GlobalMemoryStatus(ref Memory_Info);

                //computerInfo.RAMRate = Memory_Info.dwMemoryLoad.ToString() + " %";
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return computerInfo;
        }

        #region 获取系统内存容量
        /// <summary>
        /// 获取系统内存容量
        /// </summary>
        /// <returns></returns>
        public static string GetTotalMemory()
        {
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(); //用于查询一些如系统信息的管理对象
            //searcher.Query = new SelectQuery("Win32_PhysicalMemory", "", new string[] { "Capacity" });//设置查询条件
            //ManagementObjectCollection collection = searcher.Get(); //获取内存容量
            //ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();
            //long capacity = 0;
            //while (em.MoveNext())
            //{
            //    ManagementBaseObject baseObj = em.Current;
            //    if (baseObj.Properties["Capacity"].Value != null)
            //    {
            //        try
            //        {
            //            capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
            //        }
            //        catch
            //        {

            //        }
            //    }
            //}
            //return string.Format("{0} GB", capacity / (1024 * 1024 * 1024));
            return string.Empty;
        }
        #endregion
    }
    public class ComputerInfo
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CPURate { get; set; }
        /// <summary>
        /// 总内存
        /// </summary>
        public string TotalRAM { get; set; }
        /// <summary>
        /// 内存使用率
        /// </summary>
        public string RAMRate { get; set; }
        /// <summary>
        /// 系统运行时间
        /// </summary>
        public string RunTime { get; set; }
    }
}
