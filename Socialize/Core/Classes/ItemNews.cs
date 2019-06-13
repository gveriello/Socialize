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
                if (Title.Length < 30)
                    return this.Title;

                return $"{this.Description.Substring(0, 50)}...";
            }
        }
        public string ShortDescription
        {
            get
            {
                if (Description.Length < 50)
                    return this.Description;

                return $"{this.Description.Substring(0, 50)}...";
            }
        }
    }
}
