using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
  [SerializeField]
  private float _rotation = 80f;
  Animator _anim;
  private SpawnManager _spawnManager;
  private AudioSource _asteroidSounds;


	private void Start()
	{
    transform.position = new Vector3(0,5,0);

    _anim = gameObject.GetComponent<Animator>();
    if (_anim == null)
    {
      Debug.LogError("Asteroid::THE ANIMATOR IS NULL");
    }

    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    if (_spawnManager == null)
    {
      Debug.LogError("Asteroid::SPAWN MANAGER IS NULL");
    }

    _asteroidSounds = gameObject.GetComponent<AudioSource>();
    if (_asteroidSounds == null)
    {
      Debug.LogError("Asteroid::AUDIO IS NULL");
    }
  }


  private void Update()
  {
    CalculateMovement();
  }


  private void CalculateMovement()
  {
    transform.Rotate(0f,0f,1f * _rotation * Time.deltaTime);
	}


  public void AsteroidDestroyed()
  {
    Destroy(GetComponent<Collider2D>());
    _anim.SetTrigger("OnAsteroidDestroyed");
    _asteroidSounds.Play();
    _spawnManager.startSpawn();
    Destroy(this.gameObject, 2.6f);
  }


  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Laser")
    {
      Destroy(other.gameObject);
      AsteroidDestroyed();
		}
	}


}
