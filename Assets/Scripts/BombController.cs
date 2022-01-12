using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
	float deployTime;
	float explosionTime = 2.0f;
	float bombRange = 3.0f;

	float updownangle = 5.0f;

	Vector2 _vec_up;
	Vector2 _vec_down;
    // Start is called before the first frame update
    void Start()
    {
        deployTime = Time.timeSinceLevelLoad;
		_vec_up = new Vector2(Mathf.Sin(updownangle * 0.01745f), Mathf.Cos(updownangle * 0.01745f));
		_vec_down = new Vector2(-Mathf.Sin((180-updownangle) * 0.01745f), Mathf.Cos((180-updownangle) * 0.01745f));
    }

	void Explode(){
		/*
		Vector2 position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast(position, _vec_up, bombRange);
		Debug.Log(hit.collider);
		if (hit.collider && hit.collider.name.Contains("Dog")){
			hit.collider.gameObject.GetComponent<DogController>().OnBombExplosion();
			return;
		}
		hit = Physics2D.Raycast(position, _vec_down, bombRange);
		Debug.Log(hit.collider);
		if (hit.collider && hit.collider.name.Contains("Dog")){
			hit.collider.gameObject.GetComponent<DogController>().OnBombExplosion();
			return;
		}
		hit = Physics2D.Raycast(position, Vector2.left, bombRange);
		Debug.Log(hit.collider);
		if (hit.collider && hit.collider.name.Contains("Dog")){
			hit.collider.gameObject.GetComponent<DogController>().OnBombExplosion();
			return;
		}
		hit = Physics2D.Raycast(position, Vector2.right, bombRange);
		Debug.Log(hit.collider);
		if (hit.collider && hit.collider.name.Contains("Dog")){
			hit.collider.gameObject.GetComponent<DogController>().OnBombExplosion();
			return;
		}
		*/
		foreach(var dog in DogController.doglist){
			float dist = Vector2.Distance(this.gameObject.transform.position, dog.transform.position);
			if (dist < bombRange){
				dog.GetComponent<DogController>().OnBombExplosion();
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad >= deployTime + explosionTime){
			Explode();
			Destroy(this.gameObject);
		}
    }
}
