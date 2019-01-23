using System;
using LibNoise;
using LibNoise.Primitive;

namespace VoxelWorldGL.world
{
	public class NoiseGeneratorSimplex
	{
		private static SimplexPerlin _perlin;
		private float _scale;

		public NoiseGeneratorSimplex(int seed, float scale)
		{
			_scale = scale;
			_perlin = new SimplexPerlin(seed, NoiseQuality.Best);
		}

		public float Get3DNoise(int x, int y, int z)
		{
			return _perlin.GetValue(x, y, z);
		}

		public float Get2DNoise(float x, float y)
		{
			return Math.Abs(_perlin.GetValue(x * _scale, y * _scale));
		}

		public float Get1DNoise(int x)
		{
			return _perlin.GetValue(x);
		}
	}
}
