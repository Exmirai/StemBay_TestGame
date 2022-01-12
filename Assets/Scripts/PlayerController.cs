using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
	GameController _gamecontroller;
	FixedJoystick _joystick;
	SpriteRenderer _renderer;
	[SerializeField] Sprite _tex_up;
	[SerializeField] Sprite _tex_down;
	[SerializeField] Sprite _tex_left;
	[SerializeField] Sprite _tex_right;
	[SerializeField] float _spawnPosX;
	[SerializeField] float _spawnPosY;
	[SerializeField] float _movespeed;
	[SerializeField] int   _inititalBombsCount = 10;
	[SerializeField] GameObject _bomb_model;
	TMP_Text	bombs_count_text;
	int 		bombs_count;

	bool alive = true;
	Vector2 dir;
	float updownangle = 5.0f;

	Vector2 _vec_up;
	Vector2 _vec_down;

    // Start is called before the first frame update
    void Start()
    {
        _gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
		_joystick = GameObject.Find("UI Canvas/Fixed Joystick").GetComponent<FixedJoystick>();
		_renderer = this.gameObject.GetComponent<SpriteRenderer>();
		bombs_count_text = GameObject.Find("UI Canvas/Button_PlaceBomb/Bombs_Count").GetComponent<TMP_Text>();
		bombs_count = _inititalBombsCount;
		dir = Vector2.right;
		_vec_up = new Vector2(Mathf.Sin(updownangle * 0.01745f), Mathf.Cos(updownangle * 0.01745f));
		_vec_down = new Vector2(-Mathf.Sin((180-updownangle) * 0.01745f), Mathf.Cos((180-updownangle) * 0.01745f));
    }

	Vector2 GetInput(){
		return _joystick.Direction;
	}

	void MoveTo(Vector2 dir){
		RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, dir, 0.8f);
		Collider2D coll  = hit.collider;
		if (coll == null || ( coll != null && !coll.name.Contains("stone") && coll.name != "Wall")){
			Vector3 delta = dir * _movespeed * Time.deltaTime;
			this.gameObject.transform.position = this.gameObject.transform.position + delta;
			return;
		}
	}

	Vector2 LookTo(Vector2 dir){
		float angles = Vector2.Angle(dir, Vector2.up);
		if (angles < 45.0f){ //up
			_renderer.sprite = _tex_up;
			this.dir = _vec_up;
			return _vec_up;
		}else if (180 - angles < 45.0f){ // down
			_renderer.sprite = _tex_down;
			this.dir = _vec_down;
			return _vec_down;
		}
		angles = Vector2.Angle(dir, Vector2.right);
		if (angles < 45.0f){ //right
			_renderer.sprite = _tex_right;
			this.dir = dir;
			return new Vector2(1, 0);
		}else if (180 - angles < 45.0f){ // left
			this.dir = dir;
			_renderer.sprite = _tex_left;
			return new Vector2(-1, 0);
		}
		return new Vector2(0, 0);
	}
	void CheckColliders(){
		//Raycast up down left right and find bonus
	}


	void CorrectDrawOrder(){ // function should be in GameController.cs or StoneController.cs but for performance reasons i put it there
        //if (Time.frameCount % 2 == 0){
			Vector2 position = new Vector2(this.gameObject.transform.position.x + (1.3f  *dir.x), this.gameObject.transform.position.y);
			RaycastHit2D hit = Physics2D.Raycast(position, Vector2.up, 1.5f);
			if (hit.collider && hit.collider.name == "stone"){ // player up to us
				hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "ent_down";
				return;
			}
			hit = Physics2D.Raycast(position, Vector2.down, 1.5f);
			if (hit.collider && hit.collider.name == "stone"){ // player down
				hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "ent_up";
				return;
			}
		//}
	}

    // Update is called once per frame
    void Update()
    {
		if (!alive) return;
		bombs_count_text.text = bombs_count.ToString();
		Vector2 dir = GetInput();
        if (dir.magnitude > 0.0f){
			MoveTo(LookTo(dir));
		}
		CheckColliders();
		CorrectDrawOrder();
    }

	public void ResetPlayerState(){
		this.gameObject.transform.position = new Vector3(_spawnPosX, _spawnPosY);
		alive = true;
	}
	public void PlaceBomb(){
		if (bombs_count == 0) return;
		Vector2 position = new Vector2(this.gameObject.transform.position.x + (1.3f  *dir.x), this.gameObject.transform.position.y);
		Instantiate(_bomb_model, position, Quaternion.identity);
		bombs_count--;
	}
}
