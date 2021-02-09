using Newtonsoft.Json;

namespace Cirara.Models.Requests.Pet
{
    public class PetControllerRootPost
    {
        /// <summary>
        ///     Guid of the user that should be assigned the new pet
        /// </summary>
        [JsonProperty(nameof(UserIdentifier))]
        public string UserIdentifier { get; set; }
    }
}