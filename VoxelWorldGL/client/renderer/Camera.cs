using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VoxelWorldGL.client.renderer
{
	public class Camera : GameComponent
	{
		public Matrix View { get; protected set; }
		public Matrix Projection { get; protected set; }

		public Vector3 CameraPosition { get; protected set; }
		private Vector3 _cameraDirection;
		private Vector3 _cameraUp;

		//defines speed of camera movement
		private readonly float _moveSpeed = 1.0F;

		private MouseState _prevMouseState;

		public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
			: base(game)
		{
			// TODO: Construct any child components here

			// Build camera view matrix
			CameraPosition = pos;
			_cameraDirection = target - pos;
			_cameraDirection.Normalize();
			_cameraUp = up;
			CreateLookAt();

			Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
				Game.Window.ClientBounds.Width / (float) Game.Window.ClientBounds.Height, 1, 1000);
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			// TODO: Add your initialization code here
			// Set mouse position and do initial get state
			Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);

			_prevMouseState = Mouse.GetState();

			base.Initialize();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update code here
			if (Game.IsActive)
			{
				// Move forward and backward
				if (Keyboard.GetState().IsKeyDown(Keys.W))
					CameraPosition += _cameraDirection * _moveSpeed;
				if (Keyboard.GetState().IsKeyDown(Keys.S))
					CameraPosition -= _cameraDirection * _moveSpeed;

				if (Keyboard.GetState().IsKeyDown(Keys.A))
					CameraPosition += Vector3.Cross(_cameraUp, _cameraDirection) * _moveSpeed;
				if (Keyboard.GetState().IsKeyDown(Keys.D))
					CameraPosition -= Vector3.Cross(_cameraUp, _cameraDirection) * _moveSpeed;

				// Rotation in the world
				_cameraDirection = Vector3.Transform(_cameraDirection,
					Matrix.CreateFromAxisAngle(_cameraUp,
						(-MathHelper.PiOver4 / 150) * (Mouse.GetState().X - _prevMouseState.X)));

				_cameraDirection = Vector3.Transform(_cameraDirection,
					Matrix.CreateFromAxisAngle(Vector3.Cross(_cameraUp, _cameraDirection),
						(MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - _prevMouseState.Y)));
				_cameraUp = Vector3.Transform(_cameraUp,
					Matrix.CreateFromAxisAngle(Vector3.Cross(_cameraUp, _cameraDirection),
						(MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - _prevMouseState.Y)));

				// Reset prevMouseState
				if (Mouse.GetState() != _prevMouseState)
					Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);

				_prevMouseState = Mouse.GetState();
			}

			CreateLookAt();

			base.Update(gameTime);
		}

		private void CreateLookAt()
		{
			View = Matrix.CreateLookAt(CameraPosition, CameraPosition + _cameraDirection, _cameraUp);
		}
	}
}