using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 12f;
    public float maxTravel = 8f;

    [Header("Fade")]

    // Start fading away this far
    public float fadeDistance = 2f;

    private Vector3 _startPos;
    private int _dir = 1;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;

    private Color _baseColor;

    public void Fire(int direction)
    {
        _dir = Mathf.Sign(direction) >= 0 ? 1 : -1;
        _startPos = transform.position;        
        ResetVisual();
        
    }
   
    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _baseColor = _sr != null ? _sr.color : Color.white;
        _startPos = transform.position;
    }

    void OnEnable()
    {
        ResetVisual();
        _startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (_rb != null)
            _rb.linearVelocity = new Vector2(_dir * speed, 0f);

        float d = Vector3.Distance(_startPos, transform.position);

        if (_sr != null && d > maxTravel - fadeDistance)
        {
            float t = Mathf.InverseLerp(maxTravel, maxTravel - fadeDistance, d);
            var c = _baseColor;    
            c.a = Mathf.Clamp01(1f - t);
            _sr.color = c;
        }

        if (d >= maxTravel) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeHit(1);  
        }

        Destroy(gameObject);
    }

    private void ResetVisual()
    {
        if (_sr != null) _sr.color = _baseColor;  
    }
}
