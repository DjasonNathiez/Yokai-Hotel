using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KF_ShopInteractManager : MonoBehaviour
{
    public KF_ShopDialogue objectDescription;
    public Queue<string> nameObj;
    public Queue<string> description;
    public Queue<string> price;


    [Header("==== Other Stuff ====")]
    public Animator dialogueAnim; // to be filled manualy
    public Sprite objectBox; //to be filled manually
    public Sprite dialogueBox; //to be filled manually
    public GameObject normalDialogueElements;
    public GameObject shopDialogueElements;
    private KF_LevelManager lvlM;
    private Image dialogueBoxSP;
    private Text nameText;
    private Text descText;
    private Text priceText;
    public List<GameObject> objectsInLevel;
    public float textSpeed;


    // Start is called before the first frame update
    void Start()
    {
        nameObj = new Queue<string>();
        description = new Queue<string>();
        price = new Queue<string>();
        lvlM = FindObjectOfType<KF_LevelManager>();
        dialogueBoxSP = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Image>();
        dialogueAnim = GameObject.FindGameObjectWithTag("DialogueAnim").GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((lvlM.levelChanged == true) || (lvlM.hubReturn == true))
        {
            objectsInLevel.Clear();
            foreach (GameObject interact in GameObject.FindGameObjectsWithTag("ShopInteract"))
            {
                objectsInLevel.Add(interact);
            }
            lvlM.levelChanged = false;
        }
    }

    public void StartShopInteract(KF_ShopDialogue shope)
    {
        normalDialogueElements.SetActive(false);
        shopDialogueElements.SetActive(true);
        nameText = GameObject.FindGameObjectWithTag("ObjectName").GetComponent<Text>();
        descText = GameObject.FindGameObjectWithTag("ObjectDesc").GetComponent<Text>();
        priceText = GameObject.FindGameObjectWithTag("ObjectPrice").GetComponent<Text>();

        dialogueBoxSP.sprite = objectBox;
        var tempColor = dialogueBoxSP.color;
        tempColor.a = 255f;
        dialogueBoxSP.color = tempColor;

        dialogueAnim.SetBool("isOpen", true);
        nameObj.Clear();
        description.Clear();
        price.Clear();

        foreach (string namee in shope.nameObj)
        {
            nameObj.Enqueue(namee);
        }
        foreach (string desc in shope.description)
        {
            description.Enqueue(desc);
        }
        foreach (string pricee in shope.price)
        {
            price.Enqueue(pricee);
        }
        DisplayText();
    }

    public void DisplayText()
    {
        string nameString = nameObj.Dequeue();
        string descString = description.Dequeue();
        string priceString = price.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeName(nameString));
        StartCoroutine(TypeDesc(descString));
        StartCoroutine(TypePrice(priceString));
    }

    public void WalkedAway()
    {
        dialogueAnim.SetBool("isOpen", false);
        shopDialogueElements.SetActive(false);
        normalDialogueElements.SetActive(true);
        nameText.text = "";
        descText.text = "";
        priceText.text = "";
    }

    private IEnumerator TypeName(string nameString)
    {
        nameText.text = "";
        foreach (char letter in nameString.ToCharArray())
        {
            nameText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    private IEnumerator TypeDesc(string descString)
    {
        descText.text = "";
        foreach (char letter in descString.ToCharArray())
        {
            descText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    private IEnumerator TypePrice(string priceString)
    {
        priceText.text = "";
        foreach (char letter in priceString.ToCharArray())
        {
            priceText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
