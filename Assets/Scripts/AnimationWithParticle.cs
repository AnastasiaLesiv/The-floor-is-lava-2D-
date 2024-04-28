using UnityEngine;


    public class AnimationWithParticle : MonoBehaviour
    {
        public ParticleSystem dustParticles;

        public void StartDustParticles()
        {
            dustParticles.gameObject.SetActive(true);
            dustParticles.Play();
        } 
        public void StopDustParticles()
        {
            dustParticles.Stop();
            dustParticles.gameObject.SetActive(false);
        }
    }
