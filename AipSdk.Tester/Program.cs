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
            //var resultSubmit = client.BasicTextToImage("��������һ��ɫ�ʷḻ�����ݷḻ��ͼ��ͼ�а��� ��������������ڴ��齫����", "512*512", "дʵ���", 1, "TCY");
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(resultSubmit));
           
            //var resultQuery = client.BasicGetImg(resultSubmit.data?.taskId ?? 0);
            var resultQuery = client.BasicGetImg(1896804355565760210L);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(resultQuery));
            Console.Read();
        }
    }
}