using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VoxelWorldGL.item;

namespace VoxelWorldGL.block.material
{
	public class Material
	{
		public bool Replaceable { get; set; } = false;
		public bool Solid { get; set; } = true;
		public int MoveSpeed { get; set; } = 100;
		public bool Translucent { get; set; } = false;
		public ItemTool Tool { get; set; } = null;
		public bool CanBurn { get; set; } = true;
		public int BurnTime { get; set; } = 5; //default value for wood to burn
		public Color Color { get; }

		private bool _burning = false;
		public bool Burning
		{
			get => _burning;
			set
			{
				if (CanBurn) _burning = true;
				else _burning = false;
			}
		}

		public static Material Air = new MaterialAir();
		public static Material Dirt = new MaterialSolid()
		{
			CanBurn = true,
			Solid = true,
			Replaceable = false,
			BurnTime = 10
		};
		public static Material Stone = new MaterialSolid()
		{
			CanBurn = true,
			Solid = true,
			Replaceable = false,
			BurnTime = 50,
			//Tool = ItemPick
		};
		public static Material Liquid = new MaterialLiquid()
		{
			CanBurn = false,
			Solid = false,
			MoveSpeed = 50,
			Replaceable = true,
			//Tool = ItemBucket
		};

		public Material()
		{

		}
	}
}
