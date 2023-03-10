using System;

namespace Cirara.Models.Data
{
    public class Author
    {
        /// <summary>
        ///     Unique Identifier of the <see cref="Author" /> as <see cref="Guid" />
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     Name of the Author
        /// </summary>
        public string UserName { get; set; }
    }
}