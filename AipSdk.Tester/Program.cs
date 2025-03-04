using Baidu.Aip.Wenxin;

namespace AipSdk.Tester
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var client = new Wenxin("", "", "");
            client.Timeout = 60000;
            //var resultSubmit = client.BasicTextToImage("帮我制作一张色彩丰富、内容丰富的图，图中包含 “外星人与白蛇在打麻将”。", "512*512", "写实风格", 1, "TCY");
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(resultSubmit));
           
            //var resultQuery = client.BasicGetImg(resultSubmit.data?.taskId ?? 0);
            var resultQuery = client.BasicGetImg(1896804355565760210L);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(resultQuery));
            Console.Read();
        }
    }
}