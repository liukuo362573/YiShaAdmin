using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YiSha.Util.Extension;

namespace YiSha.Util
{
    public class ComputerHelper
    {
        public static ComputerInfo GetComputerInfo()
        {
            ComputerInfo computerInfo = new ComputerInfo();
            try
            {
                MemoryMetricsClient client = new MemoryMetricsClient();
                MemoryMetrics memoryMetrics = client.GetMetrics();
                computerInfo.TotalRAM = Math.Ceiling(memoryMetrics.Total / 1024).ToString() + " GB";
                computerInfo.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + " %";
                computerInfo.CPURate = Math.Ceiling(GetCPURate().ParseToDouble()) + " %";
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return computerInfo;
        }

        public static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            return isUnix;
        }

        public static string GetCPURate()
        {
            string cpuRate = string.Empty;
            if (IsUnix())
            {
                string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                string output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }
            return cpuRate;
        }
    }

    public class MemoryMetrics
    {
        public double Total;
        public double Used;
        public double Free;
    }

    public class MemoryMetricsClient
    {
        public MemoryMetrics GetMetrics()
        {
            if (ComputerHelper.IsUnix())
            {
                return GetUnixMetrics();
            }
            return GetWindowsMetrics();
        }

        private MemoryMetrics GetWindowsMetrics()
        {
            string output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        private MemoryMetrics GetUnixMetrics()
        {
            string output = ShellHelper.Bash("free -m");

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }
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
