using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoxelWorldGL.block.material;
using VoxelWorldGL.block.renderer;
using VoxelWorldGL.item;
using VoxelWorldGL.world;
using VoxelWorldGL.world.chunk;

namespace VoxelWorldGL.block.blocks
{
	public abstract class Block
	{
		public Material Material { get; set; }
		public Vector3 WorldPos { get; set; }
		public Vector3 ChunkPos { get; set; }
		public Chunk Chunk { get; }
		public BlockRenderer Renderer;
		public Faces RenderedFaces;
		public Color Color;

		public Block(Vector3 worldPos, Vector3 chunkPos, Chunk chunk, Material material)
		{
			//Material = material;
			WorldPos = worldPos;
			ChunkPos = chunkPos;
			Chunk = chunk;
			Material = material;
		}

		public void AssignBlockRenderer()
		{
			Renderer = new BlockRenderer(this);
			Renderer.DefineVertices();
		}

		public Block GetBlockFromItem(Item itemBlock)
		{
			return itemBlock is ItemBlock block ? block.GetBlock() : null;
		}

		public void Update()
		{
			if (Material != Material.Air)
			{
				if (ChunkPos.X + 1 < Settings.ChunkSize)
					if (Chunk.Blocks[(int) (ChunkPos.X + 1), (int) ChunkPos.Y, (int) ChunkPos.Z].Material !=
					    Material.Air)
						RenderedFaces.North = false;
					else
						RenderedFaces.North = true;
				if (ChunkPos.X - 1 > 0)
					if (Chunk.Blocks[(int) (ChunkPos.X - 1), (int) ChunkPos.Y, (int) ChunkPos.Z].Material !=
					    Material.Air)
						RenderedFaces.South = false;
					else
						RenderedFaces.South = true;
				if (ChunkPos.Z + 1 < Settings.ChunkSize)
					if (Chunk.Blocks[(int) (ChunkPos.X), (int) ChunkPos.Y, (int) ChunkPos.Z + 1].Material !=
					    Material.Air)
						RenderedFaces.East = false;
					else
						RenderedFaces.East = true;
				if (ChunkPos.Z - 1 > 0)
					if (Chunk.Blocks[(int) (ChunkPos.X), (int) ChunkPos.Y, (int) ChunkPos.Z - 1].Material !=
					    Material.Air)
						RenderedFaces.West = false;
					else
						RenderedFaces.West = true;
				if (ChunkPos.Y + 1 < Settings.WorldHeight)
					if (Chunk.Blocks[(int) (ChunkPos.X), (int) ChunkPos.Y + 1, (int) ChunkPos.Z].Material !=
					    Material.Air)
						RenderedFaces.Up = false;
					else
						RenderedFaces.Up = true;
				if (ChunkPos.Y - 1 > 0)
					if (Chunk.Blocks[(int) (ChunkPos.X), (int) ChunkPos.Y - 1, (int) ChunkPos.Z].Material !=
					    Material.Air)
						RenderedFaces.Down = false;
					else
						RenderedFaces.Down = true;
			}
			else if (Material == Material.Air)
			{
				RenderedFaces.North = false;
				RenderedFaces.South = false;
				RenderedFaces.East = false;
				RenderedFaces.West = false;
				RenderedFaces.Up = false;
				RenderedFaces.Down = false;
			}
		}
	}

	public enum DIRECTION
	{
		NORTH,
		SOUTH,
		EAST,
		WEST,
		UP,
		DOWN
	}

	public struct Faces
	{
		public Faces(bool facesByDefault)
		{
			North = facesByDefault;
			South = facesByDefault;
			East = facesByDefault;
			West = facesByDefault;
			Up = facesByDefault;
			Down = facesByDefault;
		}

		public bool North { get; set; }
		public bool South { get; set; }
		public bool East { get; set; }
		public bool West { get; set; }
		public bool Up { get; set; }
		public bool Down { get; set; }
	}
}