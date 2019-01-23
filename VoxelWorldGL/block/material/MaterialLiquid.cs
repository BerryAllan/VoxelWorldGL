namespace VoxelWorldGL.block.material
{
	class MaterialLiquid : Material
	{
		public MaterialLiquid() : base()
		{
			Replaceable = true;
			Solid = false;
			Translucent = true;
			CanBurn = false;
		}
	}
}
