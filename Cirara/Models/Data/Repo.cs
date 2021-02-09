using System.Collections.Generic;

namespace Cirara.Models.Data
{
    public class Repo
    {
        /// <summary>
        ///     Name of the repository
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     List of branches for this repository
        /// </summary>
        public List<Branch> Branches { get; set; }
    }
}