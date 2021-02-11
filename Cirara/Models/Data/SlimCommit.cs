using System;

namespace Cirara.Models.Data
{
    public class SlimCommit
    {
        /// <summary>
        ///     Name of the Commit Author
        /// </summary>
        public string Commiter { get; set; }

        /// <summary>
        ///     Short message of the commit
        /// </summary>
        public string CommitMessage { get; set; }

        /// <summary>
        ///     Date of when the commit was done
        /// </summary>
        public DateTimeOffset CommitDate { get; set; }

        /// <summary>
        /// SHA-1 hash of the commit
        /// </summary>
        public string CommitHash { get; set; }
    }
}