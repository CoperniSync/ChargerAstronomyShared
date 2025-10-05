using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChargerAstronomyShared.Domain.Heat
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Domain.Index;

    public sealed class HeatService
    {
        public readonly HeatMap heatMap;
        public readonly ITileIndex index;

        private readonly List<TileId> scratch = new List<TileId>(capacity: 256);

        public HeatService(HeatMap heatMap, ITileIndex index)
        {
            this.heatMap = heatMap ?? throw new ArgumentNullException(nameof(heatMap));
            this.index = index ?? throw new ArgumentNullException(nameof(index));
        }

        public HeatMap GetHeatMap() => heatMap;

        public ITileIndex GetTileIndex() => index;

        public void Step(float deltaTime)
        {
            
            // ignore these for now, just default until we get real ones
            Vector3 cameraDirection = Vector3.One;
            float fov = 45.0f * (float)(Math.PI / 180.0);

            heatMap.StepDecay(deltaTime);
            

            scratch.Clear();
            TileSelector.Select(index, cameraDirection, fov, scratch);

            const float observed = 1.0f;
            for (int i = 0; i < scratch.Count; i++)
            {
                // this is for the future where we might set different heat values to tiles depending on selector confidence

                var id = scratch[i];
                float prev = heatMap.Get(id);
                if (prev < observed)
                    heatMap.Set(scratch, 1.0f);
            }

            // there might be more things we do in this service in the future but for now this is all we need
        }
    }
}
