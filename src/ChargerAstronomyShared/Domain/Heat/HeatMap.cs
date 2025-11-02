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

        /// <summary>
        /// Retrieves the heat map value for the given tile.
        /// </summary>
        /// <param name="id">The tile for which to retrieve the heat map value.</param>
        /// <returns>The heat map value associated with the specified <paramref name="id"/> if it exists; otherwise, <see
        /// langword="0f"/>.</returns>
        public float Get(in TileId id) => heatMap.TryGetValue(id, out var x) ? x : 0f;

        /// <summary>
        /// Sets the heat value for the given tile, or list of tiles.
        /// </summary>
        /// <param name="id">A <see cref="TileId"/> object to set the heat value of.</param>
        /// <param name="value">The heat value assigned to the tile.</param>
        public void Set(in TileId id, float value)
        {
            var v = Math.Clamp(value, config.ClampMin, config.ClampMax);
            heatMap[id] = v;
            
            // no removing tiles from heatmap, for now atleast
        }

        /// <summary>
        /// Sets the heat value for a collection of tiles.
        /// </summary>
        /// <param name="ids">A collection of <see cref="TileId"/>s.</param>
        /// <param name="value">The heat value assigned to each tile in the list.</param>
        public void Set(IEnumerable<TileId> ids, float value)
        {
            foreach (var id in ids) Set(id, value);
        }

        /// <summary>
        /// Applies a delta value onto the current value associated with the specified tile.
        /// </summary>
        /// <remarks> Retrieves the current value associated with the specified <paramref
        /// name="TileId"/>, adds the provided <paramref name="delta"/> to it, and updates the value.</remarks>
        /// <param name="id">A <see cref="TileId"/> object.</param>
        /// <param name="delta">The value to add to the current value of the specified tile.</param>
        public void Apply(in TileId id, float delta)
        {
            Set(id, Get(id) + delta);
        }

        /// <summary>
        /// Applies a delta value to a collection of updates to tiles.
        /// </summary>
        /// <remarks>This method iterates through the provided collection and applies each delta to the
        /// corresponding tile. Ensure that the collection is not null and contains valid tile identifiers and delta
        /// values.</remarks>
        /// <param name="deltas">A collection of tuples, each containing a <see cref="TileId"/> representing the tile to update and a <see
        /// cref="float"/> representing the delta value to apply to the tile.</param>
        public void Apply(IEnumerable<(TileId id, float delta)> deltas)
        {
            foreach (var (id, d) in deltas) Apply(id, d);
        }

        /// <summary>
        /// Applies an exponential decay to the heat values in the heat map over a specified time interval.
        /// </summary>
        /// <remarks>This method updates the heat values in the heat map by applying an exponential decay
        /// formula: <c>Heat(t + dt) = Heat(t) * exp(-decayPerSecond * dt)</c>. The decay rate is determined by the
        /// <c>DecayPerSecond</c> configuration value.</remarks>
        /// <param name="dtSeconds">The time interval, in seconds, over which the decay is applied. Must be greater than 0.</param>
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

       /// <summary>
       /// Retrieves a list of tiles that have a heat value above the specified threshold.
       /// </summary>
       /// <param name="value">The threshold value to compare against the heat values of the tiles.</param>
       /// <param name="inclusive">A boolean indicating whether the comparison should include tiles with heat values equal to the threshold. If
       /// <see langword="true"/>, tiles with heat values equal to <paramref name="value"/> are included;  otherwise,
       /// only tiles with heat values strictly greater than <paramref name="value"/> are included.</param>
       /// <returns>A list of <see cref="TileId"/> objects representing the tiles that meet the specified heat value condition.
       /// The list will be empty if no tiles satisfy the condition.</returns>
        public IEnumerable<TileId> TilesAbove(float value, bool inclusive = true)
        {
            return heatMap.Where((id, heat) => inclusive ? heat >= value : heat > value).Select(kvp => kvp.Key);
        }

        /// <summary>
        /// Retrieves a list of tiles that have a heat value below a specified threshold.
        /// </summary>
        /// <param name="value">The threshold value to compare against the heat values of the tiles.</param>
        /// <param name="inclusive">A boolean indicating whether tiles with heat values equal to the threshold should be included. If <see
        /// langword="true"/>, tiles with heat values less than or equal to <paramref name="value"/> are included;
        /// otherwise, only tiles with heat values strictly less than <paramref name="value"/> are included.</param>
        /// <returns>A list of <see cref="TileId"/> objects representing the tiles that meet the specified condition. The list
        /// will be empty if no tiles satisfy the condition.</returns>
        public IEnumerable<TileId> TilesBelow(float value, bool inclusive = true)
        {
            return heatMap.Where((id, heat) => inclusive ? heat <= value : heat < value).Select(kvp => kvp.Key);
        }

        /// <summary>
        /// Retrieves a list of tiles that have heat values that fall within the specified range.
        /// </summary>
        /// <param name="lower">The lower bound of the heat value range.</param>
        /// <param name="upper">The upper bound of the heat value range.</param>
        /// <param name="inclusive">A boolean value indicating whether the range is inclusive.  If <see langword="true"/>, tiles with heat
        /// values equal to the lower or upper bounds are included;  otherwise, only tiles with heat values strictly
        /// between the bounds are included.</param>
        /// <returns>A list of <see cref="TileId"/> objects representing the tiles that have heat values within the specified
        /// range. The list will be empty if no tiles match the criteria.</returns>
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

        public IEnumerable<TileId> InactiveTilesAboveZero()
        {
            return TilesAbove(0f).Where(id => !id.active);
        }

        public IEnumerable<TileId> ActiveTileAtZero()
        {
            return TilesBelow(0f, true).Where(id => id.active);
        }
    }
}
