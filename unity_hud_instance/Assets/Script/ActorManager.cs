
//=======================================================
// 作者：hannibal
// 描述：模拟actor的移动和hp改变
//=======================================================
using System.Collections.Generic;
using UnityEngine;

namespace YX
{
    public class ActorManager : MonoBehaviour
    {
        public int Count = 500;

        public List<Actor> AllActors { get; private set; } = new List<Actor>();

        public static ActorManager Instance;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            AllActors.Capacity = Count;
            for (int i = 0; i < Count; i++)
            {
                Actor actor = new Actor();
                actor.Matrix = Matrix4x4.TRS(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, Vector3.one);
                actor.NameIndex = i;
                actor.Progress = Random.Range(0.0f, 1.0f);
                AllActors.Add(actor);
            }
        }
        private void Update()
        {
            foreach (var actor in AllActors) 
            {
                actor.Update();
            }
        }
    }
    public class Actor
	{
		public Matrix4x4 Matrix;
		public int NameIndex = 0;
		public float Progress = 1.0f;

        public void Update()
        {
            Matrix = Matrix4x4.TRS(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, Vector3.one);
            Progress = Random.Range(0.0f, 1.0f);
        }
    }
}
