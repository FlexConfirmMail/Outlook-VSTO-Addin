using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using FlexConfirmMail.Config;

namespace FlexConfirmMail
{
    public class QueueLogger
    {
        private static ConcurrentQueue<string> _queue;
        
        public static void Log(string message) => noexcept(() =>  _log(message) );
        public static void Log(Exception e) => noexcept(() =>  _log(e) );

        public static string[] Get()
        {
            _init();
            return _queue.ToArray();
        }

        private static void noexcept(Action func)
        {
            try { func(); } catch { }
        }

        private static void _init()
        {
            if (_queue is null)
            {
                _queue = new ConcurrentQueue<string>();
            }
        }

        private static void _log(string message)
        {
            _init();
            if (5000 < _queue.Count + 1)
            {
                string throwaway;
                _queue.TryDequeue(out throwaway);
            }
            _queue.Enqueue($"{_timestamp()} : {message}");
        }

        private static void _log(Exception e)
        {
            _log(e.ToString());
        }

        private static string _timestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
