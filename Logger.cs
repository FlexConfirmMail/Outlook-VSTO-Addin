using System;
using System.Collections.Concurrent;

namespace FlexConfirmMail
{
    public class QueueLogger
    {
        private static ConcurrentQueue<string> s_queue;

        public static void Log(string message) => NoException(() => LogImpl(message));
        public static void Log(Exception e) => NoException(() => LogImpl(e));

        public static string[] Get()
        {
            Init();
            return s_queue.ToArray();
        }

        private static void NoException(Action func)
        {
            try { func(); } catch { }
        }

        private static void Init()
        {
            if (s_queue is null)
            {
                s_queue = new ConcurrentQueue<string>();
            }
        }

        private static void LogImpl(string message)
        {
            Init();
            if (5000 < s_queue.Count + 1)
            {
                string throwaway;
                s_queue.TryDequeue(out throwaway);
            }
            s_queue.Enqueue($"{GetTimestamp()} : {message}");
        }

        private static void LogImpl(Exception e)
        {
            LogImpl(e.ToString());
        }

        private static string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
