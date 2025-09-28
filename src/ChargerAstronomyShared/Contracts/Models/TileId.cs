using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public readonly struct TileId : IEquatable<TileId> // VS auto completed/added this IEquatable thing, and it seems useful so ill keep it 
    {
        public readonly int Index;
        public TileId(int index) => Index = index;
        public bool Equals(TileId tileId) => Index == tileId.Index;
        public override bool Equals(object obj) => obj is TileId tileId && Equals(tileId);
        public override int GetHashCode() => Index.GetHashCode();
    }
}
