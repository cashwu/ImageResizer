using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output");

            var cts = new CancellationTokenSource();
            
            #region 等候使用者輸入 取消 c 按鍵

            ThreadPool.QueueUserWorkItem(x =>
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.C)
                {
                    cts.Cancel();
                }
            });

            #endregion

            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Clean(destinationPath);
            Stopwatch sw = new Stopwatch();
            sw.Start();

//            imageProcess.ResizeImages(sourcePath, destinationPath, 2.0d);
//            await imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0d, CancellationToken.None);

            await imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0d, cts.Token);

            sw.Stop();

            Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms");
        }
    }
}