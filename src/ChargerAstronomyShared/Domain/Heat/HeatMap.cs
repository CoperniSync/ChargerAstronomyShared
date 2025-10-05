using System;
using System.Collections.Generic;
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

        public void Set(in TileId id, float value)
        {
            var v = Math.Clamp(value, config.ClampMin, config.ClampMax);
            heatMap[id] = v;
            
            // no removing tiles from heatmap, for now atleast
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

        public void StepDecay(float dtSeconds)
        {

            // exponenitaly decay instead of linear decay
            // dHeat/dt = -Heat * decayPerSecond
            // Heat(t+dt) = Heat(t) * exp(-decayPerSecond * dt)

            if (dtSeconds <= 0) return;
            var k = MathF.Exp(-config.DecayPerSecond * dtSeconds);

            // copy keys to avoid iterator invalidation if we remove entries
            var keys = new List<TileId>(heatMap.Keys);
            foreach (var id in keys)
            {
                var v = heatMap[id] * k;
                heatMap[id] = Math.Clamp(v, config.ClampMin, config.ClampMax);
            }
        }

        public List<TileId> TilesAbove(float value, bool inclusive = true)
        {
            var res = new List<TileId>();
            foreach (var (id, heat) in heatMap)
            {
                if (inclusive ? heat >= value : heat > value)
                    res.Add(id);
            }
            return res;
        }

        public List<TileId> TilesBelow(float value, bool inclusive = true)
        {
            var res = new List<TileId>();
            foreach (var (id, heat) in heatMap)
            {
                if (inclusive ? heat <= value : heat < value)
                    res.Add(id);
            }
            return res;
        }

        public List<TileId> TilesInRange(float lower, float upper, bool inclusive = true)
        {
            var res = new List<TileId>();
            foreach (var (id, heat) in heatMap)
            {
                bool inInc = lower <= heat && heat <= upper;
                bool inExc = lower <  heat && heat <  upper;
                if (inclusive ? inInc : inExc) res.Add(id);
            }
            return res;
        }
    }
}
