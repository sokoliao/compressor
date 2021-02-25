using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace CompressorTests
{
    public class TempFileFixture : IDisposable
    {
        private object locker = new object();
        private List<string> files = new List<string>();

        public string GetTempFile()
        {
            lock (locker)
            {
                var fileName = Path.GetTempFileName();
                files.Add(fileName);
                return fileName;
            }
        }

        public void Dispose()
        {
            lock (locker)
            {
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}