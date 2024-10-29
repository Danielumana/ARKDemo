using UnityEngine;

public class DeathVolume : MonoBehaviour
{

    AudioSource audioSourceRef;
    public AudioClip deathAudio;

    void Start()
    {
        audioSourceRef = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision otherCollision) 
    {
        GameObject otherGameObject = otherCollision.gameObject;
        if (otherGameObject == null)
        {
            return;
        }
        if (otherGameObject.tag != "Ball")
        {
            return;
        }
        if (audioSourceRef != null)
        {
            audioSourceRef.clip = deathAudio;
            audioSourceRef.Play();
        }
        

        ARKGameMode gameModeReference = ARKGameMode.Instance;
        if (gameModeReference == null)
        {
            return;
        }
        gameModeReference.DespawnBall(otherGameObject);
    }

}
