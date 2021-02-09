using System.Collections.Generic;

namespace Cirara.Models.Data
{
    public class Branch
    {
        /// <summary>
        ///     Name of the Branch
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     List of Commits of this branch
        /// </summary>
        public List<SlimCommit> Commits { get; set; }
    }
}