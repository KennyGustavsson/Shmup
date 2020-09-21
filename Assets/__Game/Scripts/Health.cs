using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Health : MonoBehaviour{
    public enum Types{
        Player,
        Enemy,
        Object
    }
    public Types type; 
    
    public int healthID = 1;
    [SerializeField] private int _health;
    public int maxHealth = 100;
    public bool isShielded = false;
    public Color damageColor = Color.red;
    public float damageEffectTime = 0.2f;
    
    [Header("Player Type")]
    public float playerInvincibilityTime = 0.2f;
    public GameObject playerModel;
    public float flickerTime = 0.1f;
    public int dieParticleID = 1;
    
    [Header("Audio")]
    public AudioClip[] hitSound;
    public AudioClip[] dieSound;

    [Header("Screen Shake On Hit")] 
    public float screenShakeMagnitude;
    public float screenShakeLength;

    [NonSerialized] public bool isEnqued;
    
    private AudioSource _audioSource;
    private bool _isDead = false;
    private MeshRenderer _meshRenderer;
    private Color _originalColor;
    private bool _invincibility = false;
    
    private void Awake(){
        _health = maxHealth;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _originalColor = _meshRenderer.material.color;
        if(type == Types.Player) GameManager.Instance.playerHealth = _health;
    }

    private void OnEnable(){
        isEnqued = false;
        if(type == Types.Player) GameEvents.current.ONPickUpHealth += Heal;
        _meshRenderer.material.color = _originalColor;
        StopAllCoroutines();
    }

    private void OnDisable(){
        switch (type){
            case Types.Player:
                GameEvents.current.ONPickUpHealth -= Heal;
                break;
            case Types.Object:
                transform.position = new Vector3(0, 0, -40);
                _health = maxHealth;
                _isDead = false;
                break;
            case Types.Enemy:
                break;
        }
    }

    private void Heal(int id, int heal){
        if(id != healthID) return;

        _health += heal;
        if (_health > maxHealth){
            _health = maxHealth;
        }
        if(type == Types.Player) GameManager.Instance.UpdatePlayerHealth(_health);
    }
    
    public int Damage(int damage){
        if (isShielded || _invincibility) return 0;
        
        if (_health <= 0){
            if(!_isDead) Die();
            return damage;
        }

        _health -= damage;
        if(type == Types.Player) GameManager.Instance.UpdatePlayerHealth(_health);
        if (damage > 0){
            if(type != Types.Player) ScreenShake.instance.Shake(screenShakeLength, screenShakeMagnitude);
            if (_health <= 0){
                // Dead
                Die();
                return -_health;
            }
            // Hurt
            if(gameObject.activeSelf) StartCoroutine(DamageEffect());
            if (type == Types.Player){
                ScreenShake.instance.Shake(screenShakeLength, screenShakeMagnitude);
                StartCoroutine(PlayerInvincibility());
            }
        }
        // Heal
        else if(_health > maxHealth){
            _health = maxHealth;
            if(type == Types.Player) GameManager.Instance.UpdatePlayerHealth(_health);
        }
        
        return 0;
    }

    private IEnumerator DamageEffect(){
        var material = _meshRenderer.material;
        material.color = damageColor;
        
        if (hitSound.Length > 0){
            int i = Random.Range(0, hitSound.Length - 1);
            _audioSource.PlayOneShot(hitSound[i]);
        }
        
        yield return new WaitForSeconds(damageEffectTime);
        material.color = _originalColor;
    }
    
    private void Die(){
        _isDead = true;
        StopAllCoroutines();
        
        if (type == Types.Enemy){
            transform.position = new Vector3(0,0, -40);
            _health = maxHealth;
            _isDead = false;
            if(isEnqued == false) EnemySpawner.Instance.EnqueEnemy(gameObject);
            GameManager.Instance.UpdatePlayerScore(1);
            
            if (dieSound.Length > 0){
                int i = Random.Range(0, dieSound.Length - 1);
                _audioSource.PlayOneShot(dieSound[i]);
                StartCoroutine(Deactivate(dieSound[i].length));
                return;
            }
        }
        else if(type == Types.Player){
            if (dieSound.Length > 0){
                int i = Random.Range(0, dieSound.Length - 1);
                _audioSource.PlayOneShot(dieSound[i]);
                ScreenShake.instance.StopShake();
                var transform1 = transform;
                ObjectPool.Instance.ObjectPooler(dieParticleID, transform1.position, transform1.rotation);
                StartCoroutine(Deactivate(dieSound[i].length));
            }
            GameManager.Instance.UpdatePlayerHealth(0);
            return;
        }
        else{
            transform.position = new Vector3(0,0, -40);
            _health = maxHealth;
            _isDead = false;
            
            if (dieSound.Length > 0){
                int i = Random.Range(0, dieSound.Length - 1);
                _audioSource.PlayOneShot(dieSound[i]);
                StartCoroutine(Deactivate(dieSound[i].length));
                return;
            }
        }
        gameObject.SetActive(false);
    }

    private IEnumerator PlayerInvincibility(){
        _invincibility = true;

        float time = 0;
        while (time < playerInvincibilityTime){
            playerModel.SetActive(!playerModel.activeSelf);
            yield return new WaitForSeconds(flickerTime);
            time += flickerTime;
        }
        playerModel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _invincibility = false;
    }
    
    private IEnumerator Deactivate(float time){
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}