using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Rendering
{
    public class Camera
    {

        protected GraphicsDevice GraphicsDevice { get; set; }

        protected Matrix _view;
        protected Matrix _projection;

        protected Vector3 _up = Vector3.Up;
        protected Vector3 _right = Vector3.Right;
        protected Vector3 _forward = Vector3.Backward;
        protected Vector3 _direction = Vector3.Zero;
        protected Vector3 _translation = Vector3.Zero;
        protected Vector3 _position = Vector3.Zero;
        protected Vector3 _rotation = Vector3.Zero;

        protected float _fov = MathHelper.ToRadians(70);
        protected float _aspectRatio = 1.6f;
        protected float _nearClipPlane = 0.1f;
        protected float _farClipPlane = 20000000000f;

        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }

        public Vector3 Direction { get { return _direction; } set { _direction = value; } }
        public Vector3 Up { get { return _up; } }
        public Vector3 Right { get { return _right; } }
        public Vector3 Forward { get { return _forward; } }

        public Matrix RotationMXYZ { get { return Matrix.Multiply(Matrix.Multiply(Matrix.CreateRotationX(_rotation.X), Matrix.CreateRotationY(_rotation.Y)), Matrix.CreateRotationZ(_rotation.Z)); } }
        public Matrix RotationMXY { get { return Matrix.Multiply(Matrix.CreateRotationX(_rotation.X), Matrix.CreateRotationY(_rotation.Y)); } }
        public Matrix RotationMXZ { get { return Matrix.Multiply(Matrix.CreateRotationX(_rotation.X), Matrix.CreateRotationZ(_rotation.Z)); } }
        public Matrix RotationMYZ { get { return Matrix.Multiply(Matrix.CreateRotationY(_rotation.Y), Matrix.CreateRotationZ(_rotation.Z)); } }
        public Matrix RotationMX { get { return Matrix.CreateRotationX(_rotation.X); } }
        public Matrix RotationMY { get { return Matrix.CreateRotationY(_rotation.Y); } }
        public Matrix RotationMZ { get { return Matrix.CreateRotationZ(_rotation.Z); } }

        public float RotationX { get { return _rotation.X; } set { _rotation.X = value; } }
        public float RotationY { get { return _rotation.Y; } set { _rotation.Y = value; } }
        public float RotationZ { get { return _rotation.Z; } set { _rotation.Z = value; } }

        public Camera(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            _position = new Vector3(0, 0, 0);
            GeneratePerspectiveProjectionMatrix();
        }

        protected void GeneratePerspectiveProjectionMatrix()
        {
            PresentationParameters presentationParameters = GraphicsDevice.PresentationParameters;
            _aspectRatio = (float)presentationParameters.BackBufferWidth / (float)presentationParameters.BackBufferHeight;
            _projection = Matrix.CreatePerspectiveFieldOfView(_fov, _aspectRatio, _nearClipPlane, _farClipPlane);
        }

        public void Move(Vector3 translation)
        {
            _translation += translation;
        }

        public void Teleport(Vector3 position)
        {
            _position = position;
        }

        public void Look(Vector3 rotation)
        {
            _rotation = rotation;
        }

        public void Rotate(Vector3 rotation)
        {
            _rotation += rotation;
        }

        public void RotateX(float x)
        {
            _rotation.X += MathHelper.ToRadians(x);
        }

        public void RotateZ(float z)
        {
            _rotation.Z += MathHelper.ToRadians(z);
        }

        public void RotateY(float y)
        {
            _rotation.Y += MathHelper.ToRadians(y);
        }

        public virtual void Update(GameTime gameTime)
        {
            _translation = Vector3.Transform(_translation, RotationMY);
            _position += _translation;
            _translation = Vector3.Zero;

            Vector3 forward = Vector3.Transform(Forward, RotationMXY);
            _direction = _position + forward;

            _view = Matrix.CreateLookAt(_position, _direction, _up);
        }
    }
}
