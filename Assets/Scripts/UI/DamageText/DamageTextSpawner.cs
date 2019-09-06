using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        public void Spawn(float damage)
        {
            if(damageTextPrefab!=null)
            {
                DamageText instance = Instantiate(damageTextPrefab, transform);
                instance.SetText(damage.ToString());
            }
        }
    }
}
