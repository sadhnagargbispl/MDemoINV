using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace InventoryManagement.API.Common
{
    public class Program
    {
        
        public static async Task<bool> SendSMSOTP(string SMS, string MobileNo)
        {
            using (var httpClient = new HttpClient())
            {               
                var values = new Dictionary<string, string>();
                values.Add("message", SMS);
                values.Add("mobiles", MobileNo);
                var content = new FormUrlEncodedContent(values);
                try
                {
                  string  baseurl = "http://control.smsdekho.com/api/sendhttp.php?authkey=205501AxPKu2WRF5ab4fb0a&mobiles=" + MobileNo + "&message=" + SMS + "&sender=PROGLN&route=4&country=91&DLT_TE_ID=1507161209963037467";
                    var httpResponse = await httpClient.PostAsync(baseurl, content);
                    if (httpResponse != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {

                }
            }

            return false;
        }

        public static async Task<bool> SendInvoiceSMS(string SMS, string MobileNo,string tokenid)
        {
            using (var httpClient = new HttpClient())
            {
                var values = new Dictionary<string, string>();
                values.Add("message", SMS);
                values.Add("mobiles", MobileNo);
                var content = new FormUrlEncodedContent(values);
                try
                {
                    string baseurl = "http://control.smsdekho.com/api/sendhttp.php?authkey=205501AxPKu2WRF5ab4fb0a&mobiles=" + MobileNo + "&message=" + SMS + "&sender=PROGLN&route=4&country=91&DLT_TE_ID="+tokenid;
                    var httpResponse = await httpClient.PostAsync(baseurl, content);
                    if (httpResponse != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {

                }
            }

            return false;
        }
    }
}