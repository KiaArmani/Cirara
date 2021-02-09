using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cirara.Models.Data
{
    public class SlimCommit
    {
        /// <summary>
        /// Name of the Commit Author
        /// </summary>
        public string Commiter { get; set; }

        /// <summary>
        /// Short message of the commit
        /// </summary>
        public string CommitMessage { get; set; }

        /// <summary>
        /// Date of when the commit was done
        /// </summary>
        public DateTimeOffset CommitDate { get; set; }
    }
}
