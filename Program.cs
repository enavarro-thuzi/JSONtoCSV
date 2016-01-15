using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONtoCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDir = @"C:\Users\elionavarro\Documents\Analytics\FB\765091436892607\app_insights_UFC_194\";
            string sourceFileExt = @".txt";
            string targetFileExt = @".csv";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("event_type,time,value,breakdowns_client,breakdowns_country");

            foreach (var sourceFile in Directory.EnumerateFiles(sourceDir))
            {
                if(sourceFile.EndsWith(sourceFileExt))
                {
                    string json = File.ReadAllText(sourceFile);

                    var FBAnalyticsDataObj1 = JsonConvert.DeserializeObject<FBAnalyticsData>(json);

                    string eventType = sourceFile.Replace(sourceDir, "").Replace(sourceFileExt, "").Split('.')[0];

                    foreach (var fbAnalyticsEvent in FBAnalyticsDataObj1.data)
                    {
                        sb.AppendLine(eventType+","+fbAnalyticsEvent.time + "," + fbAnalyticsEvent.value + "," + fbAnalyticsEvent.breakdowns.client + "," + fbAnalyticsEvent.breakdowns.country);
                    }
                }
            }

            File.WriteAllText(sourceDir + "fbAnalyticsData"+ targetFileExt, sb.ToString());
        }
    }

    public class FBAnalyticsData
    {
        public List<FBAnalyticsEvent> data { get; set; }

        public class FBAnalyticsEvent
        {
            public string time { get; set; }
            public string value { get; set; }
            public EventBreakdown breakdowns { get; set; }

            public class EventBreakdown
            {
                public string client { get; set; }
                public string country { get; set; }
            }
        }
    }
}
