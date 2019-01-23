using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldGL.block.material
{
	class MaterialSolid : Material
	{
		public MaterialSolid() : base()
		{
			Replaceable = false;
			Solid = true;
			Translucent = false;
		}
	}
}
