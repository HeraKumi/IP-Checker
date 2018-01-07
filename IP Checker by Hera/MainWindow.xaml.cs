using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace IP_Checker_by_Hera
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IP.Text))
                {
                    MessageBox.Show("Please don't leave the IP box blank");
                    return;
                }

                HttpWebRequest r = (HttpWebRequest)WebRequest.Create($"http://v2.api.iphub.info/ip/{IP.Text}");
                r.Method = "GET";
                r.Headers["X-key"] = "MTE3OnBNOHcxZWVwV3ROczN4WFRFZmRZV212cGNIaGhyS0dy";

                HttpWebResponse rs = (HttpWebResponse)r.GetResponse();
                string result = null;

                using (System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }


                var obj = JsonConvert.DeserializeObject<IpApi>(result);

                var blocked = "";
                if (obj.Block == 0)
                    blocked = "Residential/Unclassified IP (i.e. safe IP)";
                else if (obj.Block == 1)
                    blocked = "Non-residential IP (hosting provider, proxy, etc.)";
                else
                    blocked = "Non-residential & residential IP (warning, may flag innocent people)";


                Hostname.Text = $"{obj.Hostname}";
                Typee.Text = $"{blocked}";
                Isp.Text = $"{obj.Isp}";
                CountryY.Text = $"{obj.CountryName}";
                CountryCodeE.Text = $"{obj.CountryCode}";
                Asn.Text = $"{obj.Asn}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The ip ({IP.Text}) could not be found, Please try again.");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (IP.Text != "" && Hostname.Text != "" && Typee.Text != "" && Isp.Text != "" && CountryY.Text != "" && CountryCodeE.Text != "" && Asn.Text != "")
            {
                IP.Text = "";
                Hostname.Text = "";
                Typee.Text = "";
                Isp.Text = "";
                CountryY.Text = "";
                CountryCodeE.Text = "";
                Asn.Text = "";
            }
        }

    }
    public class IpApi
    {
        [J("ip")] public string Ip { get; set; }
        [J("countryCode")] public string CountryCode { get; set; }
        [J("countryName")] public string CountryName { get; set; }
        [J("asn")] public long Asn { get; set; }
        [J("isp")] public string Isp { get; set; }
        [J("block")] public long Block { get; set; }
        [J("hostname")] public string Hostname { get; set; }
    }
}
