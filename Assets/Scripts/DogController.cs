using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    // Start is called before the first frame update
	public static List<GameObject> doglist = new List<GameObject>(); //might become enemy list (GameController.cs) in further code evolve. For sake of simplicity i leave it as it is.
	SpriteRenderer _renderer;
	bool isDirty = false;
	Vector2 dir;
	[SerializeField] Sprite _tex_up;
	[SerializeField] Sprite _tex_down;
	[SerializeField] Sprite _tex_left;
	[SerializeField] Sprite _tex_right;

	[SerializeField] Sprite _tex_dirty_up;
	[SerializeField] Sprite _tex_dirty_down;
	[SerializeField] Sprite _tex_dirty_left;
	[SerializeField] Sprite _tex_dirty_right;

	[SerializeField] float _movespeed;


	float updownangle = 5.0f;

	Vector2 _vec_up;
	Vector2 _vec_down;
    void Start()
    {
        _renderer = this.gameObject.GetComponent<SpriteRenderer>();
		dir = Vector2.left;
		_vec_up = new Vector2(Mathf.Sin(updownangle * 0.01745f), Mathf.Cos(updownangle * 0.01745f));
		_vec_down = new Vector2(-Mathf.Sin((180-updownangle) * 0.01745f), Mathf.Cos((180-updownangle) * 0.01745f));
		doglist.Add(this.gameObject);
    }

    // Update is called once per frame

	bool IsMoveAvailable(out bool[] result){ // 0 - up , 1-down, 2 - left, 4-right
		result = new bool[4];
		bool available = false;
		Vector2 position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast(position, _vec_up, 2.0f);
		if (!hit.collider || hit.collider.name != "stone"){
			result[0] = true;
			available = true;
		}
		hit = Physics2D.Raycast(position, _vec_down, 2.0f);
		if (!hit.collider || hit.collider.name != "stone"){
			result[1] = true;
			available = true;
		}
		hit = Physics2D.Raycast(position, Vector2.left, 2.0f);
		if (!hit.collider || hit.collider.name != "stone"){
			result[2] = true;
			available = true;
		}
		hit = Physics2D.Raycast(position, Vector2.right, 2.0f);
		if (!hit.collider || hit.collider.name != "stone"){
			result[3] = true;
			available = true;
		}
		return available;
	}
	void OnMove(){
		bool[] moves;
		Vector2 dir = this.dir;
		if (IsMoveAvailable(out moves)){
			int rand = Random.Range(0, 3);
			if (moves[rand] == true){
				switch(rand){
					case 0: //up
						_renderer.sprite = isDirty ? _tex_dirty_up : _tex_up;
						dir = _vec_up;
						break;
					case 1: //
						_renderer.sprite = isDirty ? _tex_dirty_down : _tex_down;
						dir = _vec_down;
						break;
					case 2: //left
						_renderer.sprite = isDirty ? _tex_dirty_left : _tex_left;
						dir = Vector2.left;
						break;
					case 3: //right
						_renderer.sprite = isDirty ? _tex_dirty_right : _tex_right;
						dir = Vector2.right;
						break;
				}
			}
		}
		Vector3 delta = dir * _movespeed * Time.deltaTime;
		this.gameObject.transform.position = this.gameObject.transform.position + delta;
	}
	GameObject _latestwaypoint; //we can change direction only once using one waypoint
	void OnMoveWaypoint(){
		Vector3 delta;
		Vector2 position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast(position, this.dir, 0.1f);
		Debug.Log(hit.collider);
		if (!hit.collider || ( hit.collider && 
							 !( hit.collider.name.Contains("Waypoint") ||
							   hit.collider.name == "Wall" ||
							   hit.collider.gameObject.Equals(_latestwaypoint)))){
			delta = this.dir * _movespeed * Time.deltaTime;
			this.gameObject.transform.position = this.gameObject.transform.position + delta;
			return;
		}
		_latestwaypoint = hit.collider.gameObject;
		bool[] moves;
		Vector2 dir = this.dir;
		if (IsMoveAvailable(out moves)){
			int rand = Random.Range(0, 3);
			if (moves[rand] == true){
				switch(rand){
					case 0: //up
						_renderer.sprite = isDirty ? _tex_dirty_up : _tex_up;
						this.dir = _vec_up;
						break;
					case 1: // down
						_renderer.sprite = isDirty ? _tex_dirty_down : _tex_down;
						this.dir = _vec_down;
						break;
					case 2: //left
						_renderer.sprite = isDirty ? _tex_dirty_left : _tex_left;
						this.dir = Vector2.left;
						break;
					case 3: //right
						_renderer.sprite = isDirty ? _tex_dirty_right : _tex_right;
						this.dir = Vector2.right;
						break;
				}
			}
		}
		delta = dir * _movespeed * Time.deltaTime;
		this.gameObject.transform.position = this.gameObject.transform.position + delta;
	}
    void Update()
    {
        OnMoveWaypoint();

    }

	public void OnBombExplosion(){
		if (isDirty) return ;
		if (_renderer.sprite.Equals(_tex_up)){
			_renderer.sprite = _tex_dirty_up;
		}else if (_renderer.sprite.Equals(_tex_down)){
			_renderer.sprite = _tex_dirty_down;
		}else if (_renderer.sprite.Equals(_tex_left)){
			_renderer.sprite = _tex_dirty_left;
		}else if (_renderer.sprite.Equals(_tex_right)){
			_renderer.sprite = _tex_dirty_right;
		}
		isDirty = true;
	}
}
