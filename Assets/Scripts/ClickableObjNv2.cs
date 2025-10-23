using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickableObjNv2 : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    private GameObject cam;
    private Animator camAnim;

    private GameObject transition;
    private Animator transitionAnim;
    [Header("Panel de dialogo")]
    [SerializeField] private GameObject desitionPanel;
    [SerializeField] private Button op1Btn;
    [SerializeField] private Button op2Btn;
    [SerializeField] private Button aceptarBtn;
    [SerializeField] private Button opExtraBtn;

    private TextMeshProUGUI desitionText;
    private TMP_Text o1Txt;
    private TMP_Text o2Txt;

    // [Header("Puerta del Cobertizo")]
    //private bool doorisOpen = false;
    //[SerializeField] private Animator doorAnim;

    [Header("Puerta Oculta")]
    [SerializeField] private Button puertaOcultaBtn;
    [Header("Casa del perro")]
    [SerializeField] private Button casitaBtn;
    private bool isfirstTime = true;
    private bool wasUsed = false;


    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        camAnim = cam.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        desitionPanel = FindInactiveWithName("PanelDialogo");
        desitionText = desitionPanel.GetComponentInChildren<TextMeshProUGUI>();
        op1Btn = FindInactiveWithName("op1").GetComponent<Button>();
        op2Btn = FindInactiveWithName("op2").GetComponent<Button>();
        o1Txt = op1Btn.GetComponentInChildren<TMP_Text>();
        o2Txt = op2Btn.GetComponentInChildren<TMP_Text>();
        aceptarBtn = FindInactiveWithName("AceptarBTN").GetComponent<Button>();
        opExtraBtn = FindInactiveWithName("opExtra").GetComponent<Button>();
        transition = GameObject.Find("Transcicsion");
        transitionAnim = transition.GetComponent<Animator>();
        puertaOcultaBtn.interactable = false;
        aceptarBtn.interactable = false;
        opExtraBtn.gameObject.SetActive(false);

    }
    public void ClickBasura()
    {
        desitionPanel.SetActive(true);
        desitionText.text = "Parece que hay algo en la basura";
        o1Txt.text = "Acercarse";
        o2Txt.text = "Que asco ¿Por qué haria eso?";
        op1Btn.gameObject.SetActive(true);
        op2Btn.gameObject.SetActive(true);
        aceptarBtn.interactable = false;

        //Eliminar los listeners previos para evitar duplicados
        op2Btn.onClick?.RemoveAllListeners();
        op1Btn.onClick?.RemoveAllListeners();

        op1Btn.onClick?.AddListener(() =>
            {
                desitionText.text = "El bote de basura está rodeado por un charco de agua, al acercarte, tu llama se debilita.";
                playerController.TakeDamage(1);
                op1Btn.gameObject.SetActive(false);
                op2Btn.gameObject.SetActive(false);
                aceptarBtn.interactable = true;
            });

        op2Btn.onClick?.AddListener(() =>
            {
                AudioManager.Instance.PlayEfecto("ButonSound");
                int randomValue = Random.Range(0, 3);
                switch (randomValue)
                {
                    case 0:
                        desitionText.text = "Retrocedes con prudencia. Ese bote de basura no es un simple cúmulo de desperdicios: es la sede secreta de la Sociedad Internacional de Gérmenes con Ambiciones de Dominio Mundial. Dentro, diminutos líderes microbianos llevan siglos planeando la invasión del mundo exterior, utilizando cáscaras de huevo como escudos y sobres de kétchup como manuales de guerra. Algunos mohos llevan bigotes finísimos y gafas diminutas, otros han desarrollado la capacidad de filosofar sobre el sentido de la existencia… de la basura. Acercarte sería interrumpir su cumbre anual, y nadie quiere provocar una guerra diplomática con una bacteria que ha memorizado el arte del sabotaje químico.";
                        break;
                    case 1:
                        desitionText.text = "El olor es insoportable, mejor te alejas.";
                        break;
                    case 2:
                        desitionText.text = "No quieres arriesgarte a enfermarte, así que te alejas.";
                        break;
                }
                op1Btn.gameObject.SetActive(false);
                op2Btn.gameObject.SetActive(false);
                aceptarBtn.interactable = true;
            });
    }
    public void ClickPuertaOculta()
    {
        if (GameManager.Instance.inventory.ContainsKey("LlaveSecreta"))
        {
            puertaOcultaBtn.interactable = true;
            desitionPanel.SetActive(true);
            desitionText.text = "La puerta está cerrada con llave ¿Quieres forzar la cerradura?";
            o1Txt.text = "Si, forzar la cerradura";
            o2Txt.text = "No, no se siente correcto";
            aceptarBtn.interactable = false;

            op1Btn.gameObject.SetActive(true);
            op2Btn.gameObject.SetActive(true);
            //Eliminar los listeners previos para evitar duplicados
            op2Btn.onClick?.RemoveAllListeners();
            op1Btn.onClick?.RemoveAllListeners();
            op1Btn.onClick?.AddListener(() =>
                {
                    AudioManager.Instance.PlayEfecto("ForzarPuertaLv2");
                    desitionText.text = "Pese a tus dudas, decides forzar la cerradura. Con un poco de esfuerzo, logras abrir la puerta. y decides entrar.";
                    op1Btn.gameObject.SetActive(false);
                    op2Btn.gameObject.SetActive(false);
                    StartCoroutine(WaitAndOpenDoor());
                    //Animacion de entrar al sotano
                });
            op2Btn.onClick?.AddListener(() =>
                {
                    AudioManager.Instance.PlayEfecto("ButonSound");
                    desitionText.text = "Decides hacerle caso a tu conciencia. Tal vez haya otra forma de entrar.";
                    op1Btn.gameObject.SetActive(false);
                    op2Btn.gameObject.SetActive(false);
                    aceptarBtn.interactable = true;
                });
        }
    }
    public void ClickCasita()
    {
        AudioManager.Instance.PlayEfecto("PerrosAudio");
        op2Btn.onClick?.RemoveAllListeners();
        op1Btn.onClick?.RemoveAllListeners();
        opExtraBtn.onClick?.RemoveAllListeners();
        aceptarBtn.interactable = false;
        if (!isfirstTime && !wasUsed)
        {
            opExtraBtn.gameObject.SetActive(true);
            opExtraBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Rodear la casa por atras";
            opExtraBtn.onClick?.AddListener(() =>
            {
                op1Btn.gameObject.SetActive(false);
                op2Btn.gameObject.SetActive(false);
                desitionText.text = "Decides rodear la casa por atras y encuentras un poco de aceite, al usarlo, tu llama se fortalece.";
                playerController.Heal(1);
                wasUsed = true;
                opExtraBtn.gameObject.SetActive(false);
                aceptarBtn.interactable = true;
            });
        }

        isfirstTime = false;
        desitionPanel.SetActive(true);
        desitionText.text = "Hay muchas sombras aterradoras frente a la casita del perro. ¿Quieres entrar?";

        o2Txt.text = "Entrar de todas formas";
        o1Txt.text = "No, mejor no arriesgarse";

        op1Btn.gameObject.SetActive(true);
        op2Btn.gameObject.SetActive(true);


        op1Btn.onClick?.AddListener(() =>
            {
                desitionText.text = "Decides no entrar, esas sombras se ven demasiado amenazantes, tal vez te hagan daño";
                AudioManager.Instance.PlayEfecto("ButonSound");
                op1Btn.gameObject.SetActive(false);
                op2Btn.gameObject.SetActive(false);
                opExtraBtn.gameObject.SetActive(false);
                aceptarBtn.interactable = true;
            });
        op2Btn.onClick?.AddListener(() =>
            {
                desitionText.text = "A pesar de las sombras, decides entrar a la casita del perro...";

                AudioManager.Instance.musicaSource.Stop();
                AudioManager.Instance.PlayEfecto("Suspenso");
                desitionPanel.SetActive(false);
                camAnim.SetBool("casita", true);
                StartCoroutine(WaitAndShowPanelAfterAnim("NombreDelEstadoAnim"));
            });
    }
    private IEnumerator WaitAndShowPanelAfterAnim(string animStateName)
    {
        yield return null;
        yield return new WaitForSecondsRealtime(5f);
        desitionPanel.SetActive(true);
        desitionText.text = "Obtienes una ganzúa, perfecta para forzar cerraduras.";
        GameManager.Instance.AddToInventory("LlaveSecreta");
        AudioManager.Instance.musicaSource.Play();
        puertaOcultaBtn.interactable = true;

        aceptarBtn.interactable = true;
        op1Btn.gameObject.SetActive(false);
        op2Btn.gameObject.SetActive(false);
        opExtraBtn.gameObject.SetActive(false);
    }
    private IEnumerator WaitAndOpenDoor()
    {
        yield return null;
        yield return new WaitForSecondsRealtime(5f);
        desitionPanel.SetActive(false);
        transitionAnim.SetTrigger("avanzar");
        SceneManager.LoadScene("Nivel3");
    }
    public void ClickObjSinOpciones(string obj)
    {
        if (obj == "cobertizo")
        {
            desitionText.text = "¿Por qué irias alli de nuevo? :p";
        }
        if (obj == "casa")
        {
            AudioManager.Instance.PlayEfecto("PuertaPrincipalLv2");
            desitionText.text = "Por mas que intentas, la puerta no abre";
        }
        desitionPanel.SetActive(true);
        aceptarBtn.interactable = true;
        o1Txt.text = "Aceptar";
        op2Btn.gameObject.SetActive(false);
        op1Btn.gameObject.SetActive(false);

    }

    private GameObject FindInactiveWithName(string tag)
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == tag)
            {
                return obj;
            }
        }
        return null;
    }
}
