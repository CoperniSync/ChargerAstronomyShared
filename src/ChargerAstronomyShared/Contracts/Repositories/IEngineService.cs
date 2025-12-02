using ChargerAstronomyShared.Contracts.Models;
using ChargerAstronomyShared.Domain.Heat;
using ChargerAstronomyShared.Domain.Index;
using ChargerAstronomyShared.Domain.SpatialIndex;
using ChargerAstronomyShared.Contracts.Streaming;
using System.Collections.Concurrent;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    /// <summary>
    /// Service interface for the engine.
    /// </summary>
    /// <typeparam name="T"><see cref="ITileIndex"/> used in the current engine service.</typeparam>
    public interface IEngineService<T> where T : IHorizontal
    {

        IEquatorialCalculator StartServices();

        /// <summary>
        /// Steps the engine simulation forward by the specified delta time.
        /// </summary>
        /// <param name="deltaTime">The amount of time passed during the step.</param>
        /// <param name="camX">X value of the camera.</param>
        /// <param name="camY">Y value of the camera.</param>
        /// <param name="camZ">Z value of the camera.</param>
        /// <param name="horizontalFOV">The FOV (zoom) of the camera.</param>
        /// <param name="magnitudeThreshold">Maximum threshold for star visibility.</param>
        /// <param name="speedMult">The current speed of the simulation.</param>
        void Step(float deltaTime, float camX, float camY, float camZ, float horizontalFOV, float magnitudeThreshold, float speedMult);

        /// <summary>
        /// Places stars in their initial positions.
        /// </summary>
        void PlaceStars(); // for initialization purposes


        /// <summary>
        /// Updates a star's horiziontal position
        /// </summary>
        /// <param name="star"> The star to be updated </param>
        void ForceStarUpdate(T star);

        /// <summary>
        /// Gets current engine statistics.
        /// </summary>
        /// <returns>An <see cref="EngineStats"/> object.</returns>
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