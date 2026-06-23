using System.Diagnostics;

namespace RvB.EC.Shared;

public class Benchmark {
    public enum Measure {
        Min,
        NormalizedMean
    }

    private interface IStopwatch {
        bool IsRunning { get; }
        long ElapsedMicroseconds { get; }

        void Start();
        void Stop();
        void Reset();
    }

    private class TimeWatch : IStopwatch, IDisposable {
        private readonly Stopwatch _stopwatch = new();
        private bool _disposedValue;
        ProcessPriorityClass _processPriorityClass;
        ThreadPriority _threadPriority;

        public long ElapsedMicroseconds
            => (_stopwatch.ElapsedTicks * 1000000) / Stopwatch.Frequency;

        public bool IsRunning {
            get { return _stopwatch.IsRunning; }
        }

        public TimeWatch() {
            if (!Stopwatch.IsHighResolution)
                throw new NotSupportedException("Your hardware doesn't support high resolution counter");

            //prevent the JIT Compiler from optimizing Fkt calls away
            long seed = Environment.TickCount;

            //use the second Core/Processor for the test
            //Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);

            //prevent "Normal" Processes from interrupting Threads
            _processPriorityClass = Process.GetCurrentProcess().PriorityClass;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //prevent "Normal" Threads from interrupting this thread
            _threadPriority = Thread.CurrentThread.Priority;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        public void Start() {
            _stopwatch.Start();
        }

        public void Stop() {
            _stopwatch.Stop();
        }

        public void Reset() {
            _stopwatch.Reset();
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing) {
                    Process.GetCurrentProcess().PriorityClass = _processPriorityClass;
                    Thread.CurrentThread.Priority = _threadPriority;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TimeWatch()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    private static long DoBenchmark(Action action, int iterations, Measure measure) {
        //clean Garbage
        GC.Collect();

        //wait for the finalizer queue to empty
        GC.WaitForPendingFinalizers();

        //clean Garbage
        GC.Collect();

        if (iterations > 1) {
            //warm up
            action();
        }

        var timings = new long[iterations];
        using (var stopwatch = new TimeWatch()) {
            for (int i = 0; i < timings.Length; i++) {
                stopwatch.Reset();
                stopwatch.Start();
                action();
                stopwatch.Stop();
                timings[i] = stopwatch.ElapsedMicroseconds;
            }
        }
        if (measure == Measure.Min)
            return timings.Min();
        return NormalizedMean(timings);
    }

    public static long BenchmarkTime(Action action, int iterations, Measure measure) {
        return DoBenchmark(action, iterations, measure);
    }

    private static long NormalizedMean(IEnumerable<long> values) {
        if (!values.Any())
            return 0;

        var deviations = Deviations(values).ToArray();
        var meanDeviation = deviations.Sum(t => Math.Abs(t.Item2)) / values.Count();
        return (long)deviations.AsEnumerable().Where(t => t.Item2 > 0 || Math.Abs(t.Item2) <= meanDeviation).Average(t => t.Item1);
    }

    private static IEnumerable<(long, long)> Deviations(IEnumerable<long> values) {
        if (!values.Any())
            yield break;

        var avg = values.Average();
        foreach (var d in values)
            yield return (d, (long)(avg - d));
    }
}
