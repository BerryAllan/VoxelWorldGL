using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using VoxelWorldGL.block.blocks;
using VoxelWorldGL.client.renderer;

namespace VoxelWorldGL.world.chunk
{
	public class ChunkRenderer
	{
		private readonly Chunk _chunk;
		public List<VertexPositionColor> Vertices { get; } = new List<VertexPositionColor>();

		public ChunkRenderer(Chunk chunk)
		{
			_chunk = chunk;
			Init();
		}

		private void Init()
		{
			foreach (Block b in _chunk.Blocks)
			{
				Vertices.AddRange(b.Renderer.Vertices);
			}
		}
	}
}
