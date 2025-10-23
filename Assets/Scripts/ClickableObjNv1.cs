using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickableObjNv1 : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    [SerializeField] private GameObject desitionPanel;
    [SerializeField] private Button llaveButton;
    [SerializeField] private Button op1Btn;
    [SerializeField] private Button op2Btn;
    private TextMeshProUGUI desitionText;
    private TMP_Text o1Txt;
    private TMP_Text o2Txt;
    [SerializeField] private Button aceptarBtn;
    private GameObject transition;
    private Animator transitionAnim;

    [Header("Puerta")]
    private bool doorisOpen = false;
    [SerializeField] private Animator doorAnim;

    [Header("Aceite")]
    [SerializeField] private Image aceiteObj;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        desitionPanel = FindInactiveWithName("PanelDialogo");
        desitionText = desitionPanel.GetComponentInChildren<TextMeshProUGUI>();
        op1Btn = FindInactiveWithName("op1").GetComponent<Button>();
        op2Btn = FindInactiveWithName("op2").GetComponent<Button>();
        aceptarBtn = FindInactiveWithName("AceptarBTN").GetComponent<Button>();
        o1Txt = op1Btn.GetComponentInChildren<TMP_Text>();
        o2Txt = op2Btn.GetComponentInChildren<TMP_Text>();
        llaveButton = FindInactiveWithName("Llave").GetComponent<Button>();
        llaveButton.interactable = true;
        transition = GameObject.Find("Transcicsion");
        transitionAnim = transition.GetComponent<Animator>();
        doorAnim = GameObject.Find("Puerta").GetComponent<Animator>();
        aceiteObj = GameObject.Find("Aceite").GetComponent<Image>();

        aceptarBtn.interactable = false;
    }
    public void ClickLlave()
    {
        desitionPanel.SetActive(true);
        desitionText.text = "Las sombras rodean la llave";
        o1Txt.text = "Tomar la llave de todas formas";
        o2Txt.text = "Ignorar la llave";
        op1Btn.gameObject.SetActive(true);
        op2Btn.gameObject.SetActive(true);
        aceptarBtn.interactable = false;

        //Eliminar los listeners previos para evitar duplicados
        op2Btn.onClick?.RemoveAllListeners();
        op1Btn.onClick?.RemoveAllListeners();

        op1Btn.onClick?.AddListener(() =>
            {
                desitionText.text = "Aún con mucho miedo, tomas la llave";
                GameManager.Instance.AddToInventory("Llave");
                AudioManager.Instance.PlayEfecto("Llave");
                llaveButton.image.color = new Color(llaveButton.image.color.r, llaveButton.image.color.g, llaveButton.image.color.b, 255);
                llaveButton.interactable = false;
                op1Btn.gameObject.SetActive(false);
                op2Btn.gameObject.SetActive(false);
                aceptarBtn.interactable = true;

            });

        op2Btn.onClick?.AddListener(() =>
            {
                AudioManager.Instance.PlayEfecto("ButonSound");
                desitionText.text = "El miedo te paraliza, decides no tomar la llave";
                op1Btn.gameObject.SetActive(false);
                op2Btn.gameObject.SetActive(false);
                aceptarBtn.interactable = true;
            });
    }
    public void ClickPuerta()
    {
        if (GameManager.Instance.inventory.ContainsKey("Llave") && !doorisOpen)
        {
            AudioManager.Instance.PlayEfecto("PuertaLV1Open");
            doorisOpen = true;
            doorAnim.SetTrigger("Abrir");
        }
        else
        {
            if (!doorisOpen)
            {
                AudioManager.Instance.PlayEfecto("PuertaLv1Cerrada");
                desitionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "La puerta está cerrada, necesitas una llave.";
                desitionPanel.SetActive(true);
                op2Btn.gameObject.SetActive(false);
                op1Btn.gameObject.SetActive(false);
                aceptarBtn.interactable = true;
            }
            else
            {
                transitionAnim.SetTrigger("avanzar");
                SceneManager.LoadScene("Nivel2");
            }
        }
    }

    public void ClickAceite()
    {
        desitionPanel.SetActive(true);
        desitionText.text = "Usas el aceite para encender tu llama";
        op1Btn.gameObject.SetActive(false);
        op2Btn.gameObject.SetActive(false);
        aceptarBtn.interactable = true;
        aceiteObj.color = new Color(aceiteObj.color.r, aceiteObj.color.g, aceiteObj.color.b, 255);

        UnityEngine.Events.UnityAction accionHeal = null;
        accionHeal = () =>
        {
            playerController.Heal(1);
            aceptarBtn.onClick.RemoveListener(accionHeal); // Se quita a sí misma después de ejecutarse
        };
        aceptarBtn.onClick.AddListener(accionHeal);
    }
    public void ClickVentana()
    {
        //Reproducir sonido de viento
        desitionPanel.SetActive(true);
        desitionText.text = "Al acercarte a la ventana, el viento sopla con fuerza, debilitando tu llama";
        AudioManager.Instance.PlayEfecto("BrisaVentana");
        op1Btn.gameObject.SetActive(false);
        op2Btn.gameObject.SetActive(false);
        aceptarBtn.interactable = true;

        UnityEngine.Events.UnityAction accionDanio = null;
        accionDanio = () =>
        {
            playerController.TakeDamage(1);
            aceptarBtn.onClick.RemoveListener(accionDanio); // Se quita a sí misma después de ejecutarse
        };
        aceptarBtn.onClick.AddListener(accionDanio);
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
