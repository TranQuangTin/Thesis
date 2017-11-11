using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

namespace Alta.Plugin
{
    public class FontIcons
    {
        /// <summary>
        /// load data from file path
        /// </summary>
        /// <param name="file">string file path default font-awesome.min.css in streamingAssetsPath </param>
        /// <returns>FontIcons data</returns>
        public static FontIcons LoadData(string file = "")
        {
            if (string.IsNullOrEmpty(file))
                file = System.IO.Path.Combine(Application.streamingAssetsPath, "font-awesome.min.css");
            return CssHelper.ReadFile(file); ;
        }

        private List<Icon> Datas;

        /// <summary>
        /// add icon to list
        /// </summary>
        /// <param name="key">key of icon</param>
        /// <param name="code">code of icon</param>
        public void Add(string key, string code)
        {
           
            this.Add(new Icon(key, code));
        }

        public void Add(Icon icon)
        {
            if (Icon.IsNullOrEmpty(icon))
            {
                return;
            }
         
            if (this.Datas == null)
            {
                Datas = new List<Icon>();
            }
            var i = (from db in this.Datas where db.index == icon.index select db).FirstOrDefault();
            if (Icon.IsNullOrEmpty(i))
            {
                Datas.Add(icon);
            }

        }
        public void Remove(string index)
        {
            this.Datas.RemoveAll(i => i.index == index);
        }

        public void Remove(Icon icon)
        {
            this.Datas.Remove(icon);
        }
        public void Remove(int index)
        {
            this.Datas.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                return this.Datas.Count;
            }
        }
        public Icon this[int pos]
        {
            get
            {
                return this.Datas[pos];
            }
        }
        public Icon this[string index]
        {
            get
            {
                if (this.Datas == null || this.Datas.Count == 0)
                    return Icon.None;
                return this.Datas.Where(i => i.index == index).FirstOrDefault();
            }
        }
        public FontIcons()
        {
            this.Datas = new List<Icon>();
        }
    }
    /// <summary>
    /// define icon 
    /// </summary>
    public struct Icon
    {

        public override bool Equals(object obj)
        {
            if(obj!=null && obj is Icon)
            {
                return this == (Icon)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Icon lhs, Icon rhs)
        {
            return lhs.index == rhs.index && lhs.code == rhs.code;
        }
        public static bool operator !=(Icon lhs, Icon rhs)
        {
            return !(lhs == rhs);
        }


        public static Icon None
        {
            get
            {
                return new Icon("");
            }
        }
        public string index;
        private string code;
        public string Hex { get { return string.Format("&#x{0}", this.code); } }

        public string Code
        {
            get
            {
                if (string.IsNullOrEmpty(this.code))
                    return this.code;
                return string.Format(@"\uf{0}", this.code).DecodeEncodedNonAsciiCharacters();
            }
            set
            {
                this.code = value;
            }
        }

        public Icon(params string[] Params)
        {
            if (Params.Length == 1)
            {
                this.index = Params[0];
                this.code = string.Empty;
            }
            else if (Params.Length == 2)
            {
                this.index = Params[0];
                this.code = Params[1];
            }
            else
            {
                this.code = string.Empty;
                this.index = string.Empty;
            }
        }
        public bool hasIndex
        {
            get
            {
                return !string.IsNullOrEmpty(this.index);
            }
        }
        public bool hasCode
        {
            get
            {
                return !string.IsNullOrEmpty(this.code);
            }
        }
        public override string ToString()
        {
            return string.Format("Icon[index={0},code={1}]", this.index, this.code);
        }

        public static bool IsNullOrEmpty(Icon i)
        {
            if (i == null)
                return true;
            if (string.IsNullOrEmpty(i.code) || string.IsNullOrEmpty(i.index))
                return true;
            return false;
        }

        public static bool IsNull(Icon i)
        {
            if (i == null)
                return true;
            if (string.IsNullOrEmpty(i.index))
                return true;
            return false;
        }
    }

    public static class CssHelper
    {
        /// <summary>
        /// convert file css to list icon
        /// </summary>
        /// <param name="aJSON">content css</param>
        /// <returns>list icon</returns>
        public static FontIcons Css2FontIcons(this string aJSON)
        {
            if (string.IsNullOrEmpty(aJSON))
                return null;
            FontIcons icons = new FontIcons();
            MatchCollection matches = Regex.Matches(aJSON, @"[.](?<key>(?:\w*[-*]\w*)+):before[\s]*[,]?[\s]*(?:[{]?content:(?:\p{P}+[f](?<code>\w+))\p{P}{0,2})*");
            List<string> keyEmptyCache = new List<string>();
            foreach (Match m in matches)
            {
                if (string.IsNullOrEmpty(m.Groups["code"].Value))
                {
                    keyEmptyCache.Add(m.Groups["key"].Value);
                }
                else
                {
                    string code = m.Groups["code"].Value;
                    icons.Add(m.Groups["key"].Value, code);
                    if (keyEmptyCache.Count > 0)
                    {
                        foreach (string key in keyEmptyCache)
                        {
                            icons.Add(key, code);
                        }
                        keyEmptyCache.Clear();
                    }
                }
            }
            return icons;
        }

        public static FontIcons ReadFile(string file)
        {
            string text = File.ReadAllText(file);
            return Css2FontIcons(text);
        }
        
        public static FontIcons cssToFontIcons(FileInfo file)
        {
            return ReadFile(file.FullName);
        }

        public static FontIcons csstoFontIcons(this TextAsset textAsset)
        {
            return Css2FontIcons(textAsset.text);
        }
    }
}