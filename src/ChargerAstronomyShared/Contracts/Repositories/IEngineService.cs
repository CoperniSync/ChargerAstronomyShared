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

        /// <summary>
        /// Starts and initializes the engine services for the application.
        /// </summary>
        /// <returns>An instance of <see cref="IEquatorialCalculator"/> representing the initialized service.</returns>
        IEquatorialCalculator StartServices();

        /// <summary>
        /// Advances the simulation by a single step, updating the state based on the elapsed time and camera direction.
        /// </summary>
        /// <param name="deltaTime">The time, in seconds, that has elapsed since the last step. Must be greater than zero.</param>
        /// <param name="cameraDirection">The direction the camera is facing, represented as a 3D vector.</param>
        /// <param name="horizontalFOV">The horizontal field of view, in degrees, used to determine the visible area. Must be a positive value.</param>
        /// <returns>A task that represents the asynchronous operation of stepping the simulation.</returns>
        public Task Step(float deltaTime, Vector3 cameraDirection, float horizontalFOV);

        public BlockingCollection<T> ActivationQueue { get; }
        public BlockingCollection<T> DeactivationQueue { get; }
        public BlockingCollection<T> UpdateTransformQueue { get; }
    }
}
