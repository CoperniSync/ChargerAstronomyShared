using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Heat;
using ChargerAstronomyShared.Domain.Index;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    public interface IEngineService
    {
        IEquatorialCalculator StartServices();

        public Task Step(float deltaTime, Vector3 cameraDirection, float horizontalFOV);

        public BlockingCollection<TileId> ActivationQueue { get; }
        public BlockingCollection<TileId> DeactivationQueue { get; }
        public BlockingCollection<TileId> UpdateTransformQueue { get; }
    }
}
