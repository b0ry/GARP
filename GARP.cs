using System;
using System.Collections.Generic;
using UnityEngine;

namespace GARP {
	namespace GA {
		public class Bush {
			public int triangles;
			public int squares;
			public int circles;
			public int nice;
			public int nasty;
			public Vector3 size;
			public Color shade;
			public List<string> types;
		}
		public class Attack {
			public int damage;
			public float range;
			public string x;
			public string z;
		}
		public class Block {
			public int block;
		}

		public class Run {
			public float speed;
			public float cooldown;
		}
		public class Strafe {
			public float strafe;
		}
	}
}
