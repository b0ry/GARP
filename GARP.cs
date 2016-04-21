using System;
using System.Collections.Generic;
using UnityEngine;

namespace GARP {
	namespace Useful {
		public class Singleton <T> where T : class, new() {
			Singleton () {}
			class SingletonCreator {
				static SingletonCreator () {}
				internal static readonly T instance = new T ();
			}
			public static T Unique {
				get { return SingletonCreator.instance;}
			}
		}
	}
	namespace GA {
		public class Levels {
			public int attackLevel;
			public int blockLevel;
			public int runLevel;
			public int strafeLevel;
		}
		public class Chromosones {
			public int triangles;
			public int squares;
			public int circles;
			public int nice;
			public int nasty;
			public Vector3 size;
			public Color shade = new Color (0f,0f,0f,0f);
			public List<string> types;

			public void Clear() {
				triangles = 0;
				squares = 0;
				circles = 0;
				nasty = 0;
				nice = 0;
			}
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
