using Newtonsoft.Json;

namespace Cirara.Models.Requests.PetAction
{
    public class PetActionControllerFeedPatch
    {
        /// <summary>
        ///     Guid of the Pet that gets fed
        /// </summary>
        [JsonProperty(nameof(PetIdentifier))]
        public string PetIdentifier { get; set; }

        /// <summary>
        ///     Amount of Hunger points that will be added
        ///     Note: In a production scenario, this should be an ItemReference that will be validated instead (e.g. a steak gives
        ///     +3 instead of just 1)
        /// </summary>
        [JsonProperty(nameof(HungerImprovement))]
        public int HungerImprovement { get; set; }
    }
}