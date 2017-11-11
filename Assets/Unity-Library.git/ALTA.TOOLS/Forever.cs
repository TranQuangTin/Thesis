using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Alta.Tools{
public class Forever : MonoBehaviour {
	void Awake(){
		DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}
