namespace Kuno.Utilities.NewId
{
    internal class NewIdGenerator
    {
        readonly int _c;
        readonly int _d;

        readonly object _sync = new object();
        readonly ITickProvider _tickProvider;
        int _a;
        int _b;
        long _lastTick;

        ushort _sequence;


        /// <summary>
        /// Initializes a new instance of the <see cref="NewIdGenerator"/> class.
        /// </summary>
        /// <param name="tickProvider">The tick provider.</param>
        /// <param name="workerIdProvider">The worker identifier provider.</param>
        /// <param name="processIdProvider">The process identifier provider.</param>
        /// <param name="workerIndex">Index of the worker.</param>
        public NewIdGenerator(ITickProvider tickProvider, IWorkerIdProvider workerIdProvider, IProcessIdProvider processIdProvider = null, int workerIndex = 0)
        {
            _tickProvider = tickProvider;

            byte[] workerId = workerIdProvider.GetWorkerId(workerIndex);

            _c = workerId[0] << 24 | workerId[1] << 16 | workerId[2] << 8 | workerId[3];

            if(processIdProvider != null)
            {
                var processId = processIdProvider.GetProcessId();
                _d = processId[0] << 24 | processId[1] << 16;
            }
            else
            {
                _d = workerId[4] << 24 | workerId[5] << 16;
            }
        }

        /// <summary>
        /// Nexts this instance.
        /// </summary>
        /// <returns>NewId.</returns>
        public NewId Next()
        {
            long ticks = _tickProvider.Ticks;
            lock (_sync)
            {
                if (ticks > _lastTick)
                    this.UpdateTimestamp(ticks);

                if (_sequence == 65535) // we are about to rollover, so we need to increment ticks
                    this.UpdateTimestamp(_lastTick + 1);

                ushort sequence = _sequence++;

                return new NewId(_a, _b, _c, _d | sequence);
            }
        }

        void UpdateTimestamp(long tick)
        {
            _lastTick = tick;
            _sequence = 0;

            _a = (int)(tick >> 32);
            _b = (int)(tick & 0xFFFFFFFF);
        }
    }
}