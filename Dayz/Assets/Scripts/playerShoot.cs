using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour {

    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform barrel;

    [SerializeField] private GameObject cam;
    private cameraRotation cameraRotation;

    [SerializeField] private float bulletForce;
    [SerializeField] private float recoil;

    [SerializeField] private float magazineSize;
    private float currentMagazine;

    [SerializeField] private Animator gunAim;
    private bool isAimingGun = false;

    void Awake() {
        currentMagazine = magazineSize;
        cameraRotation = cam.GetComponent<cameraRotation>();
    }

    void Update() {
        if  (isAimingGun) {
            cameraRotation.startGunSway();
        }

        if (Input.GetMouseButtonDown(1)) {
            if (isAimingGun) {
                isAimingGun = false;
                gunAim.SetBool("RightClick", false);
            } else {
                isAimingGun = true;
                gunAim.SetBool("RightClick", true);
            }
        }
        
        if (Input.GetMouseButtonDown(0)) {
            if (currentMagazine > 0) {
                GameObject spawn = Instantiate(BulletPrefab, barrel.transform.position, Quaternion.identity);
                Rigidbody spawnRb = spawn.GetComponent<Rigidbody>();
                spawnRb.AddForce(transform.forward * bulletForce);
                cameraRotation.gunRecoil(recoil);
                currentMagazine -= 1;
            } else {
                Debug.Log("Click, Click, Click");
            }
        }
        if (Input.GetKey("r")) {
            currentMagazine = magazineSize;
            Debug.Log(currentMagazine);
        }
    }
}
