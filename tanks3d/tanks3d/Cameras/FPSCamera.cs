/* *************************************************************************
 * 
 * This code is based on Grootjans' QuakeCamera class from Chapter 2 of the
 * XNA 3.0 Game Programming Recipes book.
 * 
 * *************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tanks3d.Cameras
{
    public class FPSCamera
    {
        enum CameraState
        {
            Standard,
            BulletView
        };

        Vector3 cameraVelocity = Vector3.Zero;

        CameraState currentState = CameraState.Standard;

        Matrix viewMatrix;
        Matrix projectionMatrix;
        Viewport viewPort;

        int held = 0;

        float leftrightRot;
        float updownRot;
        const float rotationSpeed = 0.005f;
        Vector3 cameraPosition;
        MouseState previousMouseState;

        private Game1 game;

        public FPSCamera(Game1 g, Viewport viewPort)
            : this(g, viewPort, new Vector3(0, 1, 15), 0, 0)
        {
            //calls the constructor below with default startingPos and rotation values
        }

        public FPSCamera(Game1 g, Viewport viewPort, Vector3 startingPos, float lrRot, float udRot)
        {
            this.game = g;
            this.leftrightRot = lrRot;
            this.updownRot = udRot;
            this.cameraPosition = startingPos;

            UpdateViewport(viewPort);
            UpdateViewMatrix();

            Mouse.SetPosition(viewPort.Width / 2, viewPort.Height / 2);
            previousMouseState = Mouse.GetState();
        }

        public void UpdateViewport(Viewport newViewport)
        {
            this.viewPort = newViewport;
            float viewAngle = MathHelper.PiOver4;
            float nearPlane = 0.5f;
            float farPlane = 5000.0f;


            //float ratio = (float)viewPort.Width / (float)viewPort.Height;
            //float ratio = (float)game.GraphicsDevice.DisplayMode.Height / (float)game.GraphicsDevice.DisplayMode.Width;
            float ratio = 16.0f / 9.0f;

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(viewAngle, ratio, nearPlane, farPlane);
        }

        public void Update(MouseState currentMouseState, KeyboardState keyState)
        {
            switch (currentState)
            {
                case CameraState.Standard:
                    if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        previousMouseState = currentMouseState;
                    }

                    if (currentMouseState.LeftButton == ButtonState.Pressed && currentMouseState != previousMouseState)
                    {
                        float xDifference = currentMouseState.X - previousMouseState.X;
                        float yDifference = currentMouseState.Y - previousMouseState.Y;
                        leftrightRot += rotationSpeed * xDifference;
                        updownRot += rotationSpeed * yDifference;
                        UpdateViewMatrix();
                    }

                    previousMouseState = currentMouseState;

                    if (keyState.IsKeyDown(Keys.Up))      //Forward
                        AddToCameraPosition(new Vector3(0, 0, -1));
                    if (keyState.IsKeyDown(Keys.Down))    //Backward
                        AddToCameraPosition(new Vector3(0, 0, 1));
                    if (keyState.IsKeyDown(Keys.Right))   //Right
                        AddToCameraPosition(new Vector3(1, 0, 0));
                    if (keyState.IsKeyDown(Keys.Left))    //Left
                        AddToCameraPosition(new Vector3(-1, 0, 0));
                    if (keyState.IsKeyDown(Keys.Q))                                     //Up
                        AddToCameraPosition(new Vector3(0, 1, 0));
                    if (keyState.IsKeyDown(Keys.Z))                                     //Down
                        AddToCameraPosition(new Vector3(0, -1, 0));
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        held++;
                        if (held > 50)
                        {
                            currentState = CameraState.BulletView;
                            cameraVelocity = new Vector3(5, 50, 0);
                            Console.WriteLine("HI!");
                            held = 0;
                        }
                        Console.WriteLine("Held: " + held);
                    }


                    break;

                case CameraState.BulletView:
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        held++;
                        if (held > 50)
                        {
                            currentState = CameraState.Standard;
                            held = 0;
                        }
                    }
                    AddToCameraPosition(cameraVelocity);
                    cameraVelocity -= new Vector3(0, 2, 0);
                    if (cameraPosition.Y <= 0)
                        currentState = CameraState.Standard;
                    break;
                default:
                    break;
            }


        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            float moveSpeed = 0.5f;
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = Vector3.Zero;
            Vector3 cameraOriginalUpVector = Vector3.Zero;

            switch (currentState)
            {
                case CameraState.Standard:
                    cameraOriginalTarget = new Vector3(0, 0, -1);
                    cameraOriginalUpVector = new Vector3(0, 1, 0);
                    break;
                case CameraState.BulletView:
                    Vector3 unitVec = Vector3.Normalize(cameraVelocity);
                    cameraOriginalTarget = cameraPosition + (unitVec * 10);
                    cameraOriginalUpVector = new Vector3(0, 1, 0);
                    //cameraOriginalUpVector = -Vector3.Cross(Vector3.Cross(unitVec, Vector3.Up), unitVec);
                    break;
                default:
                    break;
            }

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);
            Vector3 cameraFinalUpVector = cameraPosition + cameraRotatedUpVector;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        public float UpDownRot
        {
            get { return updownRot; }
            set { updownRot = value; }
        }

        public float LeftRightRot
        {
            get { return leftrightRot; }
            set { leftrightRot = value; }
        }

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }
        public Vector3 Position
        {
            get { return cameraPosition; }
            set
            {
                cameraPosition = value;
                UpdateViewMatrix();
            }
        }
        public Vector3 TargetPosition
        {
            get
            {
                Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
                Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
                Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
                Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;
                return cameraFinalTarget;
            }

            set
            {
                Vector3 sideVec = Vector3.Cross(Vector3.Up, value);
                UpVector = Vector3.Cross(value, sideVec);
            }
        }
        public Vector3 Forward
        {
            get
            {
                Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
                Vector3 cameraForward = new Vector3(0, 0, -1);
                Vector3 cameraRotatedForward = Vector3.Transform(cameraForward, cameraRotation);
                return cameraRotatedForward;
            }
        }
        public Vector3 SideVector
        {
            get
            {
                Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
                Vector3 cameraOriginalSide = new Vector3(1, 0, 0);
                Vector3 cameraRotatedSide = Vector3.Transform(cameraOriginalSide, cameraRotation);
                return cameraRotatedSide;
            }
        }

        private Vector3 CameraUpVector;

        public Vector3 UpVector
        {
            get
            {
                Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
                Vector3 cameraOriginalUp = new Vector3(0, 1, 0);
                Vector3 cameraRotatedUp = Vector3.Transform(cameraOriginalUp, cameraRotation);
                return cameraRotatedUp;
            }

            set
            {
                CameraUpVector = value;
            }
        }
    }
}
