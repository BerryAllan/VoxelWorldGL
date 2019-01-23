using System;
using System.Linq.Expressions;

namespace VoxelWorldGL.world.biome
{
	abstract class Biome
	{
		protected string Name;
		protected int Temperature;
		protected bool Snow;
		protected bool Rain;
		
//		protected static NoiseGeneratorSimplex TempNoise = new NoiseGeneratorSimplex(new Random().Next(1234));
//		protected static NoiseGeneratorSimplex GrassColorNoise = new NoiseGeneratorSimplex(new Random().Next(2345));

		protected Biome(string name, int temp, bool snow, bool rain)
		{
			Name = name;
			Temperature = temp;
			Snow = snow;
			Rain = rain;
		}
	}
}
