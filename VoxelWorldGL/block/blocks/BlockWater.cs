﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VoxelWorldGL.block.material;
using VoxelWorldGL.world;
using VoxelWorldGL.world.chunk;

namespace VoxelWorldGL.block.blocks
{
	class BlockWater : Block
	{
		public BlockWater(Vector3 worldPos, Vector3 chunkPos, Chunk chunk) : base(worldPos, chunkPos, chunk, Material.Liquid)
		{
			RenderedFaces = new Faces(false);
			Color = Color.Blue;
		}
	}
}
