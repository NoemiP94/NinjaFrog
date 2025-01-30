using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //istanza
    public static SoundManager instance;
    AudioSource source;
    [SerializeField]
    AudioClip[] sounds = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string Name)
    {
        foreach (var item in sounds)
        {
            if (item.name == Name) 
            {
                source.Stop();
                source.clip = item;
                source.Play();
            }
        }
    }
}
