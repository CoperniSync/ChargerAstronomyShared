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
    public interface IEngineService <T> where T : IHorizontal
    {
        IEquatorialCalculator StartServices();

        public Task Step(float deltaTime, Vector3 cameraDirection, float horizontalFOV);

        public BlockingCollection<T> ActivationQueue { get; }
        public BlockingCollection<T> DeactivationQueue { get; }
        public BlockingCollection<T> UpdateTransformQueue { get; }
    }
}
