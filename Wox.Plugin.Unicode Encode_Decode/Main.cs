﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Wox.Plugin.Unicode_Encode_Decode
{
    public class Main : IPlugin
    {
        public void Init(PluginInitContext context)
        {
            //TODO 初始化执行的方法
        }

        public List<Result> Query(Query query)
        {
            String content = query.Search;
            List<Result> results = new List<Result>();
            if (content == null || content.Length == 0)
            {
                results.Add(new Result()
                {
                    Title = "输入需要编码/解码的内容",
                    IcoPath = "Images\\unicode.png",
                    Action = e =>
                    {
                        return false;
                    }
                });
                return results;
            }

            String encode = StringToUnicode(content);
            if (encode != null && encode.Length != 0)
            {
                results.Add(new Result()
                {
                    Title = encode,
                    SubTitle = "拷贝编码内容到剪切板",
                    IcoPath = "Images\\encode.png",
                    Action = e =>
                    {
                        Clipboard.SetDataObject(encode);
                        return true;
                    }
                });
            }

            String decode = UnicodeToString(content);
            if (decode != null && decode.Length != 0)
            {
                results.Add(new Result()
                {
                    Title = decode,
                    SubTitle = "拷贝解码内容到剪切板",
                    IcoPath = "Images\\decode.png",
                    Action = e =>
                    {
                        Clipboard.SetDataObject(decode);
                        return true;
                    }
                });
            }
            return results;

        }

        /// <summary>  
        /// 字符串转为UniCode码字符串  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static string StringToUnicode(string s)
        {
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }

        /// <summary>  
        /// Unicode字符串转为正常字符串  
        /// </summary>  
        /// <param name="srcText"></param>  
        /// <returns></returns>  
        public static string UnicodeToString(string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;

        }
    }
}
