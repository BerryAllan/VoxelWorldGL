namespace VoxelWorldGL.block.material
{
	class MaterialAir : Material
	{
		public MaterialAir() : base()
		{
			Replaceable = true;
			Solid = false;
			Translucent = true;
			CanBurn = false;
		}
	}
}
