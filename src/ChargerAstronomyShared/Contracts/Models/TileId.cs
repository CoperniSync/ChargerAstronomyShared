using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public class TileId : IEquatable<TileId> // VS auto completed/added this IEquatable thing, and it seems useful so ill keep it 
    {
        public readonly int Index;
        public bool active;

        public TileId(int index) { Index = index; active = false; }
        public bool Equals(TileId tileId) => Index == tileId.Index;
        public override bool Equals(object obj) => obj is TileId tileId && Equals(tileId);
        public override int GetHashCode() => Index.GetHashCode();
    }
}
