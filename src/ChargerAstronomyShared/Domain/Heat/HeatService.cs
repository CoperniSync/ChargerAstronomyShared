using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Heat
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Index;
    using System.Threading.Tasks;

    public sealed class HeatService
    {
        public readonly HeatMap heatMap;
        public readonly ITileIndex index;

        /// <summary>
        /// A temporary collection used to store <see cref="TileId"/> instances during heat calculations.
        /// </summary>
        private readonly List<TileId> scratch = new List<TileId>(capacity: 256);

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatService"/> class with the specified heat map and tile
        /// index.
        /// </summary>
        /// <param name="heatMap">The heat map used to manage and analyze heat data.</param>
        /// <param name="index">The tile index used for spatial data organization and lookup.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="heatMap"/> or <paramref name="index"/> is <see langword="null"/>.</exception>
        public HeatService(HeatMap heatMap, ITileIndex index)
        {
            this.heatMap = heatMap ?? throw new ArgumentNullException(nameof(heatMap));
            this.index = index ?? throw new ArgumentNullException(nameof(index));
        }

        /// <summary>
        /// Retrieves the current heat map.
        /// </summary>
        /// <returns>The <see cref="HeatMap"/> instance representing the current heat map data.</returns>
        public HeatMap GetHeatMap() => heatMap;

        /// <summary>
        /// Retrieves the current tile index.
        /// </summary>
        /// <returns>An object implementing <see cref="ITileIndex"/> that represents the current tile index.</returns>
        public ITileIndex GetTileIndex() => index;

        /// <summary>
        /// Updates the heat map and processes tile selection based on the provided time step.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet"> <item>
        /// <description>Applies a decay step to the heat map using the specified <paramref
        /// name="deltaTime"/>.</description> </item> <item> <description>Selects tiles based on a simulated camera
        /// direction and field of view, storing the results in a temporary collection.</description> </item> <item>
        /// <description>Updates the heat map values for the selected tiles, ensuring they meet a minimum observed
        /// threshold.</description> </item> </list></remarks>
        /// <param name="deltaTime">The time step, in seconds, used to update the heat map decay.</param>
        public async Task Step(float deltaTime, Vector3 cameraDirection, float horizontalFOV)
        {
            
            // ignore these for now, just default until we get real ones
            
            heatMap.StepDecay(deltaTime);
            scratch.Clear();
            
            TileSelector.Select(index, cameraDirection, horizontalFOV, scratch);

            const float observed = 1.0f;
            for (int i = 0; i < scratch.Count; i++)
            {
                // this is for the future where we might set different heat values to tiles depending on selector confidence

                var id = scratch[i];
                float prev = heatMap.Get(id);
                if (prev < observed)
                    heatMap.Set(scratch, 1.0f);
            }
        }
    }
}
