using BepInEx;
using HarmonyLib;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatPet
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string pluginGUID = "com.metalted.zeepkist.catpet";
        public const string pluginName = "CatPet";
        public const string pluginVersion = "1.0";
        private void Awake()
        {
            Harmony harmony = new Harmony(pluginGUID);
            harmony.PatchAll();
            // Plugin startup logic
            Logger.LogInfo($"Plugin CatPet is loaded!");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Scene scene = SceneManager.GetActiveScene();
                if(scene.name == "3D_MainMenu")
                {
                    Debug.Log("Clicked");

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit))
                    {
                        AnimalController animal = hit.collider.transform.GetComponentInParent<AnimalController>();
                        if(animal != null)
                        {
                            Debug.Log("Hit cat");

                            //Get the heart
                            GameObject heart = GameObject.Find("Heart");
                            if(heart != null)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    GameObject heartContainer = new GameObject("HContainer");
                                    heartContainer.transform.position = hit.collider.transform.TransformPoint(Vector3.up * 2 + Random.onUnitSphere * 0.5f);

                                    GameObject heartCopy = GameObject.Instantiate(heart, heartContainer.transform);
                                    heartCopy.transform.localEulerAngles = new Vector3(270, 0, 0);

                                    heartContainer.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                                    heartContainer.AddComponent<FloatingHeart>();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class FloatingHeart : MonoBehaviour
    {
        public void Update()
        {
            transform.Translate(Vector3.up * Time.deltaTime);
            transform.localScale = transform.localScale * 0.975f;
            transform.Rotate(Vector3.up * 120f * Time.deltaTime);

            if(transform.localScale.x < 0.1f)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}

