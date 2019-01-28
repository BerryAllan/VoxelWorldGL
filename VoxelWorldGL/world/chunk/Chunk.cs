using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using VoxelWorldGL.block.blocks;

namespace VoxelWorldGL.world.chunk
{
	public class Chunk : IDisposable
	{
		public Block[,,] Blocks { get; } = new Block[Settings.ChunkSize, Settings.WorldHeight, Settings.ChunkSize];

		public Vector3[,,] BlockPositions { get; } =
			new Vector3[Settings.ChunkSize, Settings.WorldHeight, Settings.ChunkSize];

		public World World;
		public Vector3 Position;
		public ChunkRenderer Renderer;
		public Vector3[,] TopBlockTerrain = new Vector3[Settings.ChunkSize, Settings.ChunkSize];
		private readonly NoiseGeneratorSimplex _noiseGenerator;

		public Chunk(World world, Vector3 position, NoiseGeneratorSimplex noiseGenerator)
		{
			World = world;
			Position = position;
			_noiseGenerator = noiseGenerator;
			FillWithBlocks();
		}

		public void AssignChunkRenderer()
		{
			Renderer = new ChunkRenderer(this);
		}

		private void FillWithBlocks()
		{
			for (float x = 0, i = 0; i < Settings.ChunkSize; x += Settings.BlockSize, i++)
			{
				for (float z = 0, k = 0; k < Settings.ChunkSize; z += Settings.BlockSize, k++)
				{
					float posX = Position.X + x;
					float posZ = Position.Z + z;
					Vector3 topBlockPos = new Vector3(posX,
						(int) (_noiseGenerator.Get2DNoise((int) posX, (int) posZ) * 0.1 * Settings.WorldHeight / 2 +
						       Settings.GroundDisplacement), posZ);
//					Debug.WriteLine((noiseGen.Get2DNoise((int)posX, (int)posZ) * Settings.WorldHeight));
					TopBlockTerrain[(int) i, (int) k] = topBlockPos;
				}
			}

			/*for (int i = 0; i < TopBlockTerrain.GetLength(0); i++)
			{
				Debug.Write("x: ");
				for (int j = 0; j < TopBlockTerrain.GetLength(1); j++)
				{
					Debug.Write(TopBlockTerrain[i, j] + " ");
				}
				Debug.Write("\n");
			}*/
			BlockAir blockAir = new BlockAir(new Vector3(0,0,0), new Vector3(0, 0, 0), this); //air doesn't need to be instantiated; it's invisible
			//TODO: Replace this with biome specifics; use 2d noise for biome gen and height multipliers
			for (float x = 0, i = 0; i < Settings.ChunkSize; x += Settings.BlockSize, i++)
			{
				for (float y = 0, j = 0; j < Settings.WorldHeight; y += Settings.BlockSize, j++)
				{
					for (float z = 0, k = 0; k < Settings.ChunkSize; z += Settings.BlockSize, k++)
					{
						float posX = Position.X + x;
						float posY = Position.Y + y;
						float posZ = Position.Z + z;
						Vector3 blockWorldPos = new Vector3(posX, posY, posZ);
						Vector3 blockChunkPos = new Vector3(x, y, z);
						Block block;
						if ((int) y == (int) TopBlockTerrain[(int) i, (int) k].Y)
						{
							block = new BlockGrass(blockWorldPos, blockChunkPos, this);
						}
						else if (y < TopBlockTerrain[(int) i, (int) k].Y &&
						         y >= TopBlockTerrain[(int) i, (int) k].Y - 5)
						{
							block = new BlockDirt(blockWorldPos, blockChunkPos, this);
						}
						else if (y < TopBlockTerrain[(int) i, (int) k].Y - 5)
						{
							block = new BlockStone(blockWorldPos, blockChunkPos, this);
						}
						else
						{
							if (y < Settings.SeaLevel)
								block = new BlockWater(blockWorldPos, blockChunkPos, this);
							else
								block = blockAir;
						}

						Blocks[(int) i, (int) j, (int) k] = block;
						BlockPositions[(int) i, (int) j, (int) k] = blockWorldPos;
					}
				}
			}

			foreach (Block b in Blocks)
			{
				b.AssignBlockRenderer();
			}

		}

		public void Dispose()
		{
			Renderer.Vertices.Clear();
		}
	}
}