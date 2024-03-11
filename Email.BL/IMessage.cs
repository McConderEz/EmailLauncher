using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.BL
{
    public interface IMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
    }
}
