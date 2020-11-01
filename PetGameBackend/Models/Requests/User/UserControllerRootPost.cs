﻿using Newtonsoft.Json;

namespace PetGameBackend.Models.Requests.User
{
    public class UserControllerRootPost
    {
        /// <summary>
        ///     GUID of the <see cref="User" /> as string
        /// </summary>
        [JsonProperty(nameof(UserIdentifier))]
        public string UserIdentifier { get; set; }
    }
}