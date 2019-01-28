using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VoxelWorldGL.block;
using VoxelWorldGL.block.blocks;
using VoxelWorldGL.client.renderer;
using VoxelWorldGL.world.chunk;

namespace VoxelWorldGL.world
{
	public class World
	{
		public volatile ObservableCollection<Chunk> Chunks;
		public WorldRenderer Renderer;
		public readonly Game Game;
		private readonly NoiseGeneratorSimplex _noiseGenerator;
		private volatile bool _shouldChangeVerts;
		private static readonly object ChunkGenLock = new object();

		public World(LotrVox game)
		{
			_noiseGenerator = new NoiseGeneratorSimplex(new Random().Next(12345), 0.01f);
			Game = game;
			Chunks = new ObservableCollection<Chunk>();
		}

		public void Initialize()
		{
			for (int i = -Settings.RenderDistance * Settings.ChunkSize;
				i < Settings.RenderDistance * Settings.ChunkSize;
				i += Settings.ChunkSize)
			{
				for (int j = -Settings.RenderDistance * Settings.ChunkSize;
					j < Settings.RenderDistance * Settings.ChunkSize;
					j += Settings.ChunkSize)
				{
					Chunk chunk = new Chunk(this, new Vector3(i, 0, j), _noiseGenerator);
					chunk.AssignChunkRenderer();
					Chunks.Add(chunk);
				}
			}

			Renderer = new WorldRenderer(this, Game.GraphicsDevice);
			//TODO: stop lag from chunk adding and deleting
			Chunks.CollectionChanged += (sender, args) =>
			{
				//I want it to crash if there ain't any chunks
				/*if (Chunks.Count < 1)
				    return;*/
				Task.Run(() =>
				{
					lock (ChunkGenLock)
					{
						var newVerts = new List<VertexPositionColor>();
						// ReSharper disable once ForCanBeConvertedToForeach; has to be for-loop
						for (int i = 0; i < Chunks.Count; i++)
							newVerts.AddRange(Chunks[i].Renderer.Vertices);
						Renderer.Vertices = newVerts;

						//Renderer.SetVertexData();
						_shouldChangeVerts = true; //vertex changing needs to be done in the update method
					}
				});
			};
		}

		public void Update(GameTime gameTime)
		{
			Renderer.Camera.Update(gameTime);
			if (_shouldChangeVerts)
			{
				Renderer.SetVertexData();
				_shouldChangeVerts = false;
			}

			//deletes chunks too far away from the player
			Task.Run(() =>
			{
				lock (ChunkGenLock)
				{
					//TODO: make faster
					for (int i = 0; i < Chunks.Count; i++)
					{
						if (Vector2.Distance(
							    new Vector2(Renderer.Camera.CameraPosition.X, Renderer.Camera.CameraPosition.Z),
							    new Vector2(Chunks[i].Position.X, Chunks[i].Position.Z)) / Settings.ChunkSize >
						    Settings.RenderDistance / 2)
						{
							Chunks.Remove(Chunks[i]);
						}
					}
					
					//add chunks to empty spots around the player
					for (int i = -Settings.RenderDistance * Settings.ChunkSize;
						i < Settings.RenderDistance * Settings.ChunkSize;
						i += Settings.ChunkSize)
					{
						for (int j = -Settings.RenderDistance * Settings.ChunkSize;
							j < Settings.RenderDistance * Settings.ChunkSize;
							j += Settings.ChunkSize)
						{
							int newX = (int) (i + ChunkPos(Renderer.Camera.CameraPosition).X);
							int newZ = (int) (j + ChunkPos(Renderer.Camera.CameraPosition).Z);

							bool hasChunk = false;
							foreach (Chunk chunk in Chunks)
							{
								if ((int) chunk.Position.X == newX && (int) chunk.Position.Z == newZ)
								{
									hasChunk = true;
									break;
								}
							}

							if (!hasChunk)
							{
								Chunk chunk = new Chunk(this, new Vector3(newX, 0, newZ), _noiseGenerator);
								chunk.AssignChunkRenderer();
								Chunks.Add(chunk);
							}
						}
					}
				}
			});
		}

		public Vector3 ChunkPos(Vector3 playerPos)
		{
			return new Vector3((float) Math.Floor(playerPos.X / (double) Settings.ChunkSize) * Settings.ChunkSize, 0,
				(float) Math.Floor(playerPos.Z / (double) Settings.ChunkSize) * Settings.ChunkSize);
		}

		bool CheckRadius(int x, int y)
		{
			int vec2 = (int) Vector2.Subtract(new Vector2(x, y),
				new Vector2(Renderer.Camera.CameraPosition.X, Renderer.Camera.CameraPosition.Z)).Length();
			return vec2 < Settings.ChunkSize * Settings.RenderDistance;
		}

		public Block GetBlockAtPos(Vector3 pos)
		{
			foreach (Chunk chunk in Chunks)
			{
				if (Math.Abs(pos.X - chunk.Position.X) < Settings.ChunkSize &&
				    Math.Abs(pos.Z - chunk.Position.Z) < Settings.ChunkSize)
				{
					foreach (Block block in chunk.Blocks)
					{
						if (block != null && block.WorldPos.Equals(pos))
							return block;
					}
				}
			}

			return null;
		}

		public bool BlockAtPos(Vector3 position)
		{
			foreach (Chunk chunk in Chunks)
				if (Math.Abs(position.X - chunk.Position.X) < Settings.ChunkSize &&
				    Math.Abs(position.Z - chunk.Position.Z) < Settings.ChunkSize)
					foreach (Vector3 vector3 in chunk.BlockPositions)
						if (vector3.Equals(position))
							return true;

			return false;
		}
	}
}