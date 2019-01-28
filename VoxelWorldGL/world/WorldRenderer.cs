using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private IndexBuffer _indexBuffer;
		private VertexBuffer _vertexBuffer;

		//private VertexBuffer _vertexBuffer2;
		//private bool vbo1 = true;
		public List<VertexPositionColor> Vertices { get; set; } = new List<VertexPositionColor>();
		public List<long> Indices { get; set; } = new List<long>();

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
			foreach (Chunk chunk in _world.Chunks)
			{
				Vertices.AddRange(chunk.Renderer.Vertices);
			}
			Vertices = Vertices.OrderBy(vertex => Vector3.Distance(vertex.Position, Vector3.Zero)).ToList();

			_vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), Vertices.Count * 2,
				BufferUsage.WriteOnly);
			/*_vertexBuffer2 = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), Vertices.Count * 2,
				BufferUsage.WriteOnly);*/
			SetIndexData();
			_indexBuffer = new IndexBuffer(_graphicsDevice, typeof(long), Indices.Count, BufferUsage.WriteOnly);
			SetVertexData();
		}

		public void SetVertexData()
		{
			/*if (!vbo1)*/
			_vertexBuffer.SetData(Vertices.ToArray());
			/*else
				_vertexBuffer2.SetData(Vertices.ToArray());
			vbo1 = !vbo1;*/
		}

		public void SetIndexData()
		{
			
		}

		public void Draw()
		{
			_graphicsDevice.Clear(Color.CornflowerBlue);

			_basicEffect.World = _worldView;
			_basicEffect.View = Camera.View;
			_basicEffect.Projection = Camera.Projection;
			_basicEffect.VertexColorEnabled = true;

			_graphicsDevice.Indices = _indexBuffer;
			_graphicsDevice.SetVertexBuffer( /*vbo1 ?*/ _vertexBuffer /*: _vertexBuffer2*/);

			RasterizerState rasterizerState = new RasterizerState {CullMode = CullMode.None};
			_graphicsDevice.RasterizerState = rasterizerState;

			foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				/*_graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vertices.ToArray(), 0,
					Vertices.Count / 3);*/
				_graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Vertices.Count / 3);
			}
		}
	}
}