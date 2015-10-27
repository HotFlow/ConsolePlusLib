using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Configuration
{
    partial class JsonConfiguration
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private String getDictionaryValue(Dictionary<String, Object> dict, String path)
        {
            try
            {
                if (dict == null) return null;
                int i = path.LastIndexOf('.');
                if (i < 0)
                {
                    if (dict[path] is String)
                        return dict[path] as String;
                    else
                        return null;
                }
                else
                {
                    Dictionary<String, Object> subDict = getDictionaryMap(dict, path.Substring(0, i));
                    string key = path.Substring(i + 1);
                    return getDictionaryValue(subDict, key);
                }
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Dictionary<String, Object> getDictionaryMap(Dictionary<String, Object> dict, String path)
        {
            if (dict == null) return null;
            int i = path.LastIndexOf('.');
            if (i < 0)
            {
                if (dict[path] is Dictionary<String, Object>)
                    return dict[path] as Dictionary<String, Object>;
                else
                    return null;
            }
            else
            {
                Dictionary<String, Object> subDict = getDictionaryMap(dict, path.Substring(0, i));
                string key = path.Substring(i + 1);
                return getDictionaryMap(subDict, key);
            }
        }

        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ArrayList getDictionaryList(Dictionary<String, Object> dict, String path)
        {
            if (dict == null) return null;
            int i = path.LastIndexOf('.');
            if (i < 0)
            {
                if (dict[path] is ArrayList)
                    return dict[path] as ArrayList;
                else
                    return null;
            }
            else
            {
                Dictionary<String, Object> subDict = getDictionaryMap(dict, path.Substring(0, i));
                string key = path.Substring(i + 1);
                return getDictionaryList(subDict, key);
            }
        }

        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Boolean setDictionaryValue(Dictionary<String, Object> dict, string path, string value)
        {
            if (dict == null) return false;
            int i = path.IndexOf('.');
            if (i < 0)
            {
                if (!dict.ContainsKey(path) || dict[path] is string || dict[path] is ArrayList)
                {
                    dict[path] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                String head = path.Substring(0, i);
                String tail = path.Substring(i + 1);
                if (!dict.ContainsKey(head) || dict[head] is Dictionary<String, Object>)
                {
                    if (!dict.ContainsKey(head))
                    {
                        dict.Add(head, new Dictionary<String, Object>());
                    }
                    return setDictionaryValue(dict[head] as Dictionary<String, Object>, tail, value);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Boolean setDictionaryValue(Dictionary<String, Object> dict, string path, Boolean value)
        {
            if (dict == null) return false;
            int i = path.IndexOf('.');
            if (i < 0)
            {
                if (!dict.ContainsKey(path) || dict[path] is string || dict[path] is ArrayList)
                {
                    dict[path] = value.ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                String head = path.Substring(0, i);
                String tail = path.Substring(i + 1);
                if (!dict.ContainsKey(head) || dict[head] is Dictionary<String, Object>)
                {
                    if (!dict.ContainsKey(head))
                    {
                        dict.Add(head, new Dictionary<String, Object>());
                    }
                    return setDictionaryValue(dict[head] as Dictionary<String, Object>, tail, value);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Boolean setDictionaryValue(Dictionary<String, Object> dict, string path, ArrayList value)
        {
            if (dict == null) return false;
            int i = path.IndexOf('.');
            if (i < 0)
            {
                if (!dict.ContainsKey(path) || dict[path] is ArrayList || dict[path] is String)
                {
                    dict[path] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                String head = path.Substring(0, i);
                String tail = path.Substring(i + 1);
                if (!dict.ContainsKey(head) || dict[head] is Dictionary<String, Object>)
                {
                    if (!dict.ContainsKey(head))
                    {
                        dict.Add(head, new Dictionary<String, Object>());
                    }
                    return setDictionaryValue(dict[head] as Dictionary<String, Object>, tail, value);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <returns></returns>
        private String readAllText()
        {
            return File.ReadAllText(file);
        }

        /// <summary>
        /// 写出文件内容
        /// </summary>
        /// <param name="outPut"></param>
        private void writeAlltext(String file, String outPut)
        {
            File.WriteAllText(file, outPut);
        }

        /// <summary>
        /// 载入内容到Dictionary
        /// </summary>
        private void loadToDictionary()
        {
            this.dictionary = this.serializer.Deserialize<Dictionary<String, Object>>(readAllText());
        }

        /// <summary>
        /// 保存内容从Dictionary
        /// </summary>
        private void saveFromDictionary(String file)
        {
            String output = this.serializer.Serialize(this.dictionary);
            String formattedOutput = JSON_PrettyPrinter.Process(output);
            this.writeAlltext(file, formattedOutput);
        }
    }

    class JSON_PrettyPrinter
    {
        private const string INDENT_STRING = "    ";
        public static String Process(String inputText)
        {
            var indent = 0;
            var quoted = false;
            StringBuilder sb = new StringBuilder();
            char[] c = inputText.ToCharArray();
            for (var i = 0; i < inputText.Length; i++)
            {
                var ch = inputText[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        if (!quoted)
                        {
                            if (indent != 0)
                            {
                                sb.AppendLine();
                                Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                            }
                        }
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && inputText[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}
