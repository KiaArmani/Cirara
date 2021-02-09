using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cirara.Models.Data
{
    public class Branch
    {
        /// <summary>
        /// Name of the Branch
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of Commits of this branch
        /// </summary>
        public List<SlimCommit> Commits { get; set; }
    }
}
