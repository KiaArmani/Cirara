using Newtonsoft.Json;

namespace Cirara.Models.Requests.Pet
{
    public class PetControllerRootDelete
    {
        /// <summary>
        ///     Guid of the Pet that gets deleted
        /// </summary>
        [JsonProperty(nameof(PetIdentifier))]
        public string PetIdentifier { get; set; }
    }
}