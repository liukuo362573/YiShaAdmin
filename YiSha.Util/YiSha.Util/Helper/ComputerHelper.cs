using System;
using System.Runtime.InteropServices;
using YiSha.Util.Extension;

namespace YiSha.Util.Helper
{
    public static class ComputerHelper
    {
        public static ComputerInfo GetComputerInfo()
        {
            var computerInfo = new ComputerInfo();
            try
            {
                var client = new MemoryMetricsClient();
                var memoryMetrics = client.GetMetrics();
                computerInfo.TotalRam = Math.Ceiling(memoryMetrics.Total / 1024) + " GB";
                computerInfo.RamRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total) + " %";
                computerInfo.CpuRate = Math.Ceiling(GetCpuRate().ParseToDouble()) + " %";
                computerInfo.RunTime = GetRunTime();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return computerInfo;
        }

        public static bool IsUnix()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static string GetCpuRate()
        {
            if (IsUnix())
            {
                return ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'").Trim();
            }
            return ShellHelper.Cmd("wmic", "cpu get LoadPercentage").Replace("LoadPercentage", string.Empty).Trim();
        }

        public static string GetRunTime()
        {
            string runTime = string.Empty;
            try
            {
                if (IsUnix())
                {
                    string output = ShellHelper.Bash("uptime -s").Trim();
                    runTime = DateTimeHelper.FormatTime((DateTime.Now - output.ParseToDateTime()).TotalMilliseconds.ToString().Split('.')[0].ParseToLong());
                }
                else
                {
                    string output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime/Value");
                    string[] outputArr = output.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    if (outputArr.Length == 2)
                    {
                        runTime = DateTimeHelper.FormatTime((DateTime.Now - outputArr[1].Split('.')[0].ParseToDateTime()).TotalMilliseconds.ToString().Split('.')[0].ParseToLong());
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return runTime;
        }
    }

    public class MemoryMetrics
    {
        public double Total { get; set; }

        public double Used { get; set; }

        public double Free { get; set; }
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
            var total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            var free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);

            return new MemoryMetrics { Total = total, Free = free, Used = total - free };
        }

        private MemoryMetrics GetUnixMetrics()
        {
            string output = ShellHelper.Bash("free -m");

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return new MemoryMetrics
            {
                Total = double.Parse(memory[1]),
                Used = double.Parse(memory[2]),
                Free = double.Parse(memory[3])
            };
        }
    }

    public class ComputerInfo
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CpuRate { get; set; }

        /// <summary>
        /// 总内存
        /// </summary>
        public string TotalRam { get; set; }

        /// <summary>
        /// 内存使用率
        /// </summary>
        public string RamRate { get; set; }

        /// <summary>
        /// 系统运行时间
        /// </summary>
        public string RunTime { get; set; }
    }
}