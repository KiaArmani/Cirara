using Newtonsoft.Json;

namespace Cirara.Models.Requests.User
{
    public class UserControllerRootPut
    {
        /// <summary>
        ///     GUID of the <see cref="User" /> as string
        /// </summary>
        [JsonProperty(nameof(UserIdentifier))]
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     New username of the <see cref="User" />
        /// </summary>
        [JsonProperty(nameof(UserName))]
        public string UserName { get; set; }
    }
}