using UnityEngine;
public class Content
{
    float money;
    int count;
    ScriptableObject data;

    public float Money { get { return money; } }
    public int Count { get { return count; } }
    public ScriptableObject Data { get { return data; } }

    public Content(float money, int count, ScriptableObject data) 
    {
        this.money = money;
        this.count = count;
        this.data = data;
    }
}
public class ChestContent : MonoBehaviour
{
    [Header("Rango de dinero dado")]
    [SerializeField] private float maxMoney;
    [SerializeField] private float minMoney;

    [Header("Rango de cantidad de un objeto dado")]
    [SerializeField] private int maxCount;
    [SerializeField] private int minCount;

    public Content chest()
    {
        ObjectData[] objectDatas = Resources.LoadAll<ObjectData>("Data/Objects"); // Busca todos loas assets que empiezan con "OBJ_".

        int obj = Random.Range(0, objectDatas.Length); // El objeto que se selecciona de la lista.
        int count = Random.Range(minCount, maxCount + 1); // La cantidad que se le da de dicho objeto.
        float money = (float) System.Math.Round(Random.Range(minMoney, maxMoney + 1), 2); // La cantidad de dinero que se dara.

        Content content = new Content(money, count, objectDatas[obj]); // Se guarda en la clase Content

        return content; // Se devuelve la clase dicha.
    }
}
