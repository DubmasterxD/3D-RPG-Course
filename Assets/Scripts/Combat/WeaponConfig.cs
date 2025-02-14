﻿using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float range = 2f;
        [SerializeField] float damage = 5f;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        public float Range { get => range; }
        public float PercentageBonus { get => percentageBonus; }
        public float Damage { get => damage; }

        Weapon equippedWeapon = null;

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Transform handTransform = GetTransform(rightHand, leftHand);
            Projectile projectileInstance = Instantiate(projectile, handTransform.position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator anim)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                equippedWeapon = Instantiate(equippedPrefab, handTransform);
            }
            var overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                anim.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                anim.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return equippedWeapon;
        }

        public void DespawEquippedWeapon()
        {
            Destroy(equippedWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }
    }
}

