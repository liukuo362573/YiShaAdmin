using System;
using YiSha.Util.Helper;

namespace YiSha.IdGenerator
{
    public class Snowflake
    {
        private const long _TwEpoch = 1546272000000L; //2019-01-01 00:00:00

        private const int _WorkerIdBits = 5;
        private const int _DatacenterIdBits = 5;
        private const int _SequenceBits = 12;
        private const long _MaxWorkerId = -1L ^ (-1L << _WorkerIdBits);
        private const long _MaxDatacenterId = -1L ^ (-1L << _DatacenterIdBits);

        private const int _WorkerIdShift = _SequenceBits;
        private const int _DatacenterIdShift = _SequenceBits + _WorkerIdBits;
        private const int _TimestampLeftShift = _SequenceBits + _WorkerIdBits + _DatacenterIdBits;
        private const long _SequenceMask = -1L ^ (-1L << _SequenceBits);

        private long _sequence = 0L;
        private long _lastTimestamp = -1L;

        /// <summary>
        ///10位的数据机器位中的高位
        /// </summary>
        public long WorkerId { get; protected set; }

        /// <summary>
        /// 10位的数据机器位中的低位
        /// </summary>
        public long DatacenterId { get; protected set; }

        private readonly object _lock = new();

        /// <summary>
        /// 基于Twitter的snowflake算法
        /// </summary>
        /// <param name="workerId">10位的数据机器位中的高位，默认不应该超过5位(5byte)</param>
        /// <param name="datacenterId"> 10位的数据机器位中的低位，默认不应该超过5位(5byte)</param>
        /// <param name="sequence">初始序列</param>
        public Snowflake(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;

            if (workerId > _MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"worker Id can't be greater than {_MaxWorkerId} or less than 0");
            }

            if (datacenterId > _MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id can't be greater than {_MaxDatacenterId} or less than 0");
            }
        }

        public long CurrentId { get; private set; }

        /// <summary>
        /// 获取下一个Id，该方法线程安全
        /// </summary>
        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
                if (timestamp < _lastTimestamp)
                {
                    //TODO 是否可以考虑直接等待？
                    throw new Exception($"Clock moved backwards or wrapped around. Refusing to generate id for {_lastTimestamp - timestamp} ticks");
                }

                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & _SequenceMask;
                    if (_sequence == 0)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }
                _lastTimestamp = timestamp;
                CurrentId = ((timestamp - _TwEpoch) << _TimestampLeftShift) |
                            (DatacenterId << _DatacenterIdShift) |
                            (WorkerId << _WorkerIdShift) | _sequence;

                return CurrentId;
            }
        }

        private long TilNextMillis(long lastTimestamp)
        {
            var timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
            while (timestamp <= lastTimestamp)
            {
                timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
            }
            return timestamp;
        }
    }
}