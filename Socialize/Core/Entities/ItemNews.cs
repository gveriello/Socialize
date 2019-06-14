using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifyMe.Core.Classes
{
    public class ItemNews
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string PubDate { get; set; }
        public string Description { get; set; }
        public string ShortTitle
        {
            get
            {
                if (Title.Length < 100)
                    return this.Title;

                return $"{this.Description.Substring(0, 100)}...";
            }
        }
        public string ShortDescription
        {
            get
            {
                return this.Description + " [doppio click per aprire]";
            }
        }
    }
}
