using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awrad.Helpers
{
    public class ContentClass
    {
        public string Content { get; set; }
        public string FontFamily { get; set; }
        public int FontSize { get; set; }

        public ContentClass(string content)
        {
            Content = content;
            FontFamily = Constants.ContentFontFamilyName;
            FontSize = Constants.ContentFontSize;
        }

        public ContentClass(string content, string fontFamily, int fontSize)
        {
            Content = content;
            FontFamily = fontFamily;
            FontSize = fontSize;
        }

        /// <summary>
        /// Returns a ContentClass instance with default values
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ContentClass GetContent(string content) => new ContentClass(content);

        /// <summary>
        /// Returns a ContentClass instance with Quran default values
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ContentClass GetQuranContent(string content) => new ContentClass(content, Constants.QuranFontFamilyName, 
                                                                                       Constants.ContentFontSize);
    }
}
