using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform healthBar = null;
        [SerializeField] Canvas canvas = null;

        void Update()
        {
            float healthFraction = Mathf.Clamp01(health.GetPercentage() / 100);
            if (healthFraction == 1 || healthFraction == 0)
            {
                canvas.enabled = false;
            }
            else
            {
                canvas.enabled = true;
                Debug.Log(gameObject.transform.localScale);
                healthBar.localScale = new Vector3(healthFraction, 1, 1);
            }
        }
    }
}
