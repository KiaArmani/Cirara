using Newtonsoft.Json;

namespace Cirara.Models.Requests.PetAction
{
    public class PetActionControllerStrokePatch
    {
        /// <summary>
        ///     Guid of the Pet that gets feed
        /// </summary>
        [JsonProperty(nameof(PetIdentifier))]
        public string PetIdentifier { get; set; }

        /// <summary>
        ///     Amount of Hunger points that will be added
        ///     Note: In a production scenario, this should be an ItemReference that will be validated instead (e.g. a hairbrush
        ///     gives +2 instead of just 1)
        /// </summary>
        [JsonProperty(nameof(HappinessImprovement))]
        public int HappinessImprovement { get; set; }
    }
}