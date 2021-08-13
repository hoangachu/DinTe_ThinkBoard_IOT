using DINTEIOT.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DINTEIOT.Helpers.Common
{
    public class Common
    {
        public DateTime FormatDateTimeToYYYYMMDD(DateTime dateTime)
        {
            string preformat = dateTime.ToString("yyyy-MM-dd h:mm tt");
            DateTime date = Convert.ToDateTime(dateTime);
            return date;
        }
        public static string GenerateUrl(string Url)
        {
            Url = convertToUnSign3(Url);
            string UrlPeplaceSpecialWords = Regex.Replace(Url, @"&quot;|['"",&?%\.!()@$^_+=*:#/\\-]", " ").Trim();
            string RemoveMutipleSpaces = Regex.Replace(UrlPeplaceSpecialWords, @"\s+", " ");
            string ReplaceDashes = RemoveMutipleSpaces.Replace(" ", "-");
            string DuplicateDashesRemove = ReplaceDashes.Replace("--", "-");
            return DuplicateDashesRemove.ToLower();
        }
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public void SendSms()   // send sms
        {
            //var accountSid = "AC0c4d43b99bafc66948f4001a192cd54d";
            //var authToken = "8013c3f695e07d2fd5a702edc53f04f3";
            //TwilioClient.Init(accountSid, authToken);

            //var message = MessageResource.Create(
            //    body: "This is the ship that made the Kessel Run in fourteen parsecs?",
            //    from: new Twilio.Types.PhoneNumber("+15017122661"),
            //    to: new Twilio.Types.PhoneNumber("+8436403229")
            //);
            //return Content(message.Sid)
        }

        public static List<TreeViewNode> SelectSortNodes(List<TreeViewNode> nodes, List<TreeViewNode> root) // lấy treeviewnode dạng cha con
        {
            for (int i = 0; i < root.Count; i++)
            {
                List<TreeViewNode> children = nodes.FindAll(node => node.parentid == root[i].id);
                SelectSortNodes(nodes, children);
                root[i].subs = children;
            }
            return root;
        }
        public static List<TreeViewNode> SelectSortNodesPeer(List<TreeViewNode> nodes, List<TreeViewNode> root) //lấy ds ngang hàng
        {
            int index = 2;
            for (int i = 0; i < root.Count; i++)
            {
                List<TreeViewNode> children = nodes.FindAll(node => node.parentid == root[i].id);
                children.ForEach(x => x.displaynumber = "l" + index);
                index += 1;
                children.ForEach(x => root.Add(x));
                SelectSortNodesPeer(nodes, children);
            }
            return root;
        }
    }
}
