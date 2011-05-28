using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    /// <summary>
    /// This class is in charge of handling gravity, collisions, etc. for all the objects in the game.
    /// In order for an object to be influenced by physics, it must implement the IPhysicsObject interface
    /// and be added to the physicsObjects list in this class. This class should take care of the rest.
    /// </summary>
    public class PhysicsEngine : GameComponent
    {
        private Game1 game;

        /// <summary>
        /// Lists of all the objects being controlled by the physics engine.
        /// </summary>
        private List<IPhysicsObject> physicsObjects;

        /// <summary>
        /// The global time - how long the physics simulation has been running.
        /// </summary>
        private float t;

        private PhysicsIntegrator integrator;

        public PhysicsEngine(Game1 g, IntegrationMethod integrationMethod)
            : base(g)
        {
            game = g;
            physicsObjects = new List<IPhysicsObject>();
            t = 0.0f;

            switch (integrationMethod)
            {
                case IntegrationMethod.Euler:
                    this.integrator = new EulerIntegrator();
                    break;
                case IntegrationMethod.RungeKutta4:
                    this.integrator = new RK4Integrator();
                    break;
                default:
                    this.integrator = new EulerIntegrator();
                    break;
            }
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Add an object to the list of objects the physics engine is controlling.
        /// </summary>
        /// <param name="obj"></param>
        public void AddPhysicsObject(IPhysicsObject obj)
        {
            physicsObjects.Add(obj);
        }

        public override void Update(GameTime gameTime)
        {
            switch (game.currentState1)
            {
                case Game1.GameState1.Play:
                    
                    float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    t += elapsedSeconds;

                    foreach (IPhysicsObject physicsObject in physicsObjects)
                    {
                        Vector3 initialPosition = physicsObject.GetPosition();
                        Vector3 initialVelocity = physicsObject.GetVelocity();
                        Vector3 gravity = PhysicsUtil.GravityConstant * Vector3.Down;
                        State initialState = new State(initialPosition, initialVelocity);

                        State finalState = integrator.integrate(initialState, t, elapsedSeconds, gravity);

                        physicsObject.UpdatePosition(finalState.Position);
                        physicsObject.UpdateVelocity(finalState.Velocity);

                        DoCollisionDetectionWithTerrain(physicsObject);

                        //System.Console.WriteLine(physicsObject.GetPosition());
                    }

                    base.Update(gameTime);
                    break;
            }
        }

        private void DoCollisionDetectionWithTerrain(IPhysicsObject physicsObject)
        {
            Vector3 position = physicsObject.GetPosition();

            if (game.heightMapInfo.IsOnHeightmap(position))
            {                
                float objectBottom = physicsObject.GetBoundingBox().Min.Y;

                //Utility.BoundingBoxRenderer.Render(game, physicsObject.GetBoundingBox(), game.GraphicsDevice, game.worldCamera.ViewMatrix, game.worldCamera.ProjectionMatrix, Color.White);

                Vector3 v = physicsObject.GetVelocity();

                // In the bounding box, the bottom corners have indices 2,3,6,7. We're
                // interested in checking just the collisions of the bottom corners of
                // the object with the terrain.

                Vector3[] corners = physicsObject.GetBoundingBox().GetCorners();
                int[] bottomCorners = { 2, 3, 6, 7 };
                foreach (int i in bottomCorners)
                {
                    float terrainElevation;
                    Vector3 terrainNormal;

                    if (game.heightMapInfo.IsOnHeightmap(corners[i]))
                    {
                        game.heightMapInfo.GetHeightAndNormal(corners[i], out terrainElevation, out terrainNormal);
                        if (corners[i].Y <= terrainElevation)
                        {
                            HandleCollisionWithSurface(physicsObject, terrainNormal, 0.7f);

                            // Notify the physics object that a collision with the terrain had occurred.
                            physicsObject.HandleCollisionWithTerrain();

                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles a collision between the physics object and a surface described by a surface
        /// normal. The object's velocity will change such that the angle of incidence and the 
        /// angle of reflection are equal with respect to the surface normal. Note that this
        /// method does not check whether any sort of a collision actually occurred - it trusts
        /// the caller to determine that.
        /// </summary>
        /// <param name="physicsObject">The object whose velocity will change as a result of the
        /// collision.</param>
        /// <param name="surfaceNormal">The surface normal of the surface against which the object
        /// collided.</param>
        /// <param name="restitution">Coefficient of restitution. 1.0f means an elastic collision (no
        /// energy lost), so the object will bounce back as forcefully as it went in. For 0.0f, the
        /// object will effectively stop at the surface.</param>
        private void HandleCollisionWithSurface(IPhysicsObject physicsObject, Vector3 surfaceNormal,
            float restitution)
        {
            Vector3 oldVelocity = physicsObject.GetVelocity();

            // Project velocity vector onto the surface normal
            float v_dot_n = Vector3.Dot(oldVelocity, surfaceNormal);

            // Calculate the normal force
            Vector3 normalForce = -(1.0f + restitution) * v_dot_n * surfaceNormal;

            // Add the normal force to the motion vector to get the new velocity
            Vector3 newVelocity = oldVelocity + normalForce;

            physicsObject.UpdateVelocity(newVelocity);
        }
    }
}
