using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoxelWorldGL.block.blocks;
using VoxelWorldGL.client.renderer;
using VoxelWorldGL.world.chunk;

namespace VoxelWorldGL.world
{
	public class WorldRenderer
	{
		public readonly Camera Camera;
		private volatile BasicEffect _basicEffect;
		private readonly Matrix _worldView = Matrix.CreateTranslation(0, 0, 0);
		private readonly World _world;
		private readonly GraphicsDevice _graphicsDevice;
		private DynamicVertexBuffer _vertexBuffer1;
		public List<VertexPositionColor> Vertices { get; set; } = new List<VertexPositionColor>();

		public WorldRenderer(World world, GraphicsDevice graphicsDevice)
		{
			_world = world;
			_graphicsDevice = graphicsDevice;
			Camera = new Camera(world.Game, Vector3.Zero, Vector3.UnitX, Vector3.Up);
			Camera.Initialize();
			_basicEffect = new BasicEffect(graphicsDevice);
			Init();
		}

		public void Init()
		{
			foreach (Chunk c in _world.Chunks)
			{
				Vertices.AddRange(c.Renderer.Vertices);
			}

			_vertexBuffer1 = new DynamicVertexBuffer(_graphicsDevice, typeof(VertexPositionColor), Vertices.Count * 2,
				BufferUsage.WriteOnly);
			SetVertexData();
		}

		public void SetVertexData()
		{
			_vertexBuffer1.SetData(Vertices.ToArray());
		}

		public void Draw()
		{
			_graphicsDevice.Clear(Color.CornflowerBlue);

			_basicEffect.World = _worldView;
			_basicEffect.View = Camera.View;
			_basicEffect.Projection = Camera.Projection;
			_basicEffect.VertexColorEnabled = true;

			_graphicsDevice.SetVertexBuffer(_vertexBuffer1);

			RasterizerState rasterizerState = new RasterizerState {CullMode = CullMode.None};
			_graphicsDevice.RasterizerState = rasterizerState;

			foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Vertices.Count / 3);
			}
		}
	}
}