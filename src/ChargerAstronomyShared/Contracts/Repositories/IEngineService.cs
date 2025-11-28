using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Heat;
using ChargerAstronomyShared.Domain.Index;
using ChargerAstronomyShared.Domain.SpatialIndex;
using ChargerAstronomyShared.Contracts.Streaming;
using System.Collections.Concurrent;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    public interface IEngineService<T> where T : IHorizontal
    {

        IEquatorialCalculator StartServices();


        void Step(float deltaTime, float camX, float camY, float camZ, float horizontalFOV, float magnitudeThreshold, float speedMult);

        void PlaceStars(); // for initialization purposes

        EngineStats GetStats();

        /// <summary>
        /// The spatial index containing all stars organized by tile.
        /// </summary>
        SpatialStarIndex<T> SpatialStarIndex { get; }

        /// <summary>
        /// Queue for stars that need to be activated (made visible).
        /// </summary>
        BlockingCollection<T> ActivationQueue { get; }

        /// <summary>
        /// Queue for stars that need to be deactivated (made invisible).
        /// </summary>
        BlockingCollection<T> DeactivationQueue { get; }

        /// <summary>
        /// Queue for stars that need their transforms updated.
        /// </summary>
        BlockingCollection<T> UpdateTransformQueue { get; }
    }
}