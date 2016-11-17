using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatchFM.Models
{
    /// <summary>
    /// Class use to add history to user
    /// </summary>
    public class HistorySubmissionBindingModel
    {
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
    }
}