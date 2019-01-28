using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoxelWorldGL.block.blocks;

namespace VoxelWorldGL.block.renderer
{
	public class BlockRenderer
	{
		public readonly Block Block;
		public List<VertexPositionColor> Vertices { get; set; } = new List<VertexPositionColor>();

		public BlockRenderer(Block block)
		{
			Block = block;
			Block.Update();
		}

		public void DefineVertices()
		{
			if (Block.RenderedFaces.North)
				Vertices.AddRange(Block.Renderer.NorthFace(Block.WorldPos));
			if (Block.RenderedFaces.South)
				Vertices.AddRange(Block.Renderer.SouthFace(Block.WorldPos));
			if (Block.RenderedFaces.East)
				Vertices.AddRange(Block.Renderer.EastFace(Block.WorldPos));
			if (Block.RenderedFaces.West)
				Vertices.AddRange(Block.Renderer.WestFace(Block.WorldPos));
			if (Block.RenderedFaces.Up)
				Vertices.AddRange(Block.Renderer.UpFace(Block.WorldPos));
			if (Block.RenderedFaces.Down)
				Vertices.AddRange(Block.Renderer.DownFace(Block.WorldPos));

			Vertices = Vertices.OrderBy(vertex => Vector3.Distance(vertex.Position, Vector3.Zero)).ToList();
		}

		public VertexPositionColor[] NorthFace(Vector3 pos)
		{
			VertexPositionColor[] vertices = new VertexPositionColor[6];
			vertices[0] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[1] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[2] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			//vertices[3] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			//vertices[4] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[3] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);

			return  vertices;
		}

		public VertexPositionColor[] SouthFace(Vector3 pos)
		{
			VertexPositionColor[] vertices = new VertexPositionColor[6];
			vertices[0] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[1] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[2] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[3] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[4] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[5] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);

			return vertices;
		}

		public VertexPositionColor[] EastFace(Vector3 pos)
		{
			VertexPositionColor[] vertices = new VertexPositionColor[6];
			vertices[0] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[1] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[2] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[3] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[4] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[5] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);

			return vertices;
		}

		public VertexPositionColor[] WestFace(Vector3 pos)
		{
			VertexPositionColor[] vertices = new VertexPositionColor[6];
			vertices[0] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[1] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[2] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[3] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[4] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[5] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);

			return vertices;
		}

		public VertexPositionColor[] UpFace(Vector3 pos)
		{
			VertexPositionColor[] vertices = new VertexPositionColor[6];
			vertices[0] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[1] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[2] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[3] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[4] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[5] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y + 0.5f, pos.Z + 0.5f), Block.Color);

			return vertices;
		}

		public VertexPositionColor[] DownFace(Vector3 pos)
		{
			VertexPositionColor[] vertices = new VertexPositionColor[6];
			vertices[0] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[1] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[2] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[3] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z - 0.5f), Block.Color);
			vertices[4] = new VertexPositionColor(new Vector3(pos.X + 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);
			vertices[5] = new VertexPositionColor(new Vector3(pos.X - 0.5f, pos.Y - 0.5f, pos.Z + 0.5f), Block.Color);

			return vertices;
		}
	}
}