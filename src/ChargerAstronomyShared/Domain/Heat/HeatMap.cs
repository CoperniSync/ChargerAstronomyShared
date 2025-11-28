using System;
using System.Collections.Generic;
using System.Linq;
using ChargerAstronomyShared.Contracts.Models;

namespace ChargerAstronomyShared.Domain.Heat
{
    public sealed class HeatMap
    {
        private readonly HeatConfig config;
        private readonly Dictionary<TileId, float> heatMap = new Dictionary<TileId, float>(1 << 15);

        public HeatMap(HeatConfig cfg)
        {
            config = cfg ?? throw new ArgumentNullException(nameof(cfg));
        }

        public float Get(in TileId id) => heatMap.TryGetValue(id, out var x) ? x : 0f;

        public Dictionary<TileId, float> GetMany(IEnumerable<TileId> ids)
        {
            var result = new Dictionary<TileId, float>();
            foreach (var id in ids)
            {
                result[id] = Get(id);
            }
            return result;
        }

        public void Set(in TileId id, float value)
        {
            var v = Math.Clamp(value, config.ClampMin, config.ClampMax);
            heatMap[id] = v;
        }

        public void Set(IEnumerable<TileId> ids, float value)
        {
            foreach (var id in ids) Set(id, value);
        }

        public void Apply(in TileId id, float delta)
        {
            Set(id, Get(id) + delta);
        }

        public void Apply(IEnumerable<(TileId id, float delta)> deltas)
        {
            foreach (var (id, d) in deltas) Apply(id, d);
        }

        /// <summary>
        /// Applies decay to the heat values.
        /// Can use exponential decay OR instant decay based on config.
        /// </summary>
        public void StepDecay(float dtSeconds)
        {
            if (dtSeconds <= 0) return;

            if (config.UseInstantDecay)
            {
                // INSTANT DECAY: Set all heat to zero
                // Tiles will only be hot if they're actively visible THIS frame
                var keys = new List<TileId>(heatMap.Keys);
                foreach (var id in keys)
                {
                    heatMap[id] = 0f;
                }
            }
            else
            {
                // EXPONENTIAL DECAY: Heat cools down over time
                // Heat(t+dt) = Heat(t) * exp(-decayPerSecond * dt)
                var k = MathF.Exp(-config.DecayPerSecond * dtSeconds);

                var keys = new List<TileId>(heatMap.Keys);
                foreach (var id in keys)
                {
                    var v = heatMap[id] * k;
                    heatMap[id] = Math.Clamp(v, config.ClampMin, config.ClampMax);
                }
            }
        }

        public IEnumerable<TileId> TilesAbove(float value, bool inclusive = true)
        {
            return heatMap.Where(kvp => inclusive ? kvp.Value >= value : kvp.Value > value).Select(kvp => kvp.Key);
        }

        public IEnumerable<TileId> TilesBelow(float value, bool inclusive = true)
        {
            return heatMap.Where(kvp => inclusive ? kvp.Value <= value : kvp.Value < value).Select(kvp => kvp.Key);
        }

        public List<TileId> TilesInRange(float lower, float upper, bool inclusive = true)
        {
            var res = new List<TileId>();
            foreach (var (id, heat) in heatMap)
            {
                bool inInc = lower <= heat && heat <= upper;
                bool inExc = lower < heat && heat < upper;
                if (inclusive ? inInc : inExc) res.Add(id);
            }
            return res;
        }

        public IEnumerable<TileId> InactiveTilesAboveZero()
        {
            return TilesAbove(0f).Where(id => !id.active);
        }

        public IEnumerable<TileId> ActiveTileAtZero()
        {
            return TilesBelow(0f, true).Where(id => id.active);
        }

        public IEnumerable<TileId> GetAllTrackedTiles()
        {
            return heatMap.Keys;
        }

        public void Clear()
        {
            heatMap.Clear();
        }
    }
}