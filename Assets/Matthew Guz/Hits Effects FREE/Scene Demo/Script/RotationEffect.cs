using UnityEngine;

namespace MatthewAssets
{


    public class ParticleRotationController : MonoBehaviour
    {
        public ParticleSystem particleSystem; 
        public float minZRotation = 0f; 
        public float maxZRotation = 360f; 

        void Start()
        {
            SetRandomRotation();
            particleSystem.Play(); 
        }

        void SetRandomRotation()
        {
            float randomZRotation = Random.Range(minZRotation, maxZRotation);
            particleSystem.transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);
        }
    }
}