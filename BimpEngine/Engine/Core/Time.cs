using System.Diagnostics;

namespace BimpEngine.Engine.Core
{
    public static class Time
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();
        private static double lastTime;
        private static double fpsTimer;
        private static int frameCounter;

        public static float DeltaTime { get; private set; }
        public static float UnscaledDeltaTime { get; private set; }

        public static float TimeScale { get; set; } = 1f;
        public static float FixedDeltaTime { get; set; } = 0.02f;

        public static float TimeSinceStartup { get; private set; }
        public static int FPS { get; private set; }
        public static long FrameCount { get; private set; }

        public static void Start()
        {
            Stopwatch.Start();
            lastTime = Stopwatch.Elapsed.TotalSeconds;
            fpsTimer = lastTime;
        }

        public static void Update()
        {
            double currentTime = Stopwatch.Elapsed.TotalSeconds;

            UnscaledDeltaTime = (float)(currentTime - lastTime);
            DeltaTime = UnscaledDeltaTime * TimeScale;

            TimeSinceStartup = (float)currentTime;
            lastTime = currentTime;

            FrameCount++;
            frameCounter++;

            if (currentTime - fpsTimer >= 1.0)
            {
                FPS = frameCounter;
                frameCounter = 0;
                fpsTimer = currentTime;
            }
        }
    }
}
